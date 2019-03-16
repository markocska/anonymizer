
/****** Object:  StoredProcedure [dbo].[sp_SimpleAnonymizer]    Script Date: 2019. 01. 17. 10:35:57 ******/
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON

-- =============================================
-- Author:		Mark Rusz
-- Create date: 2019-01-15
-- Description:	Scrambles the given columns and sets other given columns to a constant value for a table
-- =============================================

SET XACT_ABORT, NOCOUNT ON

--Input parameters
declare @db nvarchar(128), @schema nvarchar(128), @table nvarchar(128)
declare @constant_columns table(column_name nvarchar(128))
declare @scrambled_columns table(column_name nvarchar(128))

insert into @constant_columns 
select column_name 
from @const_columns_table

insert into @scrambled_columns
select column_name
from @scrambled_columns_table

set @db = @db_value
set @schema = @schema_value
set @table = @table_value

--Variables to store the error messages and the operation that is currently done
declare  @error_message nvarchar(500), @operation nvarchar(200) 

-- Surrounding the parameter names with [ ] so we won't run into errors regarding the naming.
Select @db = '[' + @db + ']',
		@schema = '[' + @schema + ']',
		@table = '[' + @table + ']';
	
declare @full_table_name nvarchar(300) = @db + '.' + @schema + '.' + @table

BEGIN TRY

-- Primary key checkings

	-- We create a table that contains all the primary key columns and unique constraint columns

	declare @check_script nvarchar(MAX) = '', @check_count int
	declare @all_columns_list table ([column_name] nvarchar(128))

	insert into @all_columns_list 
		select column_name from @constant_columns
		union 
		select column_name from @scrambled_columns

	declare @unique_pr_column_list table([column_name] nvarchar(128), [is_unique_constraint] nvarchar(128), 
										[is_primary_key] nvarchar(128))

	set @check_script =	 '(select c.name, i.is_unique_constraint, i.is_primary_key
			    from ' + @db + '.sys.columns c
				inner join ' + @db + '.' + 'sys.index_columns ic on c.object_id = ic.object_id and c.column_id = ic.column_id
				inner join ' + @db + '.' + 'sys.indexes i on ic.object_id = i.object_id and ic.index_id = i.index_id
				where 
			     	(i.is_unique_constraint = 1 or i.is_primary_key = 1)
				and i.is_disabled = 0
				and	c.object_id = OBJECT_ID(''' + @db + '.' + @schema + '.' + @table + '''))'
	
	insert into @unique_pr_column_list exec (@check_script)
		
	--- We check whether the table has a primary key

	set @operation = 'checking if the table has a primary key'

	select @check_count = count(1) from @unique_pr_column_list l where l.is_primary_key = 1

	if @check_count = 0
	begin
		set @error_message = 'The table ' + @db + '.' + @schema + '.' + @table + ' has no primary key!'
		raiserror(@error_message, 16, 1)
	end

	-- We check whether any of the input columns is part of a primary key or a unique constraint
	
	declare @input_unique_intersection table([column_name] nvarchar(128), [is_unique_constraint] nvarchar(128),
											 [is_primary_key] nvarchar(128))

	insert into @input_unique_intersection 
	select up.column_name, up.is_primary_key, up.is_unique_constraint
	from @unique_pr_column_list up 
	join @all_columns_list al on up.column_name = al.column_name 

	select @check_count = count(1) from @input_unique_intersection

	if @check_count <> 0 
	begin
		declare @unique_columns_str nvarchar(max) = '', @prkey_columns_str nvarchar(max) = ''
		
		select @unique_columns_str = @unique_columns_str + ' [' + column_name + '] ' 
		from @input_unique_intersection i 
		where i.is_primary_key = 1

		if @unique_columns_str <> ''
		begin
			set @error_message = 'The following input columns are part of a unique constraint: ' + @unique_columns_str + ' .' + CHAR(13) + CHAR(11)
		end

		select @prkey_columns_str = @prkey_columns_str + ' [' + column_name + '] '
		from @input_unique_intersection i 
		where i.is_unique_constraint = 1

		if @prkey_columns_str <> ''
		begin
			set @error_message = 'The following input columns are part of a primary key constraint: ' + @prkey_columns_str + ' .' + CHAR(13) + CHAR(11) 
		end

		raiserror(@error_message, 16, 1)
	end

END TRY
BEGIN CATCH
	
	set @error_message  ='Error while ' +  @operation + ' - ' + error_message() + '. Line: '
			+ convert(nvarchar,error_line())
	raiserror(@error_message,16,1)
	return

END CATCH