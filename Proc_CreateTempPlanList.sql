USE [mms]
GO
/****** Object:  StoredProcedure [dbo].[Proc_CreateTempPlanList]    Script Date: 2017-12-13 22:20:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[Proc_CreateTempPlanList]
(@userid nvarchar(30),@res_table_name nvarchar(100) output)
as
declare @sql nvarchar(2000)
set @res_table_name='##tmp_plan_list'+@userid
set @sql='if exists(select * from tempdb..sysobjects where id=object_id(''tempdb..'+@res_table_name+'''))
	drop table '+@res_table_name+'
create table '+@res_table_name+'(ID int identity(1,1),
Correspond_Draft_Code nvarchar(300),Drawing_No nvarchar(50),PackId int,TaskId int,DraftId int,MDPId int,
TechnicsLine nvarchar(50),ItemCode1 nvarchar(50),NumCasesSum decimal(18,4),DemandNumSum decimal(18,4),Mat_Unit nvarchar(10), 
Quantity int,Rough_Size nvarchar(50),Dinge_Size nvarchar(50),Rough_Spec nvarchar(50),DemandDate  nvarchar(50),MaterialsDes nvarchar(500),Stage nvarchar(100), 
Unit_Price decimal(18, 4),Sum_Price decimal(18, 4),Special_Needs nvarchar(200),Urgency_Degre nvarchar(100),
Secret_Level nvarchar(100),Use_Des nvarchar(100),Shipping_Address nvarchar(100),Certification nvarchar(100),TaskCode nvarchar(50))'
exec(@sql)
--print @sql





