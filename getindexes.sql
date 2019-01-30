declare @sql_to_get_clust_indexes varchar(MAX) = '',
		@sql_to_turn_off_clust_indexes varchar(MAX) = '',
		@db nvarchar(100) = 'CNFS_HUN', 
		@full_table_name nvarchar(300) = 'CNFS_HUN.dbo.Agreement_Table',
		@query_to_run nvarchar(max) = ''
		declare @non_clustered_indexes table ([index_name] nvarchar(128))

		
select @sql_to_get_clust_indexes = 
	'select i.name 
	from ' + @db + '.sys.indexes i
	join ' + 
		@db + '.sys.objects o
		ON i.object_id = o.object_id 
	where i.object_id = object_id(''' + @full_table_name + ''')
	    and i.type_desc = ''NONCLUSTERED''
		and i.is_disabled = 0 
		and o.type_desc = ''USER_TABLE''  '


insert into @non_clustered_indexes exec (@sql_to_get_clust_indexes)

select * from @non_clustered_indexes;


dbcc freeproccache
dbcc dropcleanbuffers
exec dbo.sp_SimpleAnonymizer @db='CNFS_HUN', @schema='dbo', @tablep='agreement_historic_data_table', @column_name = 'total_amount_payable'
