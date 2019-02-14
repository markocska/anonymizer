
/****** Object:  StoredProcedure [dbo].[sp_SimpleAnonymizer]    Script Date: 2019. 01. 17. 10:35:57 ******/
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON


declare @db nvarchar(128), @schema nvarchar(128),@table_name nvarchar(128),@enable bit

select @db = @db_value, @schema = @schema_value, @table_name = @table_value, @enable = @enable_value

declare @sql_to_get_indexes nvarchar(MAX) = '', 
		@full_table_name nvarchar(500) = @db + '.' + @schema + '.' + @table_name,
		@sql_to_en_dis_non_clust_indexes nvarchar(MAX) = ''
declare @non_clustered_indexes table  ([index_name] nvarchar(128))

select @sql_to_get_indexes = 
		'select i.name 
		from ' + @db + '.sys.indexes i
		join ' + 
			@db + '.sys.objects o
			ON i.object_id = o.object_id 
		where i.object_id = object_id(''' + @full_table_name + ''')
			and i.type_desc <> ''CLUSTERED''
			and i.is_unique_constraint = 0
			and i.is_primary_key = 0
			and i.is_disabled = ' + CAST(@enable as nvarchar) + ' 
			and o.type_desc = ''USER_TABLE''  '

	insert into @non_clustered_indexes exec (@sql_to_get_indexes)

	select @sql_to_en_dis_non_clust_indexes
			= @sql_to_en_dis_non_clust_indexes + 'alter index ' + i.index_name +  ' on ' + @full_table_name +
			case @enable when 1 then ' rebuild;' else ' disable;' end + char(13) + char(10)
										from @non_clustered_indexes i
	exec (@sql_to_en_dis_non_clust_indexes) 

