/****** Script for SelectTopNRows command from SSMS  ******/
SELECT TOP (1000) refinancing_agreement_id, settlement_balance,
			reporting_week_num, customer_id, actual_paid_up_week
  FROM [CNFS_HUN].[dbo].[Agreement_Table]