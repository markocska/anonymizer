USE [Anonymizer]
GO
/****** Object:  StoredProcedure [dbo].[sp_SimpleAnonymizer]    Script Date: 2019. 01. 16. 14:15:11 ******/
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
	
declare @proc nvarchar(MAX)
declare @idx nvarchar(100), @column_type nvarchar(100)
declare  @error_message nvarchar(500); 

set @idx = @index_name

-- Parameter checkings

 declare @check_script nvarchar(MAX), @check_count int, @param_definition nvarchar(MAX)

 if OBJECT_ID(@db + '.' + @schema + '.' + @tablep) IS NULL
	begin		
		set @error_message = 'The table ' + @db + '.' + @schema + '.' + @tablep + ' does not exist.';
		raiserror(@error_message,16,1);
	end


set @check_script =
					'SELECT @check_count_out = count(1) '
					+ 'FROM '   
						+ @db + '.' + 'sys.columns c
					INNER JOIN ' 
						+ @db + '.' + 'sys.types t ON c.user_type_id = t.user_type_id
					WHERE
						c.object_id = OBJECT_ID(''' + @db + '.' + @schema + '.' + @tablep + ''') and ' + 'c.[name] = ' + '''' + @column_name + '''';



set @param_definition = '@check_count_out int OUTPUT';
exec sp_executesql @check_script, @param_definition, @check_count_out=@check_count OUTPUT;
	if @check_count = 0
     begin
		 Set @error_message = 'The given column with name ' + @column_name + ' does not exist in the table ' 
				+ @db + '.' + @schema + '.' + @tablep;  
		 raiserror(@error_message,16,1)
	 end

 --if NOT EXISTS( SELECT 
	--				*
	--			FROM    
	--				sys.columns c
	--			INNER JOIN 
	--				sys.types t ON c.user_type_id = t.user_type_id
	--			INNER JOIN 
	--				sys.index_columns ic ON ic.object_id = c.object_id AND ic.column_id = c.column_id
	--			INNER JOIN 
	--				sys.indexes i ON ic.object_id = i.object_id AND ic.index_id = i.index_id
	--			WHERE
	--				c.object_id = OBJECT_ID(@db + '.' + @schema + '.' + @tablep) and i.is_unique = 1 and c.[name] = @index_name )
	--  begin
	--	 Set @error_message = 'The unique index with name ' + @index_name + ' does not exist in the table ' 
	--			+ @db + '.' + @schema + '.' + @tablep;  
	--	 raiserror(@ErrorMessage,16,1)
	--  end

--------

-- Surrounding the parameter names with [ ] so we won't run into errors regarding the naming.
	Select @db = '[' + @db + ']',
		   @schema = '[' + @schema + ']',
		   @tablep = '[' + @tablep + ']',
		   @column_name = '[' + @column_name + ']',
		   @index_name = '[' + @index_name + ']';
--------
	

				set @proc = 
				'ALTER TABLE ' + @db + '.' + @schema + '.' + @tablep + ' DISABLE TRIGGER ALL '
			+	'create table #t1
				(
					' +	@index_name + ' nvarchar(500), ' 
					  + @column_name + ' nvarchar(500), '
					+ ' rownum int) '
			+	'create table #t2
				(
					random int, ' 
					+ @column_name + ' nvarchar(2000)
				)'
				
					set @proc = @proc + 
						' insert into #t1
						select convert(nvarchar(500),' + @idx + '), '
						+	   @column_name + ', '
						+	  'row_number () over (order by ' + @idx +')
						from ' + @db + '.' + @schema + '.' + @tablep + ' ' + @where +

						' insert into #t2
						select row_number () over (order by x), ' + @column_name 
							+ ' from (select CHECKSUM(NewId()) x, ' + @column_name + 'from #t1) a

						update #t1
						set ' + @column_name + ' = x.new_value
						from
						(
							select random r, ' + @column_name + ' new_value from #t2
						) x where x.r = rownum

					 truncate table #t2	'					 


					set @proc = @proc + ' update ' + @db + '.' + @schema + '.' + @tablep +
					' set ' + @column_name + ' = x.' + @column_name + ' '
					+
					' from
					(
						select ' + @idx + ' i, '
							     + @column_name
						+ ' from #t1
					) x
					where x.i = convert(nvarchar(500),' + @idx + ') collate SQL_Hungarian_CP1250_CI_AS

					drop table #t1
					drop table #t2 '
					+
						'ALTER TABLE ' + @db + '.dbo.' + @tablep + ' ENABLE TRIGGER ALL'
				

				exec (@proc)	
				end

		--if @cv = 0 or @cv is null
		---- debug, ha a paraméterek el lennének írva
		--begin
		--	print 'Nincs anonimizálás beállítva:' 
		--	print 'Adatbázis:'+ @db 
		--	print 'Tábla:' + @tablep
		--end



