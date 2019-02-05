USE [Anonymizer]
GO
/****** Object:  StoredProcedure [dbo].[sp_SimpleAnonymizer]    Script Date: 2019. 01. 17. 10:35:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

if type_id('dbo.ConstantColumnList') is null
	begin
		create type dbo.ConstantColumnList
		as table
		(
			column_name nvarchar(128),
			column_value nvarchar(max)
		);
	end
go

if TYPE_ID('dbo.ColumnList') is null
	begin
		create type dbo.ColumnList
		as table
		(
			column_name nvarchar(128)
		);
	end
go
