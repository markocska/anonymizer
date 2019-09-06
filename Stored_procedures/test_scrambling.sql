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
set [name] = source.[first name], baba = source.[last name]
from dbo.[cucuka] dest
	join dbo.employees as a1 on dest.id = a1.id
	join dbo.Franchisees as a0 on a1.id = a0.ID
	join dbo.[big table] as source on source.id = a0.ID;

	use People;
update NAV.dbo.Citizens
set first_name = source.[first name], last_name = source.[last name]
from Nav.dbo.Citizens dest
	join nav.dbo.EmployeesCitizens a2 on dest.citizenId = a2.citizenId
	join People.dbo.employees source on a2.employeeId = source.id;
	

update  [NAV].[dbo].[citizens]
set [first_name] = source.[first name],
 [last_name] = source.[last name]
from [NAV].[dbo].[citizens] dest
     join [NAV].[dbo].[EmployeesCitizens] a0 on a0.[citizenId] = dest.[citizenId]
join [People].[dbo].[employees] source on a0.[employeeId] = source.[id]


update  [NAV].[dbo].[citizens]
set [first_name] = source.[first name],
 [last_name] = source.[last name]
from [NAV].[dbo].[citizens] dest
     join [NAV].[dbo].[EmployeesCitizens] a0 on a0.[citizenId] = dest.[citizenId]
join [People].[dbo].[employees] source on a0.[employeeId] = source.[id]


update  [NAV].[dbo].[citizens]
set [first_name] = dest.[first name],
 [last_name] = dest.[last name]
from [NAV].[dbo].[citizens] dest

create table CitizensEUCitizens(
	Id int primary key,
	CitizenId int,
	euCitizenId int
);

alter table EUCitizens

add first_name nvarchar(100), last_name nvarchar(100);


update  [NAV].[dbo].[EUCitizens]
set [first_name] = source.[first name],
 [last_name] = source.[last name]
from [NAV].[dbo].[EUCitizens] dest
     join [NAV].[dbo].[CitizensEUCitizens] a1 on a1.[euCitizenId] = dest.[Id]
	 join nav.dbo.EmployeesCitizens a0 on a1.citizenId = a0.citizenId
join [People].[dbo].[employees] source on a0.[employeeId] = source.[id]



update  [NAV].[dbo].[citizens]
set [first_name] = source.[first name],
 [last_name] = source.[last name]
from [NAV].[dbo].[citizens] dest
     join [NAV].[dbo].[EmployeesCitizens] a0 on a0.[citizenId] = dest.[citizenId]
join [People].[dbo].[employees] source on a0.[employeeId] = source.[id];

update  [NAV].[dbo].[EUCitizens]
set [first_name] = source.[first name],
 [last_name] = source.[last name]
from [NAV].[dbo].[EUCitizens] dest
     join [NAV].[dbo].[CitizensEUCitizens] a1 on a1.[euCitizenId] = dest.[Id]
join [NAV].[dbo].[EmployeesCitizens] a0 on a1.[id] = a0.[employeeId]
join [People].[dbo].[employees] source on a0.[employeeId] = source.[id];

update  [NAV].[dbo].[EmployeesCitizens]
set [full name] = source.[first name],
 [citizen name] = source.[last name]
from [People].[dbo].[employees] source