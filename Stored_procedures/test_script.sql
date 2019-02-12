
use Anonymizer;

declare @const_columns dbo.ConstantColumnList
declare @columns dbo.ColumnList
declare @const_columns_in_clause nvarchar(MAX) = ''

insert into @const_columns values 
insert into @columns values ('nameced')


declare @sql_to_get_clust_indexes varchar(MAX) = '',
		@sql_to_turn_off_clust_indexes varchar(MAX) = '',
		@db nvarchar(100) = 'cnfs_hun', 
		@schema nvarchar(100) = 'dbo', 
		@table_name nvarchar(100) = 'agreement_table', 
		@full_table_name nvarchar(300) = 'cnfs_hun.dbo.agreement_table',
		@query_to_run nvarchar(max) = ''
		declare @non_clustered_indexes table ([index_name] nvarchar(128))


use Anonymizer;
exec dbo.sp_SimpleAnonymizer @db='cnfs_hun', @schema='dbo', @tablep='agreement_table', @scrambled_columns = @columns, @constant_columns = @const_columns


select i.name 
		from CNFS_HUN.sys.indexes i
		join CNFS_HUN.sys.objects o
			ON i.object_id = o.object_id 
		where i.object_id = object_id('target_ifuat.dbo.T_ROLES')
			and i.is_primary_key = 0
			and i.is_disabled = 0
			and o.type_desc = 'USER_TABLE';

select f.name, f.object_id
	from target_ifuat.sys.indexes f
	where  f.is_unique_constraint = 1

	select o.name 
	from target_ifuat.sys.objects o
	where o.object_id = 1106102981


alter table cnfs_hun.dbo.agreement_table
add constraint 


--alter index all on cnfs_hun.dbo.agreement_table rebuild;



