
execute Anonymizer.dbo.sp_SimpleAnonymizer @db='Target_IFUAT', @schema='dbo', @tablep='t_actors', @index_name='t_actors_id', @column_name='maidenname', @where = ''



DECLARE @sql_to_describe NVARCHAR(MAX),
		@sql_to_get_type nvarchar(MAX), @type nvarchar(100), @param_definition nvarchar(500), @column_name nvarchar(100)

set @sql_to_describe = N'select * from target_ifuat.dbo.t_actors';
set @param_definition = N'@sql nvarchar(MAX), @column nvarchar(100), @type_out nvarchar(100) OUTPUT'

set  @column_name = N'isdead'

set @sql_to_get_type = 
N'	SELECT @type_out = system_type_name
    FROM target_ifuat.sys.dm_exec_describe_first_result_set(@sql, NULL, 1)
	where name = @column';

exec sp_executesql @sql_to_get_type, @param_definition, @sql=@sql_to_describe, @column=@column_name, @type_out=@type OUTPUT;

select @type

ALTER TABLE [Target_IFUAT].[dbo].[t_actors] DISABLE TRIGGER ALL create table #t1
	(
		[t_actors_id] bigint, [firstname] varchar(50),  rownum int 
	) PRINT 2;;create table #t2
	(
		random int, [firstname] varchar(50) )  insert into #t1
		select [t_actors_id], [firstname], row_number () over (order by [t_actors_id])
		from [Target_IFUAT].[dbo].[t_actors]  insert into #t2
		select row_number () over (order by x), [firstname] from (select CHECKSUM(NewId()) x, [firstname]from #t1) a ; 

		update #t1
		set [firstname] = x.new_value
		from
		(
			select random r, [firstname] new_value from #t2
		) x where x.r = rownum ;
		PRINT 3
		truncate table #t2	 update [Target_IFUAT].[dbo].[t_actors] set [firstname] = x.[firstname]  from
	(
		select [t_actors_id] i, [firstname] from #t1
	) x
	where x.i = [t_actors_id]);
	PRINT 5
	drop table #t1
	drop table #t2 ALTER TABLE [Target_IFUAT].dbo.[t_actors] ENABLE TRIGGER ALL