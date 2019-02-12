-- ================================================
-- Template generated from Template Explorer using:
-- Create Scalar Function (New Menu).SQL
--
-- Use the Specify Values for Template Parameters 
-- command (Ctrl-Shift-M) to fill in the parameter 
-- values below.
--
-- This block of comments will not be included in
-- the definition of the function.
-- ================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
alter FUNCTION dbo.func_get_minimal_unique_index_columns
(
	@db nvarchar(128),
	@schema nvarchar(128),
	@table nvarchar(128)
)
RETURNS nvarchar(MAX)
with execute as caller
AS
BEGIN
	-- Declare the return variable here
	declare @get_unique_columns_script nvarchar(MAX)

	set @get_unique_columns_script = 
			'use CNFS_HUN;

				 with index_column_quantities as (
				 select i.index_id, sum(1) as num_of_rows, i.object_id from 
				 sys.indexes i 
				 join sys.index_columns ic on i.index_id = ic.index_id and i.object_id = ic.object_id
				 where i.is_unique = 1 and i.is_disabled = 0
				 group by i.object_id, i.index_id
				 having i.object_id = object_id(''' + @db + '.' + @schema + '.' + @table + ''') 
				 ), 
 
				 min_column_quantity_idx as (
				 select top(1) index_id, object_id from index_column_quantities ic
				 where num_of_rows = (select min(num_of_rows) from index_column_quantities)
				 )

				 select c.name from
				 min_column_quantity_idx m 
				 join sys.index_columns ic on m.index_id = ic.index_id and m.object_id = ic.object_id
				 join sys.columns c on ic.column_id = c.column_id and ic.object_id = c.object_id '


	-- Return the result of the function
	RETURN @get_unique_columns_script

END
GO

