
/****** Object:  StoredProcedure [dbo].[sp_SimpleAnonymizer]    Script Date: 2019. 01. 17. 10:35:57 ******/
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON

if type_id('dbo.AnonymizerColumnAndValueList') is null
	begin
		create type dbo.AnonymizerColumnAndValueList
		as table
		(
			column_name nvarchar(128),
			column_value nvarchar(max)
		);
	end

if type_id('dbo.AnonymizerColumnList') is null
	begin
		create type dbo.AnonymizerColumnList
		as table
		(
			column_name nvarchar(128)
		);
	end

