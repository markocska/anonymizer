USE [Anonymizer]
GO
/****** Object:  StoredProcedure [dbo].[sp_SimpleAnonymizer]    Script Date: 2019. 01. 17. 10:35:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		Mark Rusz
-- Create date: 2019-01-15
-- Description:	Scrambles a column of a given table
-- =============================================
ALTER PROCEDURE [dbo].[sp_SimpleAnonymizer] (
	-- Add the parameters for the stored procedure here
	@db nvarchar(100),
	@schema nvarchar(100),
	@tablep nvarchar(100),
	@index_name nvarchar(100),
	@column_name nvarchar(100),
	@where nvarchar(MAX)
	)
	as 
	begin

SET XACT_ABORT, NOCOUNT ON

	

declare  @error_message nvarchar(500), @operation nvarchar(200) 
declare @param_definition nvarchar(MAX), @proc nvarchar(MAX)


set @index_name = @index_name

-- Surrounding the parameter names with [ ] so we won't run into errors regarding the naming.
	Select @db = '[' + @db + ']',
		   @schema = '[' + @schema + ']',
		   @tablep = '[' + @tablep + ']';

BEGIN TRY
-- Parameter checkings
	
	set @operation = 'checking the input parameters.'

	declare @check_script nvarchar(MAX), @check_count int

	if OBJECT_ID(@db + '.' + @schema + '.' + @tablep) IS NULL
		begin		
			set @error_message = 'The table ' + @db + '.' + @schema + '.' + @tablep + ' does not exist.';
			raiserror(@error_message,16,1);
		end


	set @check_script =
						'SELECT @check_count_out = count(1) '
						+ 'FROM '   
							+ @db + '.sys.columns c '
						+ 'WHERE
							c.object_id = OBJECT_ID(''' + @db + '.' + @schema + '.' + @tablep + ''') and ' + 'c.[name] = ' + '''' + @column_name + '''';



	set @param_definition = '@check_count_out int OUTPUT';
	set @check_count = 0;
	exec sp_executesql @check_script, @param_definition, @check_count_out=@check_count OUTPUT;

	if @check_count = 0
		 begin
			 Set @error_message = 'The given column with name ' + upper(@column_name) + ' does not exist in the table ' 
					+ @db + '.' + @schema + '.' + @tablep;  
			 raiserror(@error_message,16,1)
		 end


	set @check_script =			
					'SELECT 
						@check_count_out = count(1)
					FROM '   
						+ @db + '.sys.columns c
					WHERE
						c.object_id = OBJECT_ID(''' + @db + '.' + @schema + '.' + @tablep + ''') '
						+ 'and c.[name] = ' + '''' + @index_name + '''';

	set @param_definition = '@check_count_out int OUTPUT';
	set @check_count = 0;
	exec sp_executesql @check_script, @param_definition, @check_count_out=@check_count OUTPUT;
		if @check_count = 0
		  begin
			 Set @error_message = 'The index column with name ' + upper(@index_name) + ' does not exist in the table ' 
					+ @db + '.' + @schema + '.' + @tablep;  
			 raiserror(@error_message,16,1)
		  end

--------



-- Get the scrambled columns's type and the index's type

	DECLARE @sql_to_describe NVARCHAR(MAX),
			@sql_to_get_type nvarchar(MAX), @column_type nvarchar(100), @index_type nvarchar(100)

	set @sql_to_describe = N'select * from target_ifuat.dbo.t_actors';
	set @param_definition = N'@sql nvarchar(MAX), @column nvarchar(100), @type_out nvarchar(100) OUTPUT'

	set @sql_to_get_type = 
	N'	SELECT @type_out = system_type_name
		FROM target_ifuat.sys.dm_exec_describe_first_result_set(@sql, NULL, 1)
		where name = @column';

	set @operation = 'getting the scrambled column''s type.'		
	exec sp_executesql @sql_to_get_type, @param_definition, @sql=@sql_to_describe, @column=@column_name, @type_out=@column_type OUTPUT;

	set @operation = 'getting the index column''s type.'
	exec sp_executesql @sql_to_get_type, @param_definition, @sql=@sql_to_describe, @column=@index_name, @type_out=@index_type OUTPUT;

--------

    set @column_name = '[' + @column_name + ']';
    set @index_name = '[' + @index_name + ']';
	
	set @proc = 
	N'ALTER TABLE ' + @db + '.' + @schema + '.' + @tablep + ' DISABLE TRIGGER ALL '
+	'create table #t1
	(
		' +	@index_name + ' ' + @index_type + ', ' 
			+ @column_name + ' ' + @column_type + ', '
		+ ' rownum int 
	) '
+	'create table #t2
	(
		random int, ' 
		+ @column_name + ' ' + @column_type + ' '
+	') '
				
	set @proc = @proc + 
		' insert into #t1
		select ' + @index_name + ', '
		+	   @column_name + ', '
		+	  'row_number () over (order by ' + @index_name +')
		from ' + @db + '.' + @schema + '.' + @tablep + ' ' + @where +

		' insert into #t2
		select row_number () over (order by x), ' + @column_name 
			+ ' from (select CHECKSUM(NewId()) x, ' + @column_name + 'from #t1) a ; 

		update #t1
		set ' + @column_name + ' = x.new_value
		from
		(
			select random r, ' + @column_name + ' new_value from #t2
		) x where x.r = rownum ;
		
		truncate table #t2	'			 

	set @proc = @proc + ' update ' + @db + '.' + @schema + '.' + @tablep +
	' set ' + @column_name + ' = x.' + @column_name + ' '
	+
	' from
	(
		select ' + @index_name + ' i, '
					+ @column_name
		+ ' from #t1
	) x
	where x.i = ' + @index_name + ';

	drop table #t1
	drop table #t2 '
	+
		'ALTER TABLE ' + @db + '.dbo.' + @tablep + ' ENABLE TRIGGER ALL'
	
	set @operation = 'doing the scrambling.'
	exec (@proc)	
END TRY
BEGIN CATCH

	if @@TRANCOUNT > 0 rollback transaction
	declare @msg nvarchar(MAX) ='Error while ' +  @operation + ' - ' + error_message() + '. Line: '
			+ convert(nvarchar,error_line())
	raiserror(@msg,16,1)

END CATCH

end
