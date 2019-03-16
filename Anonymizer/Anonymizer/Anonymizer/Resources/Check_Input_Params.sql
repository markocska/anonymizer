

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

-- Parameter checkings
	
	set @operation = 'checking the input parameters.'
	
	declare @check_script nvarchar(MAX) = '', @check_count int,
			@param_definition nvarchar(MAX) = ''
	declare @section_check_table table ([column_name] nvarchar(128)) 
	declare @all_columns_list table ([column_name] nvarchar(128))

	-- Checking whether the are columns that appear both at the scrambled and constant columns
	insert into @section_check_table 
		select column_name from @constant_columns
		intersect
		select column_name from @scrambled_columns

	select @check_count = count(1) from @section_check_table;

	if @check_count <> 0
	begin
		declare @duplicated_columns nvarchar(MAX) = ''
		
		select @duplicated_columns =  @duplicated_columns + '[' + column_name + '], '
		from @section_check_table

		set @error_message = 'A column can''t appear both among the constant and scrambled columns. The following columns '
		+ 'appear in both: ' + @duplicated_columns + '.'

		raiserror(@error_message,16,1)
	end
	
	-- Checking whether the table to anonymize exists
	if OBJECT_ID(@db + '.' + @schema + '.' + @table) IS NULL
		begin		
			set @error_message = 'The table ' + @db + '.' + @schema + '.' + @table + ' does not exist.';
			raiserror(@error_message,16,1);
		end

	--Checking whether every column to anonymize exists in the table
	declare @all_columns_list_str nvarchar(MAX) = '', @not_existing_column_list_str nvarchar(MAX) = ''
	declare @not_existing_column_list table ([column_name] nvarchar(128))
	declare @existing_column_list table ([column_name] nvarchar(128))
	
	insert into @all_columns_list 
		select column_name from @constant_columns
		union 
		select column_name from @scrambled_columns

	select @all_columns_list_str = @all_columns_list_str + '''' + column_name + '''' + ',' 
		from @all_columns_list

	set @all_columns_list_str = stuff(@all_columns_list_str, len(@all_columns_list_str),1,'')

	set @check_script =	 ' SELECT c.[name] '
						+ 'FROM '   
							+ @db + '.sys.columns c '
						+ 'WHERE
							c.object_id = OBJECT_ID(''' + @db + '.' + @schema + '.' + @table + ''') and ' + 'c.[name] in (' + @all_columns_list_str + ')' ;

	insert into @existing_column_list exec (@check_script)

	insert into @not_existing_column_list
		select a.column_name from @all_columns_list a
		where not exists(
			select column_name from @existing_column_list e
			where e.column_name = a.column_name
		)


	select @check_count = count(1) from @not_existing_column_list

	if @check_count <> 0
	begin
		select @not_existing_column_list_str = @not_existing_column_list_str + '[' + column_name + '], ' from @not_existing_column_list
		set @error_message = 'The following columns doesn''t exist in the table: ' + @not_existing_column_list_str
		raiserror(@error_message, 16, 1)
	end
END TRY
BEGIN CATCH
	
	set @error_message  ='Error while ' +  @operation + ' - ' + error_message() + '. Line: '
			+ convert(nvarchar,error_line())
	raiserror(@error_message,16,1)
	return

END CATCH