USE [mms]
GO
/****** Object:  StoredProcedure [dbo].[Proc_CreateTempMergeList]    Script Date: 2017-12-13 22:21:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[Proc_CreateTempMergeList]
(@userid nvarchar(30),@res_table_name nvarchar(100) output)
as
declare @sql nvarchar(2000)
set @res_table_name='##tmp_merge_list'+@userid
set @sql='if exists(select * from tempdb..sysobjects where id=object_id(''tempdb..'+@res_table_name+'''))
	drop table '+@res_table_name+'
create table '+@res_table_name+'(ID int identity(1,1),
Correspond_Draft_Code nvarchar(300),Drawing_No nvarchar(50),PackId int,TaskId int,DraftId int,
TechnicsLine nvarchar(50),ItemCode1 nvarchar(50),NumCasesSum decimal(18,4),DemandNumSum decimal(18,4),Mat_Unit nvarchar(10), 
Quantity int,Rough_Size nvarchar(50),Dinge_Size nvarchar(50),Rough_Spec nvarchar(50),DemandDate date,MaterialsDes nvarchar(500),Stage int, 
Unit_Price decimal(18, 4),Sum_Price decimal(18, 4),TaskCode nvarchar(50))'
exec(@sql)
--print @sql






