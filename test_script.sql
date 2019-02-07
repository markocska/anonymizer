
use Anonymizer;

declare @const_columns dbo.ConstantColumnList
declare @columns dbo.ColumnList
declare @const_columns_in_clause nvarchar(MAX) = ''

insert into @const_columns values ('customer_id','3'),('settlement_balance', '4')
insert into @columns values ('reporting_week_num'), ('agreement_db_id')


declare @sql_to_get_clust_indexes varchar(MAX) = '',
		@sql_to_turn_off_clust_indexes varchar(MAX) = '',
		@db nvarchar(100) = 'cnfs_hun', 
		@schema nvarchar(100) = 'dbo', 
		@table_name nvarchar(100) = 'agreement_table', 
		@full_table_name nvarchar(300) = 'cnfs_hun.dbo.agreement_table',
		@query_to_run nvarchar(max) = ''
		declare @non_clustered_indexes table ([index_name] nvarchar(128))


use Anonymizer;
dbcc freeproccache
dbcc dropcleanbuffers
exec dbo.sp_SimpleAnonymizer @db='cnfs_hun', @schema='dbo', @tablep='agreement_table', @scrambled_columns = @columns, @constant_columns = @const_columns