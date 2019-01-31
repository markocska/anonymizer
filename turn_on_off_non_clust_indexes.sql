USE [Anonymizer]
GO
/****** Object:  StoredProcedure [dbo].[sp_SimpleAnonymizer]    Script Date: 2019. 01. 17. 10:35:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

alter procedure dbo.sp_EnableDisableClusteredIndexes
(
	@db nvarchar(128),
	@schema nvarchar(128),
	@table_name nvarchar(128),
	@enable bit 
) 
as 
begin

declare @sql_to_get_non_clust_indexes nvarchar(MAX) = '', 
		@full_table_name nvarchar(500) = @db + '.' + @schema + '.' + @table_name,
		@sql_to_en_dis_non_clust_indexes nvarchar(MAX) = ''
declare @non_clustered_indexes table  ([index_name] nvarchar(128))

select @sql_to_get_non_clust_indexes = 
		'select i.name 
		from ' + @db + '.sys.indexes i
		join ' + 
			@db + '.sys.objects o
			ON i.object_id = o.object_id 
		where i.object_id = object_id(''' + @full_table_name + ''')
			and i.type_desc = ''NONCLUSTERED''
			and i.is_disabled = ' + CAST(@enable as nvarchar) + ' 
			and o.type_desc = ''USER_TABLE''  '

	insert into @non_clustered_indexes exec (@sql_to_get_non_clust_indexes)

	select @sql_to_en_dis_non_clust_indexes
			= @sql_to_en_dis_non_clust_indexes + 'alter index ' + i.index_name +  ' on ' + @full_table_name +
			case @enable when 1 then ' rebuild;' else ' disable;' end + char(13) + char(10)
										from @non_clustered_indexes i
	exec (@sql_to_en_dis_non_clust_indexes) 



end;