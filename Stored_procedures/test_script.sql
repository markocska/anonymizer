
use People;

declare @columns dbo.AnonymizerColumnList;
declare @const_columns dbo.AnonymizerColumnAndValueList;
declare @const_columns_in_clause nvarchar(MAX) = '';

insert into @const_columns values ('demail','23');
insert into @columns values ('first name'), ('last name');




use People;
exec dbo.sp_SimpleAnonymizer @db='People', @schema='dbo', @tablep='big table', @scrambled_columns = @columns, @constant_columns = @const_columns


select i.name 
		from CNFS_HUN.sys.indexes i
		join CNFS_HUN.sys.objects o
			ON i.object_id = o.object_id 
		where i.object_id = object_id('cnfs_hun.dbo.agreement_table')
			and i.is_primary_key = [
			and i.is_disabled = 1
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



