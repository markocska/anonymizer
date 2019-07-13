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

ALTER TABLE [People].[dbo].[big table] DISABLE TRIGGER ALL 

create table #prim_keys_and_columns
		(
			id int, [first name] nvarchar(50),  [last name] nvarchar(50),  rownum int 
		)  
create table #column1
		 (
			random int, [first name] nvarchar(50) 
		 )  
create table #column2
		 (
			random int, [last name] nvarchar(50) 
		 )  

insert into #prim_keys_and_columns with (tablock)
		select id,  [first name],  [last name], row_number () over (order by id)
		from [People].[dbo].[big table]  

 insert into #column1 with (tablock)
				select row_number () over (order by x), [first name] from (select CHECKSUM(NewId()) x, [first name] from #prim_keys_and_columns) a ;  
 insert into #column2 with (tablock)
				select row_number () over (order by x), [last name] from (select CHECKSUM(NewId()) x, [last name] from #prim_keys_and_columns) a ;  
		
		CREATE INDEX IDX_PRIMARYKEYS ON #prim_keys_and_columns(id);  
CREATE INDEX IDX_COLUMN1_RANDOM ON #COLUMN1(random); 
CREATE INDEX IDX_COLUMN2_RANDOM ON #COLUMN2(random);

alter table [People].[dbo].[big table] nocheck constraint all; 

exec dbo.sp_EnableDisableNon_PrKeyUniqueClust_Indexes @db='[People]', @schema = '[dbo]', @table_name = '[big table]', @enable = 0; 


update [People].[dbo].[big table]with (tablock) set  
[first name] = x.[first name],  
[last name] = x.[last name],   [demail] = cast('23' as nvarchar(100))  from
		(
			select id, #column1.[first name] , #column2.[last name]  from #prim_keys_and_columns  
 
			join #column1 on #prim_keys_and_columns.rownum = #column1.random  
 
			join #column2 on #prim_keys_and_columns.rownum = #column2.random  
		) x
		where [People].[dbo].[big table].id= x.id;

		drop table #prim_keys_and_columns;  
drop table #column1 ; 
drop table #column2 ; ALTER TABLE [People].[dbo].[big table] ENABLE TRIGGER ALL;  alter table [People].[dbo].[big table] check constraint all; 


use People;
update dbo.cucuka
set [name] = b.[first name], baba = b.[last name]
from dbo.[cucuka] c
	join dbo.employees as e on c.id = e.id
	join dbo.Franchisees as l on e.id = l.ID
	join dbo.[big table] as b on b.id = l.ID;

	use People;
update NAV.dbo.Citizens
set first_name = source.[first name], last_name = source.[last name]
from Nav.dbo.Citizens dest
	join nav.dbo.EmployeesCitizens a2 on dest.citizenId = a2.citizenId
	join People.dbo.employees source on a2.employeeId = source.id;
	

