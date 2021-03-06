USE [mms]
GO
/****** Object:  StoredProcedure [dbo].[Proc_Save_XHRW_Change_Record]    Script Date: 2017-12-13 20:40:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[Proc_Save_XHRW_Change_Record]
(
@PackId int,@DraftIdStr nvarchar(max),@StateStr nvarchar(max),@UserId int,@NoAccpetIdStr nvarchar(max)
)
as
begin transaction T
declare @index int=0,@sum int=0,@index1 int=0,@sum1 int=0,@strId nvarchar(10)='',
@tmpMDML_Id nvarchar(10)='',@stateTmp nvarchar(10)='',@ChangeEvidenceId int=0,
@MDPId int=0,@MDP_Code nvarchar(30),@Quantity int=0,@QuantityTmp int=0,@QuantitySum int=0,@QuantityCha int=0,
@Unit_Price decimal(18,4)=0,@Sum_Price decimal(18,4)=0,@MDML_Id nvarchar(max)='',@Change_Code nvarchar(30),
@Special_Needs nvarchar(200),@Urgency_Degre nvarchar(100),@Secret_Level nvarchar(100),@Stage nvarchar(100),
@Use_Des nvarchar(100),@Shipping_Address nvarchar(100),@Certification nvarchar(100),@DemandDate date,@MCR_Id int=0

set @sum=(select [dbo].GetForNum_1(@DraftIdStr))
while @index<=@sum
begin
	set @strId=(select DBO.SplitStr_1(@DraftIdStr,','))
	set @DraftIdStr=(select DBO.ReplaceStr_1(@DraftIdStr,','))
	set @stateTmp=(select DBO.SplitStr_1(@StateStr,','))
	set @StateStr=(select DBO.ReplaceStr_1(@StateStr,','))
	--获得变更凭据ID,更改单－－－－－－－是不是只要更改就有M_Change_List_ID？
	set @ChangeEvidenceId=(select max(M_Change_List_ID) from M_Change_Flow where MDDLD_Id=@strId)
	exec Proc_CodeBuild 'BG',@Change_Code output--生成变更单号
	--1.已删除；2.不需要提交；3.属性变更；4.材料减少；5.材料增加。
	--情况1、2、3、4，变更后，都要在变更表中增加1－n条对冲记录；情况4，再根据变更后的物资数量增加一条提交记录，
	--情况5，单独处理，用变更后的新物资数量减去提交表中的未对冲过（Is_Change=0）的记录（1－n条）的提交的物资数量之和后，
	--增加一条新提交记录
	if(@stateTmp='5')--材料增加
	begin
		--根据Id获得该材料提交数量及M_Demand_Merge_List表ID，如：1085，1900，……型号表中的每条数据有可能提交n次，
		--故该字段提交一次累加一个M_Demand_Merge_List表ID
		select @MDML_Id=MDML_Id,@Quantity=Quantity from M_Demand_DetailedList_Draft where Id=@strId
		set @sum1=(select [dbo].GetForNum_1(@MDML_Id))
		set @index1=0
		while @index1<=@sum1
		begin
			set @tmpMDML_Id=(select DBO.SplitStr_1(@MDML_Id,','))
			set @MDML_Id=(select DBO.ReplaceStr_1(@MDML_Id,','))
			--根据M_Demand_DetailedList_Draft－MDML_Id、Id、未变更标识（有可能该记录在M_Change_Record中已存在记录）获得提交数量
			set @QuantityTmp=(select Quantity from M_Demand_Merge_List where id=@MDML_Id and Correspond_Draft_Code=@strId 
				and Is_Change=0)
			set @QuantitySum=@QuantitySum+@QuantityTmp--将之前提交的数量累加
			set @index1=@index1+1
		end
		--set @Unit_Price=0--未完，等待计算
		--set @Sum_Price=0--未完，等待计算
		set @QuantityCha=@Quantity-@QuantitySum--获得本次变更数量和历次提交数量的差，即本次变更后需要增加的数量
		select @DemandDate=DemandDate,@Special_Needs=Special_Needs,@Urgency_Degre=Urgency_Degre,@Secret_Level=Secret_Level,
			@Use_Des=Use_Des,@Shipping_Address=Shipping_Address,@Certification=Certification,@Unit_Price=Unit_Price,
			@Sum_Price=Sum_Price from M_Demand_Merge_List where id=@MDML_Id and Correspond_Draft_Code=@strId and Is_Change=0
		--将增加的数量添加到提交表中，
		insert M_Demand_Merge_List(Correspond_Draft_Code,Drawing_No,PackId,TaskId,DraftId,TechnicsLine,ItemCode1,NumCasesSum,
			Mat_Unit,Quantity,Rough_Size,Dinge_Size,DemandDate,MaterialsDes,Stage,Unit_Price,Sum_Price,Is_Submit,Special_Needs,Urgency_Degre,
			Secret_Level,Use_Des,Shipping_Address,Certification,[User_ID],Submit_Date,MDPId,DemandNumSum,Rough_Spec,Mat_Rough_Weight,
			TaskCode,MaterialDept)
			select @strId,Drawing_No,PackId,TaskId,DraftId,Technics_Line,ItemCode1,NumCasesSum,Mat_Unit,@QuantityCha,Rough_Size,Dinge_Size,
			@DemandDate,MaterialsDes,Stage,@Unit_Price,@Sum_Price,1,@Special_Needs,@Urgency_Degre,@Secret_Level,@Use_Des,
			@Shipping_Address,@Certification,@userId,getdate(),MDPId,DemandNumSum,Rough_Spec,Mat_Rough_Weight,TaskCode,
			MaterialDept from M_Demand_DetailedList_Draft where Id=@strId
		set @tmpMDML_Id=(select SCOPE_IDENTITY())
		--更新状态为已提交，并连接M_Demand_DetailedList_Draft－MDML_Id(M_Demand_Merge_List的Id),
		update M_Demand_DetailedList_Draft set Material_State=1,
			MDML_Id=(case when MDML_Id is null or MDML_Id='' then @tmpMDML_Id else MDML_Id+','+@tmpMDML_Id end) where id=@strId
		--增加变更记录(增加后的数量)
		insert M_Change_Record(Change_Code, Change_Evidence_Id, Correspond_Draft_Code, MDPId, MDMId, Change_Date,Change_State, MReduce_Num, [User_ID])
			select @Change_Code,@ChangeEvidenceId,@strId,MDPId,@tmpMDML_Id,getdate(),@stateTmp,@QuantityCha, @UserId
			from M_Demand_Merge_List where Correspond_Draft_Code=@strId and id=@tmpMDML_Id
		set @MCR_Id=(select @@IDENTITY)
		--更新物流中心未接受的记录的状态Accept_State
		exec Proc_Modify_ChangeRecord_Accept_State @MCR_Id,@NoAccpetIdStr,@strId
	end
	else--1,2,3,4情况
	begin
		--根据Id获得该材料提交数量及M_Demand_Merge_List表ID，如：1085，1900，……型号表中的每条数据有可能提交n次，
		--故该字段提交一次累加一个M_Demand_Merge_List表ID
		set @MDML_Id=(select MDML_Id from M_Demand_DetailedList_Draft where Id=@strId)
		set @sum1=(select [dbo].GetForNum_1(@MDML_Id))
		set @index1=0
		while @index1<=@sum1
		begin
			set @tmpMDML_Id=(select DBO.SplitStr_1(@MDML_Id,','))
			set @MDML_Id=(select DBO.ReplaceStr_1(@MDML_Id,','))
			--如果已提交且未变更，
			if exists(select id from M_Demand_Merge_List where id=@tmpMDML_Id and Is_Change=0)
			begin
				--增加变更记录，并将该记录的“是否变更”更新为已变更
				insert M_Change_Record(Change_Code,Change_Evidence_Id,Correspond_Draft_Code,MDPId,MDMId,Change_Date,Change_State,
					MReduce_Num,[User_ID])
					select @Change_Code,@ChangeEvidenceId,@strId,MDPId,@tmpMDML_Id,getdate(),@stateTmp,Quantity, @UserId
						from M_Demand_Merge_List where Correspond_Draft_Code=@strId and id=@tmpMDML_Id
				set @MCR_Id=(select @@IDENTITY)
				--更新物流中心未接受的记录的状态Accept_State
				exec Proc_Modify_ChangeRecord_Accept_State @MCR_Id,@NoAccpetIdStr,@strId
				update M_Demand_Merge_List set Is_Change=1 where Correspond_Draft_Code=@strId and id=@tmpMDML_Id
			end
			set @index1=@index1+1
		end
		if(@stateTmp='1' or @stateTmp='2')--1.已删除；2.不需要提交；
		begin
			update M_Demand_DetailedList_Draft set Material_State=3 where id=@strId
		end
		
		if(@stateTmp='4' or @stateTmp='3')--4-材料减少,3-属性变更
		begin
			select @MDML_Id=max(id) from M_Demand_Merge_List where Correspond_Draft_Code=@strId
			if(@MDML_Id is not null and @MDML_Id!='')
			begin
				select @DemandDate=DemandDate,@Special_Needs=Special_Needs,@Urgency_Degre=Urgency_Degre,@Secret_Level=Secret_Level,
					@Use_Des=Use_Des,@Shipping_Address=Shipping_Address,@Certification=Certification,@Unit_Price=Unit_Price,
					@Sum_Price=Sum_Price from M_Demand_Merge_List where id=@MDML_Id and Correspond_Draft_Code=@strId
				insert M_Demand_Merge_List(Correspond_Draft_Code,Drawing_No,PackId,TaskId,DraftId,TechnicsLine,ItemCode1,NumCasesSum,
					Mat_Unit,Quantity,Rough_Size,Dinge_Size,DemandDate,MaterialsDes,Stage,Unit_Price,Sum_Price,Is_Submit,Special_Needs,Urgency_Degre,
					Secret_Level,Use_Des,Shipping_Address,Certification,[User_ID],Submit_Date,MDPId,DemandNumSum,Rough_Spec,
					Mat_Rough_Weight,TaskCode,MaterialDept)
					select @strId,Drawing_No,PackId,TaskId,DraftId,Technics_Line,ItemCode1,NumCasesSum,Mat_Unit,Quantity,Rough_Size,Dinge_Size,
					@DemandDate,MaterialsDes,Stage,@Unit_Price,@Sum_Price,1,@Special_Needs,@Urgency_Degre,@Secret_Level,@Use_Des,
					@Shipping_Address,@Certification,@userId,getdate(),MDPId,DemandNumSum,Rough_Spec,Mat_Rough_Weight,TaskCode,
					MaterialDept from M_Demand_DetailedList_Draft where Id=@strId
				set @tmpMDML_Id=(select SCOPE_IDENTITY())
				update M_Demand_DetailedList_Draft set Material_State=1,
					MDML_Id=(case when MDML_Id is null or MDML_Id='' then @tmpMDML_Id else MDML_Id+','+@tmpMDML_Id end) where id=@strId
			end
		end
	end
	set @index=@index+1
end 

if @@error<>0
	rollback transaction T
else
	commit transaction T




