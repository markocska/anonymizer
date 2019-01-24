USE [Anonymizer]
GO
/****** Object:  StoredProcedure [dbo].[sp_SimpleAnonymizer]    Script Date: 2019. 01. 17. 10:35:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		Mark Rusz
-- Create date: 2019-01-15
-- Description:	Scrambles a column of a given table
-- =============================================
ALTER PROCEDURE [dbo].[sp_SimpleAnonymizer] (
	-- Add the parameters for the stored procedure here
	@db nvarchar(100),
	@schema nvarchar(100),
	@tablep nvarchar(100),
	@column_name nvarchar(100),
	@where nvarchar(MAX) = ''
	)
	as 
	begin

SET XACT_ABORT, NOCOUNT ON


declare  @error_message nvarchar(500), @operation nvarchar(200) 
declare @param_definition nvarchar(MAX), @proc nvarchar(MAX)

declare @get_primary_keys_cmd nvarchar(MAX)
declare @primary_keys_table table ([column_name] nvarchar(200), [column_type] nvarchar(200))
declare @i int, @maxrows int
declare @name nvarchar(200), @type nvarchar(200), @primary_keys nvarchar(MAX) = ''

declare @check_script nvarchar(MAX), @check_count int

declare @sql_to_describe NVARCHAR(MAX) = '',
		@sql_to_get_type nvarchar(MAX), @column_type nvarchar(100)

declare @primary_keys_with_types nvarchar(MAX) = '',
		@update_where_clause nvarchar(MAX) = ''


-- Surrounding the parameter names with [ ] so we won't run into errors regarding the naming.
	Select @db = '[' + @db + ']',
		   @schema = '[' + @schema + ']',
		   @tablep = '[' + @tablep + ']';

BEGIN TRY
-- Parameter checkings
	
	set @operation = 'checking the input parameters.'

	if OBJECT_ID(@db + '.' + @schema + '.' + @tablep) IS NULL
		begin		
			set @error_message = 'The table ' + @db + '.' + @schema + '.' + @tablep + ' does not exist.';
			raiserror(@error_message,16,1);
		end


	set @check_script =
						'SELECT @check_count_out = count(1) '
						+ 'FROM '   
							+ @db + '.sys.columns c '
						+ 'WHERE
							c.object_id = OBJECT_ID(''' + @db + '.' + @schema + '.' + @tablep + ''') and ' + 'c.[name] = ' + '''' + @column_name + '''';



	set @param_definition = '@check_count_out int OUTPUT';
	set @check_count = 0;
	exec sp_executesql @check_script, @param_definition, @check_count_out=@check_count OUTPUT;

	if @check_count = 0
		 begin
			 Set @error_message = 'The given column with name ' + upper(@column_name) + ' does not exist in the table ' 
					+ @db + '.' + @schema + '.' + @tablep;  
			 raiserror(@error_message,16,1)
		 end

--------

--- Get the primary key/keys's name and type


	set @get_primary_keys_cmd =	'(select c.name
		from target_ifuat.sys.columns c
			inner join ' + @db + '.' + 'sys.index_columns ic on c.object_id = ic.object_id and c.column_id = ic.column_id
			inner join ' + @db + '.' + 'sys.indexes i on ic.object_id = i.object_id and ic.index_id = i.index_id
			where 
				i.is_primary_key = 1 and 
				c.object_id = OBJECT_ID(''' + @db + '.' + @schema + '.' + @tablep + '''))' ;

	set @sql_to_describe = 'select * from ' + @db + '.' +  @schema + '.' + @tablep 

	set @sql_to_get_type = 
	'select	name, system_type_name
		FROM target_ifuat.sys.dm_exec_describe_first_result_set(''' + @sql_to_describe + ' '', NULL, 1)
		where name in ' + @get_primary_keys_cmd;

	set @operation = 'Getting the primary keys and their types.'

	insert into @primary_keys_table exec (@sql_to_get_type) 

-- Get the scrambled columns's type 

	set @sql_to_describe = 'select * from target_ifuat.dbo.t_actors';
	set @param_definition = '@sql nvarchar(MAX), @column nvarchar(100), @type_out nvarchar(100) OUTPUT'

	set @sql_to_get_type = 
	'	SELECT @type_out = system_type_name
		FROM target_ifuat.sys.dm_exec_describe_first_result_set(@sql, NULL, 1)
		where name = @column';

	set @operation = 'getting the scrambled column''s type.'		
	exec sp_executesql @sql_to_get_type, @param_definition, @sql=@sql_to_describe, @column=@column_name, @type_out=@column_type OUTPUT;
---

----Create the terms with the primary key/keys for the scrambling SQLs

	select @maxrows = count(1), @i = 1 from @primary_keys_table

	while @maxrows >= @i
	begin
		select @name = column_name , @type = column_type from @primary_keys_table order by column_name offset @i-1 row fetch next 1 row only

		if @i <> @maxrows
			begin
				set @primary_keys = @primary_keys + @name + ','
				set @primary_keys_with_types = @primary_keys_with_types + @name + ' ' + @type + ',' 
				set @update_where_clause = @update_where_clause + @db + '.' + @schema + '.' + @tablep + '.' + @name + '= x.' + @name + ' and '
			end
		else 
			begin
				set @primary_keys = @primary_keys + @name
				set @primary_keys_with_types = @primary_keys_with_types + @name + ' ' + @type
				set @update_where_clause = @update_where_clause + @db + '.' + @schema + '.' + @tablep + '.' + @name + '= x.' + @name
			end

		set @i = @i + 1
	end

--------

    set @column_name = '[' + @column_name + ']';

	
	set @proc = 
	N'ALTER TABLE ' + @db + '.' + @schema + '.' + @tablep + ' DISABLE TRIGGER ALL '
+	'create table #t1
	(
		'	+ @primary_keys_with_types + ','
			+ @column_name + ' ' + @column_type + ', '
		+ ' rownum int 
	) '
+	'create table #t2
	(
		random int, ' 
		+ @column_name + ' ' + @column_type + ' '
+	') '
	
	
	set @proc = @proc + 
		' insert into #t1
		select ' + @primary_keys + ', '
		+	   @column_name + ', '
		+	  'row_number () over (order by ' + @primary_keys +')
		from ' + @db + '.' + @schema + '.' + @tablep + ' ' + @where +

		' insert into #t2
		select row_number () over (order by x), ' + @column_name 
			+ ' from (select CHECKSUM(NewId()) x, ' + @column_name + 'from #t1) a ; 

		update #t1
		set ' + @column_name + ' = x.new_value
		from
		(
			select random r, ' + @column_name + ' new_value from #t2
		) x where x.r = rownum ;
		
		truncate table #t2	'			 

	set @proc = @proc + ' update ' + @db + '.' + @schema + '.' + @tablep +
	' set ' + @column_name + ' = x.' + @column_name + ' '
	+
	' from
	(
		select ' + @primary_keys + ' , '
					+ @column_name
		+ ' from #t1
	) x
	where ' + @update_where_clause + ';

	drop table #t1
	drop table #t2 '
	+
		'ALTER TABLE ' + @db + '.dbo.' + @tablep + ' ENABLE TRIGGER ALL'
	
	set @operation = 'doing the scrambling.'
	print @proc
	exec (@proc)	
END TRY
BEGIN CATCH

	if @@TRANCOUNT > 0 rollback transaction
	declare @msg nvarchar(MAX) ='Error while ' +  @operation + ' - ' + error_message() + '. Line: '
			+ convert(nvarchar,error_line())
	raiserror(@msg,16,1)

END CATCH

end
