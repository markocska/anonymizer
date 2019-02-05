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
	@where nvarchar(MAX) = '',
	@constant_columns as dbo.ConstantColumnList readonly,
	@scrambled_columns as dbo.ColumnList readonly
	)
	as 
	begin

SET XACT_ABORT, NOCOUNT ON

declare @full_table_name nvarchar(300) = ''

declare  @error_message nvarchar(500), @operation nvarchar(200) 
declare @param_definition nvarchar(MAX), @proc nvarchar(MAX) = ''

declare @get_primary_keys_cmd nvarchar(MAX)
declare @primary_keys_table table ([column_name] nvarchar(200), [column_type] nvarchar(200))
declare @i int, @maxrows int
declare @column_name nvarchar(200) = '', @column_type nvarchar(200) = '', @primary_keys nvarchar(MAX) = '',
		@const_column_value nvarchar(MAX) = '', @const_column_type nvarchar(128) = '',
		@const_columns_in_clause nvarchar(MAX) = ''
declare @const_columns_with_types table ([column_name] nvarchar(128), [column_type] nvarchar(128))

declare @scrambled_columns_with_types table ([column_name] nvarchar(128), [column_type] nvarchar(128))

declare @check_script nvarchar(MAX), @check_count int

declare @sql_to_describe NVARCHAR(MAX) = '',
		@sql_to_get_type nvarchar(MAX) = ''

declare @primary_keys_with_types nvarchar(MAX) = '',
		@update_where_clause nvarchar(MAX) = ''

declare	@non_clustered_indexes table ([index_name] nvarchar(128))
declare @sql_to_get_clust_indexes nvarchar(MAX) = '',
		@sql_to_turn_off_clust_indexes varchar(MAX) = '',
		@sql_to_turn_on_clust_indexes varchar(MAX) = ''

-- Surrounding the parameter names with [ ] so we won't run into errors regarding the naming.
	Select @db = '[' + @db + ']',
		   @schema = '[' + @schema + ']',
		   @tablep = '[' + @tablep + ']';

	Set @full_table_name = @db + '.' + @schema + '.' + @tablep

BEGIN TRY
-- Parameter checkings
	
	--set @operation = 'checking the input parameters.'

	--if OBJECT_ID(@db + '.' + @schema + '.' + @tablep) IS NULL
	--	begin		
	--		set @error_message = 'The table ' + @db + '.' + @schema + '.' + @tablep + ' does not exist.';
	--		raiserror(@error_message,16,1);
	--	end


	--set @check_script =
	--					'SELECT @check_count_out = count(1) '
	--					+ 'FROM '   
	--						+ @db + '.sys.columns c '
	--					+ 'WHERE
	--						c.object_id = OBJECT_ID(''' + @db + '.' + @schema + '.' + @tablep + ''') and ' + 'c.[name] = ' + '''' + @column_name + '''';



	--set @param_definition = '@check_count_out int OUTPUT';
	--set @check_count = 0;
	--exec sp_executesql @check_script, @param_definition, @check_count_out=@check_count OUTPUT;

	--if @check_count = 0
	--	 begin
	--		 Set @error_message = 'The given column with name ' + upper(@column_name) + ' does not exist in the table ' 
	--				+ @db + '.' + @schema + '.' + @tablep;  
	--		 raiserror(@error_message,16,1)
	--	 end

--------

--- Get the primary key/keys's name and type
	
	set @operation = 'Getting the primary keys and their types.'

	set @get_primary_keys_cmd =	'(select c.name
		from ' + @db + '.sys.columns c
			inner join ' + @db + '.' + 'sys.index_columns ic on c.object_id = ic.object_id and c.column_id = ic.column_id
			inner join ' + @db + '.' + 'sys.indexes i on ic.object_id = i.object_id and ic.index_id = i.index_id
			where 
				i.is_primary_key = 1 and 
				c.object_id = OBJECT_ID(''' + @db + '.' + @schema + '.' + @tablep + '''))' ;

	set @sql_to_describe = 'select * from ' + @db + '.' +  @schema + '.' + @tablep 

	set @sql_to_get_type = 
	'select	name, system_type_name
		FROM ' + @db + '.sys.dm_exec_describe_first_result_set(''' + @sql_to_describe + ' '', NULL, 1)
		where name in ' + @get_primary_keys_cmd;

	insert into @primary_keys_table exec (@sql_to_get_type) 
----

----Create the terms with the primary key/keys for the scrambling SQLs

	select @maxrows = count(1), @i = 1 from @primary_keys_table

	while @maxrows >= @i
	begin
		select @column_name = column_name , @column_type = column_type from @primary_keys_table order by column_name offset @i-1 row fetch next 1 row only

		if @i <> @maxrows
			begin
				set @primary_keys = @primary_keys + @column_name + ','
				set @primary_keys_with_types = @primary_keys_with_types + @column_name + ' ' + @column_type + ',' 
				set @update_where_clause = @update_where_clause + @db + '.' + @schema + '.' + @tablep + '.' + @column_name + '= x.' + @column_name + ' and '
			end
		else 
			begin
				set @primary_keys = @primary_keys + @column_name
				set @primary_keys_with_types = @primary_keys_with_types + @column_name + ' ' + @column_type
				set @update_where_clause = @update_where_clause + @db + '.' + @schema + '.' + @tablep + '.' + @column_name + '= x.' + @column_name
			end

		set @i = @i + 1
	end

--------
--------Get the constant columns's types
	
	set @operation = 'Getting the constant column''s types.'
	
	declare @const_column_name_list dbo.ColumnList,
			@sql_to_get_constant_types nvarchar(MAX),
			@const_count int 

	insert into @const_column_name_list 
	select c.column_name
	from @constant_columns c

	exec @sql_to_get_constant_types = dbo.func_get_column_types @columns = @const_column_name_list, @db=@db, @schema = @schema, @table = @tablep

	select @const_count = COUNT(1) from @constant_columns

	if (@const_count <> 0)
		insert into @const_columns_with_types exec (@sql_to_get_constant_types)

--------
-------- Create the constant column clauses
	 declare @const_update_clause nvarchar(MAX) = ''
	
	select @maxrows = count(1), @i = 1 from @constant_columns

	while @maxrows >= @i
	begin
		select @column_name = c.column_name, @const_column_type = t.[column_type], @const_column_value = c.column_value
		from @constant_columns c join @const_columns_with_types t on c.column_name = t.column_name
		order by c.column_name offset @i-1 row fetch next 1 row only

		set @const_update_clause = @const_update_clause + 
		' , ' + '[' + @column_name + ']' + ' = ' + 'cast(''' + @const_column_value + ''' as ' + @const_column_type + ') '

		set @i = @i + 1
	end

--------
-------- Get the scrambled columns's types 

	set @operation = 'Getting the scrambled columns''s types.'
	
	declare @sql_to_get_scrambled_types nvarchar(MAX)

	exec @sql_to_get_scrambled_types = dbo.func_get_column_types @columns = @scrambled_columns, @db=@db, @schema = @schema, @table = @tablep

	insert into @scrambled_columns_with_types exec (@sql_to_get_scrambled_types)

	update @scrambled_columns_with_types
	set column_name = '[' + column_name + ']'

--------
-------- Create the scrambled column clauses
	declare @create_table_clauses nvarchar(MAX) = '', @column_names_clause nvarchar(MAX) = '',
			@insert_into_table_clauses nvarchar(MAX) = '', @join_tables_clause nvarchar(MAX) = '',
			@update_select_clause nvarchar(MAX) = '', @drop_tables_clause nvarchar(MAX) = '',
			@create_indexes_clause nvarchar(MAX) = '', @scrambled_update_clause nvarchar(MAX) = '',
			@column_names_with_types_clause nvarchar(MAX) = ''

	select * from @scrambled_columns_with_types;
	select @maxrows = COUNT(1), @i = 1 from @scrambled_columns_with_types

	while @maxrows >= @i
	begin
		select @column_name = s.column_name, @column_type = s.column_type
		from @scrambled_columns_with_types s
		order by s.column_name offset @i-1 row fetch next 1 row only

		select @create_table_clauses = @create_table_clauses + ' ' + CHAR(13) + CHAR(10) + 
		 'create table #column' + convert(nvarchar, @i) + '
		 (
			random int, ' 
			+ @column_name + ' ' + @column_type + ' 
		 ) '

		 select @column_names_with_types_clause = @column_names_with_types_clause + ' ' + @column_name + ' ' + @column_type +  ', '

		 select @column_names_clause = @column_names_clause + ' ' + @column_name + ', '

		 select @insert_into_table_clauses = @insert_into_table_clauses + ' ' + CHAR(13) + CHAR(10) +
		 ' insert into #column' + CONVERT(nvarchar, @i) + ' with (tablock)
				select row_number () over (order by x), ' + @column_name 
			+ ' from (select CHECKSUM(NewId()) x, ' + @column_name + ' from #prim_keys_and_columns) a ; '

		select @create_indexes_clause = @create_indexes_clause + ' ' + CHAR(13) + CHAR(10) +
			'CREATE INDEX IDX_COLUMN' + CONVERT(nvarchar, @i) + '_RANDOM ON #COLUMN' + convert(nvarchar, @i) + '(random);'

		select @scrambled_update_clause = @scrambled_update_clause + ' ' + CHAR(13) + CHAR(10) +
			@column_name + ' = x.' + @column_name + case when @i <> @maxrows then ', ' else ' ' end
			
		 select @update_select_clause = @update_select_clause + ', ' +
			'#column' + CONVERT(nvarchar, @i) + '.' + @column_name + ' '
		
		 select @join_tables_clause = @join_tables_clause + ' ' + CHAR(13) + CHAR(10) + ' 
			join #column' + CONVERT(nvarchar, @i) + ' on #prim_keys_and_columns.rownum = #column' + CONVERT(nvarchar, @i) + '.random '

		select @drop_tables_clause = @drop_tables_clause + ' ' + CHAR(13) + CHAR(10) + 
			'drop table #column' + CONVERT(nvarchar, @i) + ' ;'

		set @i = @i + 1
	end
--------
END TRY
BEGIN CATCH
	
	set @error_message  ='Error while ' +  @operation + ' - ' + error_message() + '. Line: '
			+ convert(nvarchar,error_line())
	raiserror(@error_message,16,1)

END CATCH

BEGIN TRY
-------- Disable all non-clustered indexes
	set @operation = 'Disabling all the non-clustered indexes'
	exec dbo.sp_EnableDisableClusteredIndexes @db=@db, @schema = @schema, @table_name = @tablep, @enable = 0
--------

--------Doing the scrambling
	
	set @operation = 'Doing the scrambling.'

	set @proc = 
	 'ALTER TABLE ' + @db + '.' + @schema + '.' + @tablep + ' DISABLE TRIGGER ALL '
+	'create table #prim_keys_and_columns
	(
		'	+ @primary_keys_with_types + ','
			+ @column_names_with_types_clause
		+ ' rownum int 
	) '
+	@create_table_clauses

	set @proc = @proc + 
		' insert into #prim_keys_and_columns with (tablock)
		select ' + @primary_keys + ', '
		+	   @column_names_clause
		+	  'row_number () over (order by ' + @primary_keys +')
		from ' + @db + '.' + @schema + '.' + @tablep + ' ' + @where +

		@insert_into_table_clauses + ' 
		
		CREATE INDEX IDX_PRIMARYKEYS ON #prim_keys_and_columns(' + @primary_keys + '); ' +
		@create_indexes_clause  
	
	print @const_update_clause
	set @proc = @proc + ' update ' + @db + '.' + @schema + '.' + @tablep + 'with (tablock)' +
	' set ' + @scrambled_update_clause + ' '
	+ @const_update_clause +
	' from
	(
		select ' + @primary_keys + 
		  @update_select_clause
		+ ' from #prim_keys_and_columns ' +
		@join_tables_clause + ' 
	) x
	where ' + @update_where_clause + ';

	drop table #prim_keys_and_columns; ' +
	@drop_tables_clause 
	+
		' ALTER TABLE ' + @db + '.dbo.' + @tablep + ' ENABLE TRIGGER ALL'
	
	set @operation = 'doing the scrambling.'
	print @proc
	exec (@proc)
---------

-------- Enable all non-clustered indexes
	set @operation = 'Enabling all the non-clustered indexes'
	exec dbo.sp_EnableDisableClusteredIndexes @db=@db, @schema = @schema, @table_name = @tablep, @enable = 1

END TRY
BEGIN CATCH
	
	exec dbo.sp_EnableDisableClusteredIndexes @db=@db, @schema = @schema, @table_name = @tablep, @enable = 1
	set @error_message ='Error while ' +  @operation + ' - ' + error_message() + '. Line: '
			+ convert(nvarchar,error_line())
	raiserror(@error_message,16,1)

END CATCH

end
