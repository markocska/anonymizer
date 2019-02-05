declare @sql_to_get_clust_indexes varchar(MAX) = '',
		@sql_to_turn_off_clust_indexes varchar(MAX) = '',
		@db nvarchar(100) = 'target_ifuat', 
		@schema nvarchar(100) = 'dbo', 
		@table_name nvarchar(100) = 't_actors', 
		@full_table_name nvarchar(300) = 'target_ifuat.dbo.t_actors',
		@query_to_run nvarchar(max) = ''
		declare @non_clustered_indexes table ([index_name] nvarchar(128))

--exec dbo.sp_EnableDisableClusteredIndexes @db=@db, @schema=@schema, @table_name=@table_name, @enable = 1
		
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

use Anonymizer;
dbcc freeproccache
dbcc dropcleanbuffers
exec dbo.sp_SimpleAnonymizer @db='cnfs_hun', @schema='dbo', @tablep='agreement_table', @column_name = 'creation_week'

exec dbo.sp_EnableDisableClusteredIndexes @db=@db, @schema='dbo', @table_name='agreement_table', @enable = 1

use cnfs_hun;

select rows, rowmodctr
from cnfs_hun.sys.sysindexes with (nolock)
where id = object_id('agreement_historic_data_table')

use Target_IFUAT;
begin transaction disable_indexes

alter index gi_t_actors100 on dbo.t_actors disable;

alter index gi_t_actors100 on dbo.t_actors rebuild;

commit

select cast(cucuka as nvarchar(128))