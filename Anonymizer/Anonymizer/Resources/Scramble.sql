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
-- Description:	Scrambles the given columns and sets other given columns to a constant value for a table
-- =============================================


SET XACT_ABORT, NOCOUNT ON

--Input parameters
declare @db nvarchar(128), @schema nvarchar(128), @table nvarchar(128),
		@where nvarchar(max)
declare @constant_columns table(column_name nvarchar(128), column_value nvarchar(max))
declare @scrambled_columns table(column_name nvarchar(128))

insert into @const_columns 
select column_name, column_value 
from @const_columns_and_values_table

insert into @scrambled_columns
select column_name
from @scrambled_columns_and_values_table

set @db = @db_value
set @schema = @schema_value
set @table = @table_value
set @where = @where_value

--Variables to store the error messages and the operation that is currently done
declare  @error_message nvarchar(500), @operation nvarchar(200) 

--We store the script that does that actual scrambling and that we will execute in this variable
declare  @proc nvarchar(MAX) = ''

--Loop variables
declare @i int, @maxrows int

--Variables for storing column names and types
declare @column_name nvarchar(128) = '', @column_type nvarchar(128) = ''

-- Varibles that we will store some scripts in, to get the column types for various columns
declare @sql_to_describe nvarchar(MAX) = '',
		@sql_to_get_type nvarchar(MAX) = ''		

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

	-- We create a table that contains all the primary key columns and unique constraint columns
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

--------

--- Get the primary key/keys's name and type
	
	set @operation = 'getting the primary keys and their types.'

	declare @get_primary_keys_cmd nvarchar(MAX)
	declare @primary_keys_table table ([column_name] nvarchar(200), [column_type] nvarchar(200))

	set @get_primary_keys_cmd =	'(select c.name
		from ' + @db + '.sys.columns c
			inner join ' + @db + '.' + 'sys.index_columns ic on c.object_id = ic.object_id and c.column_id = ic.column_id
			inner join ' + @db + '.' + 'sys.indexes i on ic.object_id = i.object_id and ic.index_id = i.index_id
			where 
				i.is_primary_key = 1 and 
				c.object_id = OBJECT_ID(''' + @db + '.' + @schema + '.' + @table + '''))' ;

	set @sql_to_describe = 'select * from ' + @db + '.' +  @schema + '.' + @table 

	set @sql_to_get_type = 
	'select	name, system_type_name
		FROM ' + @db + '.sys.dm_exec_describe_first_result_set(''' + @sql_to_describe + ' '', NULL, 1)
		where name in ' + @get_primary_keys_cmd;

	insert into @primary_keys_table exec (@sql_to_get_type) 

----

----Create the terms with the primary key/keys for the scrambling SQLs
	
	set @operation = 'creating the terms with the primary key/keys for the scrambling SQLs'

	declare @update_where_clause nvarchar(MAX) = '', @primary_keys_with_types nvarchar(MAX) = '',
		    @primary_keys nvarchar(MAX) = ''

	select @maxrows = count(1), @i = 1 from @primary_keys_table

	while @maxrows >= @i
	begin
		select @column_name = column_name , @column_type = column_type from @primary_keys_table order by column_name offset @i-1 row fetch next 1 row only

		if @i <> @maxrows
			begin
				set @primary_keys = @primary_keys + @column_name + ','
				set @primary_keys_with_types = @primary_keys_with_types + @column_name + ' ' + @column_type + ',' 
				set @update_where_clause = @update_where_clause + @db + '.' + @schema + '.' + @table + '.' + @column_name + '= x.' + @column_name + ' and '
			end
		else 
			begin
				set @primary_keys = @primary_keys + @column_name
				set @primary_keys_with_types = @primary_keys_with_types + @column_name + ' ' + @column_type
				set @update_where_clause = @update_where_clause + @db + '.' + @schema + '.' + @table + '.' + @column_name + '= x.' + @column_name
			end

		set @i = @i + 1
	end

--------
--------Get the constant columns's types
	
	set @operation = 'getting the constant column''s types.'
	
    declare @const_columns_with_types table ([column_name] nvarchar(128), [column_type] nvarchar(128))
	declare @const_column_name_list dbo.ColumnList,
			@sql_to_get_constant_types nvarchar(MAX),
			@const_count int 

	insert into @const_column_name_list 
	select c.column_name
	from @constant_columns c

	exec @sql_to_get_constant_types = dbo.func_get_column_types @columns = @const_column_name_list, @db=@db, @schema = @schema, @table = @table

	select @const_count = COUNT(1) from @constant_columns

	insert into @const_columns_with_types exec (@sql_to_get_constant_types)

--------
-------- Create the constant column clauses

	set @operation = 'creating the constant column clauses.'

	declare @const_update_clause nvarchar(MAX) = ''
	declare	@const_column_value nvarchar(MAX) = '', @const_column_type nvarchar(128) = ''
	
	select @maxrows = count(1), @i = 1 from @constant_columns

	while @maxrows >= @i
	begin
		select @column_name = c.column_name, @const_column_type = t.[column_type], @const_column_value = c.column_value
		from @constant_columns c join @const_columns_with_types t on c.column_name = t.column_name
		order by c.column_name offset @i-1 row fetch next 1 row only

		set @const_update_clause = @const_update_clause + 
		case when @i = 1 then ' ' else ' , ' end + '[' + @column_name + ']' + ' = ' + 'cast(''' + @const_column_value + ''' as ' + @const_column_type + ') '

		set @i = @i + 1
	end

--------
-------- Get the scrambled columns's types 

	set @operation = 'getting the scrambled columns''s types.'
	
	declare @sql_to_get_scrambled_types nvarchar(MAX)
	declare @scrambled_columns_with_types table ([column_name] nvarchar(128), [column_type] nvarchar(128))

	exec @sql_to_get_scrambled_types = dbo.func_get_column_types @columns = @scrambled_columns, @db=@db, @schema = @schema, @table = @table

	insert into @scrambled_columns_with_types exec (@sql_to_get_scrambled_types)

	update @scrambled_columns_with_types
	set column_name = '[' + column_name + ']'

--------
-------- Create the scrambled column clauses
	
	set @operation = 'creating the scrambled column clauses'

	declare @create_table_clauses nvarchar(MAX) = '', @column_names_clause nvarchar(MAX) = '',
			@insert_into_table_clauses nvarchar(MAX) = '', @join_tables_clause nvarchar(MAX) = '',
			@update_select_clause nvarchar(MAX) = '', @drop_tables_clause nvarchar(MAX) = '',
			@create_indexes_clause nvarchar(MAX) = '', @scrambled_update_clause nvarchar(MAX) = '',
			@column_names_with_types_clause nvarchar(MAX) = ''

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
			@column_name + ' = x.' + @column_name + case when @i = @maxrows and @const_count = 0 then ' ' else ', ' end
			
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
	return

END CATCH

BEGIN TRY

-------- Disable all non-clustered indexes
	--set @operation = 'disabling all the non-clustered indexes'
	--exec dbo.sp_EnableDisableClusteredIndexes @db=@db, @schema = @schema, @table_name = @table, @enable = 0
--------

--------Doing the scrambling
	
	set @operation = 'doing the scrambling.'

	set @proc = 
		 'ALTER TABLE ' + @db + '.' + @schema + '.' + @table + ' DISABLE TRIGGER ALL '
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
		from ' + @db + '.' + @schema + '.' + @table + ' ' + @where +

		@insert_into_table_clauses + ' 
		
		CREATE INDEX IDX_PRIMARYKEYS ON #prim_keys_and_columns(' + @primary_keys + '); ' +
		@create_indexes_clause  

	set @proc = @proc + 
		'alter table ' + @db + '.' + @schema + '.' + @table + ' nocheck constraint all; ' + CHAR(13) + CHAR(10) + 
				
		'exec dbo.sp_EnableDisableNon_PrKeyUniqueClust_Indexes @db=''' + @db + ''', @schema = ''' + @schema + ''', @table_name = ''' + @table + ''', @enable = 0;'

	set @proc = @proc + ' update ' + @db + '.' + @schema + '.' + @table + 'with (tablock)' +
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
			' ALTER TABLE ' + @db + '.' + @schema + '.' + @table + ' ENABLE TRIGGER ALL; ' + 
			' alter table ' + @db + '.' + @schema + '.' + @table + ' check constraint all; '
	
	set @operation = 'doing the scrambling.'
	print @proc
	exec (@proc)
---------

-------- Enable all non-clustered indexes
	set @operation = 'enabling all the non-clustered indexes'
	exec dbo.sp_EnableDisableNon_PrKeyUniqueClust_Indexes @db=@db, @schema = @schema, @table_name = @table, @enable = 1

END TRY
BEGIN CATCH
	
	exec dbo.sp_EnableDisableNon_PrKeyUniqueClust_Indexes @db=@db, @schema = @schema, @table_name = @table, @enable = 1
	set @error_message ='Error while ' +  @operation + ' - ' + error_message() + '. Line: '
			+ convert(nvarchar,error_line())
	raiserror(@error_message,16,1)
	return
END CATCH


