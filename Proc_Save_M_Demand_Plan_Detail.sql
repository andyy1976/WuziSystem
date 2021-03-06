USE [mms]
GO
/****** Object:  StoredProcedure [dbo].[Proc_Save_M_Demand_Plan_Detail]    Script Date: 2017-12-13 20:47:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[Proc_Save_M_Demand_Plan_Detail]
(
@ID int,@Correspond_Draft_Code nvarchar(300),@Drawing_No nvarchar(50),@PackId int,@TaskId int,@DraftId int,@TechnicsLine nvarchar(50)
,@ItemCode1 nvarchar(50),@NumCasesSum decimal(18,4),@Mat_Unit nvarchar(10),@Quantity int,@Rough_Size nvarchar(50),@Dinge_Size nvarchar(50),@DemandDate date,
@MaterialsDes nvarchar(500),@Stage int,@Unit_Price decimal(18,4),@Sum_Price decimal(18,4),@Special_Needs int,@Urgency_Degre int,
@Secret_Level int,@Use_Des int,@Shipping_Address nvarchar(10),@Certification int,@userId int,@MDPId int
,@DemandNumSum decimal(18, 4),@Rough_Spec nvarchar(50)
)
as
begin transaction T
declare @res int=0,@curdate datetime,@sql nvarchar(max)='',@tmptablename nvarchar(100)='##tmp_merge_list',@state int=0
set @tmptablename=@tmptablename+Convert(nvarchar(10),@userId)
set @curdate=(select getdate())
set @state=(select Material_State from M_Demand_DetailedList_Draft where Id=@ID)
insert M_Demand_Merge_List(Correspond_Draft_Code,Drawing_No,PackId,TaskId,DraftId,TechnicsLine,ItemCode1,NumCasesSum,
	Mat_Unit,Quantity,Rough_Size,Dinge_Size,DemandDate,MaterialsDes,Stage,Unit_Price,Sum_Price,Is_Submit,Special_Needs,Urgency_Degre,
	Secret_Level,Use_Des,Shipping_Address,Certification,[User_ID],Submit_Date,MDPId,DemandNumSum,Rough_Spec)
values(@Correspond_Draft_Code,@Drawing_No,@PackId,@TaskId,@DraftId,@TechnicsLine,@ItemCode1,@NumCasesSum,@Mat_Unit,@Quantity,
	@Rough_Size,@Dinge_Size,@DemandDate,@MaterialsDes,@Stage,@Unit_Price,@Sum_Price,1,@Special_Needs,@Urgency_Degre,
	@Secret_Level,@Use_Des,@Shipping_Address,@Certification,@userId,@curdate,@MDPId,@DemandNumSum,@Rough_Spec)

if @@error<>0
	rollback transaction T
else
	commit transaction T







