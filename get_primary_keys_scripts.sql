
declare @get_primary_keys_cmd nvarchar(MAX)
declare @primary_keys_table table ([name] nvarchar(128), column_id int)
declare @i int, @maxrows int
declare @name nvarchar(200)


set @get_primary_keys_cmd =	'select c.name, c.column_id
	from target_ifuat.sys.columns c
		inner join sys.index_columns ic on c.object_id = ic.object_id and c.column_id = ic.column_id
		inner join sys.indexes i on ic.object_id = i.object_id and ic.index_id = i.index_id
		where 
			i.is_primary_key = 1 and 
			c.object_id = OBJECT_ID(''target_ifuat.dbo.t_actors'')'

insert into @primary_keys_table exec (@get_primary_keys_cmd)

select @maxrows = count(1), @i = 1 from @primary_keys_Table

while @maxrows >= @i
begin
	select @name = [name] from @primary_keys_table order by column_id offset @i-1 row fetch next 1 row only
	print @name
	set @i = @i + 1
end

--select * from @primary_keys_Table



--select *
--	from target_ifuat.sys.columns c
--		inner join sys.index_columns ic on c.object_id = ic.object_id and c.column_id = ic.column_id
--		inner join sys.indexes i on ic.object_id = i.object_id and ic.index_id = i.index_id
--		where 
--		--i.is_primary_key = 1 and 
--			c.object_id = OBJECT_ID('target_ifuat.dbo.t_actors')