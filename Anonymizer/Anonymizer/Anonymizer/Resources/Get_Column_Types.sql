/****** Object:  StoredProcedure [dbo].[sp_SimpleAnonymizer]    Script Date: 2019. 01. 17. 10:35:57 ******/
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON

	declare @columns as dbo.AnonymizerColumnList 
	--declare @columns_table as dbo.AnonymizerColumnList 
    declare @db as nvarchar(128), @schema as nvarchar(128), @table as nvarchar(128)

	select @db = @db_value, @schema = @schema_value, @table = @table_value

	insert into @columns 
	select column_name 
	from @columns_table

	--select @db = @db_value, @schema = @schema_value, @table = @table_value
	
	declare @column_count int
	select @column_count = count(1) from @columns

	if @column_count = 0
		select ''

	declare @sql_to_describe nvarchar(MAX) = '', @columns_in_clause nvarchar(MAX) = '',
			@sql_to_get_type nvarchar(MAX) = ''
	declare @const_columns_with_types table([column_name] nvarchar(128), [column_type] nvarchar(128))
	
	set @sql_to_describe = 'select * from ' + @db + '.' +  @schema + '.' + @table 
	
	set @columns_in_clause = '('
	select @columns_in_clause = @columns_in_clause + '''' + c.column_name + '''' + ',' 
									  from @columns c
	
	set @columns_in_clause = STUFF(@columns_in_clause,LEN(@columns_in_clause), 1,'')

	set @columns_in_clause = @columns_in_clause + ')'

	set @sql_to_get_type = 
	'select	name, system_type_name
		FROM ' + @db + '.sys.dm_exec_describe_first_result_set(''' + @sql_to_describe + ' '', NULL, 1)
		where name in ' + @columns_in_clause;

	select @sql_to_get_type


