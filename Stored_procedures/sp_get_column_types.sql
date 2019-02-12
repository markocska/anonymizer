USE [Anonymizer]
GO
/****** Object:  StoredProcedure [dbo].[sp_SimpleAnonymizer]    Script Date: 2019. 01. 17. 10:35:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


alter function [dbo].[func_get_column_types]
(
@columns as dbo.ColumnList readonly,
@db as nvarchar(128),
@schema as nvarchar(128),
@table as nvarchar(128)
)
returns  nvarchar(MAX)
with execute as caller
as
begin
	
	declare @column_count int
	select @column_count = count(1) from @columns

	if @column_count = 0
		return ''

	declare @sql_to_describe nvarchar(MAX), @columns_in_clause nvarchar(MAX),
			@sql_to_get_type nvarchar(MAX)
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

	return @sql_to_get_type
end


