USE [mms]
GO
/****** Object:  StoredProcedure [dbo].[Proc_Save_M_Demand_Plan_Detail_1]    Script Date: 2017-12-13 20:44:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[Proc_Save_M_Demand_Plan_Detail_1]
(
@ID int,@userId int,@MDPId int,@Special_Needs int,@Urgency_Degre int,@Secret_Level int,
@Use_Des int,@Shipping_Address nvarchar(10),@Certification int
)
as
begin transaction T
declare @res int=0,@curdate datetime,@sql nvarchar(max)='',@state int=0,@Correspond_Draft_Code nvarchar(300),
@Drawing_No nvarchar(50),@PackId int,@TaskId int,@DraftId int,@TechnicsLine nvarchar(50),@ItemCode1 nvarchar(50),
@NumCasesSum decimal(18,4),@Mat_Unit nvarchar(10),@Quantity int,@Rough_Size nvarchar(50),@Dinge_Size nvarchar(50),@DemandDate date,@MaterialsDes nvarchar(500),
@Stage int,@Unit_Price decimal(18,4),@Sum_Price decimal(18,4),@DemandNumSum decimal(18, 4),@Rough_Spec nvarchar(50)

set @curdate=(select getdate())
set @state=(select Material_State from M_Demand_DetailedList_Draft where Id=@ID)
set @Unit_Price=0--未完，等待计算
set @Sum_Price=0--未完，等待计算
insert M_Demand_Merge_List(Correspond_Draft_Code,Drawing_No,PackId,TaskId,DraftId,TechnicsLine,ItemCode1,NumCasesSum,
	Mat_Unit,Quantity,Rough_Size,Dinge_Size,DemandDate,MaterialsDes,Stage,Unit_Price,Sum_Price,Is_Submit,Special_Needs,Urgency_Degre,
	Secret_Level,Use_Des,Shipping_Address,Certification,[User_ID],Submit_Date,MDPId,DemandNumSum,Rough_Spec,TaskCode,MaterialDept)
select @ID,Drawing_No,PackId,TaskId,DraftId,Technics_Line,ItemCode1,NumCasesSum,Mat_Unit,Quantity,Rough_Size,Dinge_Size,
	DemandDate,MaterialsDes,Stage,@Unit_Price,@Sum_Price,1,@Special_Needs,@Urgency_Degre,@Secret_Level,@Use_Des,@Shipping_Address,
	@Certification,@userId,@curdate,@MDPId,DemandNumSum,Rough_Spec,TaskCode,MaterialDept 
	from M_Demand_DetailedList_Draft where Id=@ID
update M_Demand_DetailedList_Draft set MDPId=@MDPId where Id=@ID
if @@error<>0
	rollback transaction T
else
	commit transaction T






