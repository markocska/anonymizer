USE [Target_IFUAT]
GO

/****** Object:  Table [dbo].[T_ACTORS]    Script Date: 2019. 01. 24. 14:45:44 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE anonymizer.[dbo].anonymizer_test(
	[user_ID] [bigint] IDENTITY(1,1) NOT NULL,
	[group_id] bigint not null,
	[country_id] bigint not null, 
	[AMNAME] [varchar](100) NULL,
	[DATEOFBIRTH] [datetime] NULL,
	[DEBTORID] [varchar](30) NULL,
	[DMNAME] [varchar](100) NULL,
	[EMPLOYERADDRESS] [varchar](100) NULL,
	[EMPLOYERNAME] [varchar](100) NULL,
	[EMPLOYERPHONE] [varchar](50) NULL,
	[FAMILYNAME] [varchar](50) NULL,
	[FIRSTNAME] [varchar](50) NULL,
	[GENDERID] [int] NULL,
	[ISDEAD] [bit] NULL,
	[ISFORGETEN] [bit] NULL,
	[MAIDENNAME] [varchar](50) NULL,
	[MOTHERSNAME] [varchar](50) NULL,
	[PERSONALIDENTITYNO] [varchar](13) NULL,
	[PLACEOFBIRTH] [varchar](50) NULL,
	[T_TRANSACTIONS_1] [bigint] NOT NULL,
 CONSTRAINT [PK_T_ACTORS104] PRIMARY KEY CLUSTERED 
(
	[user_id] ASC, [group_id] asc, country_id asc
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

insert into Anonymizer.dbo.anonymizer_test
select * from
(
	/****** Script for SelectTopNRows command from SSMS  ******/
SELECT
	   abs(checksum(newId())) as [group_id]	,
	   abs(checksum(newId())) as [country_id]
      ,[AMNAME]
      ,[DATEOFBIRTH]
      ,[DEBTORID]
      ,[DMNAME]
      ,[EMPLOYERADDRESS]
      ,[EMPLOYERNAME]
      ,[EMPLOYERPHONE]
      ,[FAMILYNAME]
      ,[FIRSTNAME]
      ,[GENDERID]
      ,[ISDEAD]
      ,[ISFORGETEN]
      ,[MAIDENNAME]
      ,[MOTHERSNAME]
      ,[PERSONALIDENTITYNO]
      ,[PLACEOFBIRTH]
      ,[T_TRANSACTIONS_1]
  FROM [Target_IFUAT].[dbo].[T_ACTORS]
) as a

