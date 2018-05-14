USE [MMSDBBeta]
GO
/****** Object:  StoredProcedure [dbo].[Proc_Save_M_Demand_Plan_And_Change1]    Script Date: 2018-05-11 19:02:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[Proc_Save_M_Demand_Plan_And_Change1]
(
	@MDDLDIDStr nvarchar(max),  --M_Demand_DetailedList_Draftb表ID以','连接的串
	@NeedsStr nvarchar(max),    --特殊需求
	@DegreStr nvarchar(max),	--紧急程度
	@LevelStr nvarchar(max),	--密级
	@DesStr nvarchar(max),		--用途
	@AddrStr nvarchar(max),		--配送地址
	@CertStr nvarchar(max),		--合格证
	@ManufacturerStr nvarchar(max), --生产厂家
	@userId int,
	@PackId int,
	@DraftId int,
	@DateStr nvarchar(max)
)
AS
begin transaction T
	declare @MDDLDID nvarchar(10)='',
			@sum int=0,
			@tmpNeedsStr nvarchar(200)='',
			@tmpDegreStr nvarchar(100)='',
			@tmpLevelStr nvarchar(100)='',
			@tmpDesStr nvarchar(100)='',
			@tmpAddrStr nvarchar(100)='',
			@tmpCertStr nvarchar(100)='',
			@tmpManufacturerStr nvarchar(100) = '',
			@tmpDateStr nvarchar(30)='',
			@curdate datetime,
			@MDPLId int,
			@MDPL_Code nvarchar(30) = '',

			@Unit_Price decimal(18,4),
			@Sum_Price decimal(18,4),
			@MDML_Id nvarchar(max)='',
			@tmp nvarchar(max)='',
			@user_rq_number_start nvarchar(50),
			@project nvarchar(50)

	select @curdate = GetDate()

	--增加M_Demand_Plan_List记录
	exec Proc_CodeBuildByCodeDes '需求申请','TRQ', @MDPL_Code output

	select @user_rq_number_start = isnull(Code_Name,'TRQ') from Sys_CodeBuild where Code_Des = '需求申请'
	
	select @project = isnull(	(select top 1 DiCT_Code from GetBasicdata_T_Item 
	where DICT_CLASS = 'CUX_DM_PROJECT' and DICT_NAME = (select Model from Sys_Model where Convert(nvarchar(50),ID) = (select Model from P_Pack where PackId = @PackId))),'620')

	Insert into M_Demand_Plan_List (MDP_Code, PackId, DraftId, [User_ID], Submit_Date,Submit_Type, Submit_State)
	values (@MDPL_Code, @PackId, @DraftId, @userId, @curdate, '0','4')
	select @MDPLId = @@IDENTITY
	
	--获得ID字符串循环次数
	set @sum=(select [dbo].GetForNum_1(@MDDLDIDStr))

	while 0<=@sum
	begin
		select @sum = @sum - 1

		set @MDDLDID=(select DBO.SplitStr_1(@MDDLDIDStr,','))
		set @MDDLDIDStr=(select DBO.ReplaceStr_1(@MDDLDIDStr,','))
		set @tmpNeedsStr=(select DBO.SplitStr_1(@NeedsStr,','))--特殊需求
		set @NeedsStr=(select DBO.ReplaceStr_1(@NeedsStr,','))
		set @tmpDegreStr=(select DBO.SplitStr_1(@DegreStr,','))--紧急程度
		set @DegreStr=(select DBO.ReplaceStr_1(@DegreStr,','))
		set @tmpLevelStr=(select DBO.SplitStr_1(@LevelStr,','))--密级
		set @LevelStr=(select DBO.ReplaceStr_1(@LevelStr,','))
		set @tmpDesStr=(select DBO.SplitStr_1(@DesStr,','))--用途
		set @DesStr=(select DBO.ReplaceStr_1(@DesStr,','))
		set @tmpAddrStr=(select DBO.SplitStr_1(@AddrStr,','))--配送地址
		set @AddrStr=(select DBO.ReplaceStr_1(@AddrStr,','))
		set @tmpCertStr=(select DBO.SplitStr_1(@CertStr,','))--合格证
		set @CertStr=(select DBO.ReplaceStr_1(@CertStr,','))
		set @tmpManufacturerStr=(select DBO.SplitStr_1(@ManufacturerStr,','))--生产厂家
		set @ManufacturerStr=(select DBO.ReplaceStr_1(@ManufacturerStr,','))
		set @tmpDateStr=(select DBO.SplitStr_1(@DateStr,','))--需求时间
		set @DateStr=(select DBO.ReplaceStr_1(@DateStr,','))
		set @Unit_Price=0--未完，等待计算
		set @Sum_Price=0--未完，等待计算	
		declare @can nvarchar(200)=''
		set @can=(select ACCOUNT_NUM from GetCustInfo_T_CUST_ACCT where CUST_ACCOUNT_ID=(
			select Cust_Account_ID from Sys_DeptEnum where DeptCode=(select MaterialDept 
			from M_Demand_DetailedList_Draft where Id=@MDDLDID)))--需求部门

		--变更类型，1：删除，2：不需要提交，3：属性变更，4：材料减少，5：材料增加，空：第一次提交
		declare @Change_State nvarchar(2)
		if (select count(*) from M_Demand_Merge_List where Correspond_Draft_Code = @MDDLDID and Is_Submit = 'true') = 0 begin
			select @Change_State = ''
		end
		else begin
			declare @lastMDMLItemCode nvarchar(50),
					@lastMDMLRough_Spec nvarchar(50),
					@lastMDMLRough_Size nvarchar(50),
					@lastMDMLDinge_Size nvarchar(50),
					@lastMDMLMat_Rough_Weigh nvarchar(50),
					@ItemCode nvarchar(50),
					@Rough_Spec nvarchar(50),
					@Rough_Size nvarchar(50),
					@Dinge_Size nvarchar(50),
					@Mat_Rough_Weigh nvarchar(50),
					@Is_Del bit,
					@NumCasesSum decimal(18,4),
					@DemandNumSum decimal(18,4),
					@Material_State nvarchar(50),
					@verCode nvarchar(50),
					@Change_Code nvarchar(50), 
					@Change_Evidence_Id nvarchar(50),
					@NumCasesSumSubmit decimal(18,4),
					@DemandNumSumSubmit decimal(18,4)

			select @ItemCode = ItemCode1 ,@Rough_Spec = Rough_Spec , @Rough_Size = Rough_Size  , @Dinge_Size = Dinge_Size  , @Mat_Rough_Weigh = Mat_Rough_Weight
					,@Is_Del = Is_del, @NumCasesSum = NumCasesSum, @DemandNumSum = DemandNumSum, @Material_State = Material_State, @verCode = VerCode
			from M_Demand_DetailedList_Draft where ID = @MDDLDID
					
			select top 1 @lastMDMLItemCode = ItemCode1,  @lastMDMLRough_Spec = Rough_Spec, @lastMDMLRough_Size = Rough_Size,@lastMDMLDinge_Size = Dinge_Size,
					@lastMDMLMat_Rough_Weigh = Mat_Rough_Weight
			from M_Demand_Merge_List where Correspond_Draft_Code = @MDDLDID and Is_Submit = 'true' order by ID desc

			select @Change_Code = ChangeList_Code, @Change_Evidence_Id = ID from M_Change_List where PackID = @PackId and VerCode = @verCode

			if @Is_Del = 1 and @Material_State in ('1','2','7') select @Change_State = '1'
			else begin
				if @Material_State = '7' select @Change_State = '2'
				else begin
					if @lastMDMLItemCode != @ItemCode or @lastMDMLRough_Spec != @Rough_Spec 
						or @lastMDMLRough_Size != @Rough_Size or @lastMDMLDinge_Size != @Dinge_Size  or @lastMDMLMat_Rough_Weigh != @Mat_Rough_Weigh
					select @Change_State = '3'
					else begin
						select @NumCasesSumSubmit = sum(NumCasesSum)
							, @DemandNumSumSubmit= sum(DemandNumSum)
						from M_Demand_Merge_List where Correspond_Draft_Code = @MDDLDID and Is_Submit = 'true'

						if @NumCasesSum > @NumCasesSumSubmit select @Change_State = '5'
						else if @NumCasesSum = @NumCasesSumSubmit select @Change_State = ''
						else select @Change_State = '4'
					end
				end
			end
		end
		
		if @Change_State = '' begin
			insert M_Demand_Merge_List(Correspond_Draft_Code,Drawing_No,PackId,TaskId,DraftId,TechnicsLine,ItemCode1,NumCasesSum,
				Mat_Unit,Quantity,Rough_Size,Dinge_Size,DemandDate,MaterialsDes,Stage,Unit_Price,Sum_Price,Is_Submit,Special_Needs,Urgency_Degre,
				Secret_Level,Use_Des,Shipping_Address,Certification,[User_ID],Submit_Date,MDPId,DemandNumSum,Rough_Spec,TaskCode,
				MaterialDept,Mat_Rough_Weight,TDM_Description,
				ORG_ID, USER_RQ_NUMBER, USER_RQ_ID, USER_RQ_LINE_ID, RQ_TYPE, [ITEM_CODE],[SPECIAL_REQUEST] , [QUANTITY1], [PIECE],
				 [DIMENSION],[MANUFACTURER], [RQ_DATE], [URGENCY_LEVEL], [REQUESTER],[Requester_Phone], [ITEM_REVISION], [UNANIMOUS_BATCH]
				, [SECURITY_LEVEL] ,[PROJECT], [PHASE] ,[BATCH_QTY], [USAGE] ,[TASK] ,[SUBJECT] , [CUSTOMER_NAME],
				 [CUSTOMER_ACCOUNT_NUMBER], [DELIVERY_ADDRESS]
				, [PREPARER] , [SUBMISSION_DATE], [SYSTEM_CODE], [CREATION_DATE], [ATTRIBUTE2], [ATTRIBUTE3], [ATTRIBUTE4], [ATTRIBUTE5], Material_Name,Quantity_Left,DemandNum_Left)
			select @MDDLDID,Drawing_No,PackId,TaskId,DraftId,Technics_Line,ItemCode1,NumCasesSum,Mat_Unit,Quantity,Rough_Size,Dinge_Size,
				@tmpDateStr,MaterialsDes,Stage,@Unit_Price,@Sum_Price,1,@tmpNeedsStr,@tmpDegreStr,@tmpLevelStr,@tmpDesStr,@tmpAddrStr,
				@tmpCertStr,@userId,@curdate,@MDPLId,DemandNumSum,Rough_Spec,TaskCode,MaterialDept,Mat_Rough_Weight,TDM_Description,
				'81', @MDPL_Code, @MDPLId, @@IDENTITY, '清单需求', ItemCode1, @tmpNeedsStr, DemandNumSum, NumCasesSum, Rough_Size,@tmpManufacturerStr,
				 @tmpDateStr, @tmpDegreStr, (select UserName from Sys_UserInfo_PWD where ID = @userId )
				, (select Phone from Sys_UserInfo_PWD where ID = @userId )
				, (select isnull((select SEG8 from GetCommItem_T_Item where SEG3 = M_Demand_DetailedList_Draft.ItemCode1),0)),'Y'
				, @tmpLevelStr, @project, (select Basicdata_DICT_CODE from Sys_Phase where Code = Stage), 1, @tmpDesStr, TaskCode, Null, '天津航天长征火箭制造有限公司',@can, @tmpAddrStr
				, (select UserName from Sys_UserInfo_PWD where ID = M_Demand_DetailedList_Draft.User_ID)
				,@curdate, 'TJ-WZ', Import_Date, '' -- CUX_DM_CERTIFICATION_C的Code,
				, @tmpCertStr, Null, Drawing_No, Material_Name,NumCasesSum,DemandNumSum from M_Demand_DetailedList_Draft where Id=@MDDLDID
			set @MDML_Id= @@IDENTITY

			--更新MDPId,MDML_Id及Material_State列
			update M_Demand_DetailedList_Draft set MDPId=@MDPLId,MDML_Id=(case when MDML_Id is null or MDML_Id='' then @MDML_Id
				else MDML_Id+','+@MDML_Id end),Material_State=1 where Id=@MDDLDID
		
			Insert into WriteReqOrder_T_List (ORG_ID, USER_RQ_NUMBER, USER_RQ_ID, USER_RQ_LINE_ID, RQ_TYPE
				, [ITEM_CODE],[SPECIAL_REQUEST] , [QUANTITY], [PIECE], [DIMENSION],[MANUFACTURER], [RQ_DATE], [URGENCY_LEVEL], [REQUESTER],[Requester_Phone], [ITEM_REVISION], [UNANIMOUS_BATCH]
				, [SECURITY_LEVEL] ,[PROJECT], [PHASE] ,[BATCH_QTY], [USAGE] ,[TASK] ,[SUBJECT] , [CUSTOMER_NAME],
				 [CUSTOMER_ACCOUNT_NUMBER], [DELIVERY_ADDRESS]
				, [PREPARER] , [SUBMISSION_DATE], [SYSTEM_CODE], [CREATION_DATE], [ATTRIBUTE2], [ATTRIBUTE3], [ATTRIBUTE4], [ATTRIBUTE5])
			select '81', @user_rq_number_start + '-' + convert(nvarchar(50),@MDPLId) + '-' + convert(nvarchar(50),@MDML_Id), @MDPLId, @MDML_Id, '清单需求'
				, ItemCode1, @tmpNeedsStr, DemandNumSum, NumCasesSum, Rough_Size,@tmpManufacturerStr, @tmpDateStr, @tmpDegreStr
				, (select UserName from Sys_UserInfo_PWD where ID = @userId )
				, (select Phone from Sys_UserInfo_PWD where ID = @userId )
				, (select isnull((select SEG8 from GetCommItem_T_Item where SEG3 = M_Demand_DetailedList_Draft.ItemCode1),0)),
				'Y', @tmpLevelStr, @project, (select Basicdata_DICT_CODE from Sys_Phase where Code = Stage), 1, @tmpDesStr, TaskCode, Null, '天津航天长征火箭制造有限公司',
				 @can, @tmpAddrStr--车间在cust表的编号Account_Num, Loacatton_ID
				, (select UserName from Sys_UserInfo_PWD where ID = M_Demand_DetailedList_Draft.User_ID)
				, @curdate, 'TJ-WZ', Import_Date, '' -- CUX_DM_CERTIFICATION_C的Code,
				, @tmpCertStr, Null, Drawing_No
			from M_Demand_DetailedList_Draft where Id=@MDDLDID
		end
		else if @Change_State = '1' or @Change_State = '2' or @Change_State = '3' begin
			Insert into M_Change_Record (Change_Code, Change_Evidence_Id, Correspond_Draft_Code, MDPId, MDMId, Change_Date, Change_State, [User_ID], Column_Changed, Original_Value, Changed_Value)
			select  @Change_Code, @Change_Evidence_Id , @MDDLDID, @MDPLId, ID, @curdate, @Change_State, @userId, 'PIECE',NumCasesSum, '0'
			from M_Demand_Merge_List where Correspond_Draft_Code = @MDDLDID and NumCasesSum != 0 and Is_Submit = 'true'

			Insert into M_Change_Record (Change_Code, Change_Evidence_Id, Correspond_Draft_Code, MDPId, MDMId, Change_Date, Change_State, [User_ID], Column_Changed, Original_Value, Changed_Value)
			select  @Change_Code, @Change_Evidence_Id , @MDDLDID, @MDPLId, ID, @curdate, @Change_State, @userId, 'QUANTITY',DemandNumSum, '0'
			from M_Demand_Merge_List where Correspond_Draft_Code = @MDDLDID and DemandNumSum != 0 and Is_Submit = 'true'

			Update M_Demand_Merge_List set NumCasesSum = 0, DemandNumSum = 0,Quantity_Left=0,DemandNum_Left=0 where Correspond_Draft_Code = @MDDLDID and (NumCasesSum != 0 or DemandNumSum != 0)
		
			if @Change_State = '1' or @Change_State = '2' begin
				Update M_Demand_DetailedList_Draft set Material_State = '3' where Id = @MDDLDID
			end
			else if @Change_State = '3' begin
				insert M_Demand_Merge_List(Correspond_Draft_Code,Drawing_No,PackId,TaskId,DraftId,TechnicsLine,ItemCode1,NumCasesSum,
					Mat_Unit,Quantity,Rough_Size,Dinge_Size,DemandDate,MaterialsDes,Stage,Unit_Price,Sum_Price,Is_Submit,Special_Needs,Urgency_Degre,
					Secret_Level,Use_Des,Shipping_Address,Certification,[User_ID],Submit_Date,MDPId,DemandNumSum,Rough_Spec,TaskCode,
					MaterialDept,Mat_Rough_Weight,TDM_Description,
					ORG_ID, USER_RQ_NUMBER, USER_RQ_ID, USER_RQ_LINE_ID, RQ_TYPE, [ITEM_CODE],[SPECIAL_REQUEST] , [QUANTITY1], [PIECE],
					 [DIMENSION],[MANUFACTURER], [RQ_DATE], [URGENCY_LEVEL], [REQUESTER],[Requester_Phone], [ITEM_REVISION], [UNANIMOUS_BATCH]
					, [SECURITY_LEVEL] ,[PROJECT], [PHASE] ,[BATCH_QTY], [USAGE] ,[TASK] ,[SUBJECT] , [CUSTOMER_NAME],
					 [CUSTOMER_ACCOUNT_NUMBER], [DELIVERY_ADDRESS]
					, [PREPARER] , [SUBMISSION_DATE], [SYSTEM_CODE], [CREATION_DATE], [ATTRIBUTE2], [ATTRIBUTE3], [ATTRIBUTE4], [ATTRIBUTE5], Material_Name,Quantity_Left,DemandNum_Left)
				select @MDDLDID,Drawing_No,PackId,TaskId,DraftId,Technics_Line,ItemCode1,NumCasesSum,Mat_Unit,Quantity,Rough_Size,Dinge_Size,
					@tmpDateStr,MaterialsDes,Stage,@Unit_Price,@Sum_Price,1,@tmpNeedsStr,@tmpDegreStr,@tmpLevelStr,@tmpDesStr,@tmpAddrStr,
					@tmpCertStr,@userId,@curdate,@MDPLId,DemandNumSum,Rough_Spec,TaskCode,MaterialDept,Mat_Rough_Weight,TDM_Description,
					'81', @MDPL_Code, @MDPLId, @@IDENTITY, '清单需求', ItemCode1, @tmpNeedsStr, DemandNumSum, NumCasesSum, Rough_Size,@tmpManufacturerStr,
					 @tmpDateStr, @tmpDegreStr, (select UserName from Sys_UserInfo_PWD where ID = @userId )
					, (select Phone from Sys_UserInfo_PWD where ID = @userId )
					, (select isnull((select SEG8 from GetCommItem_T_Item where SEG3 = M_Demand_DetailedList_Draft.ItemCode1),0)),'Y'
					, @tmpLevelStr, @project, (select Basicdata_DICT_CODE from Sys_Phase where Code = Stage), 1, @tmpDesStr, TaskCode, Null, '天津航天长征火箭制造有限公司',@can, @tmpAddrStr
					, (select UserName from Sys_UserInfo_PWD where ID = M_Demand_DetailedList_Draft.User_ID)
					,@curdate, 'TJ-WZ', Import_Date, '' -- CUX_DM_CERTIFICATION_C的Code,
					, @tmpCertStr, Null, Drawing_No, Material_Name,NumCasesSum,DemandNumSum from M_Demand_DetailedList_Draft where Id=@MDDLDID
				set @MDML_Id= @@IDENTITY

				--更新MDPId,MDML_Id及Material_State列
				update M_Demand_DetailedList_Draft set MDPId=@MDPLId,MDML_Id=(case when MDML_Id is null or MDML_Id='' then @MDML_Id
					else MDML_Id+','+@MDML_Id end),Material_State=1 where Id=@MDDLDID
		
				Insert into WriteReqOrder_T_List (ORG_ID, USER_RQ_NUMBER, USER_RQ_ID, USER_RQ_LINE_ID, RQ_TYPE
					, [ITEM_CODE],[SPECIAL_REQUEST] , [QUANTITY], [PIECE], [DIMENSION],[MANUFACTURER], [RQ_DATE], [URGENCY_LEVEL], [REQUESTER],[Requester_Phone], [ITEM_REVISION], [UNANIMOUS_BATCH]
					, [SECURITY_LEVEL] ,[PROJECT], [PHASE] ,[BATCH_QTY], [USAGE] ,[TASK] ,[SUBJECT] , [CUSTOMER_NAME],
					 [CUSTOMER_ACCOUNT_NUMBER], [DELIVERY_ADDRESS]
					, [PREPARER] , [SUBMISSION_DATE], [SYSTEM_CODE], [CREATION_DATE], [ATTRIBUTE2], [ATTRIBUTE3], [ATTRIBUTE4], [ATTRIBUTE5])
				select '81', @user_rq_number_start + '-' + convert(nvarchar(50),@MDPLId) + '-' + convert(nvarchar(50),@MDML_Id), @MDPLId, @MDML_Id, '清单需求'
					, ItemCode1, @tmpNeedsStr, DemandNumSum, NumCasesSum, Rough_Size,@tmpManufacturerStr, @tmpDateStr, @tmpDegreStr
					, (select UserName from Sys_UserInfo_PWD where ID = @userId )
					, (select Phone from Sys_UserInfo_PWD where ID = @userId )
					, (select isnull((select SEG8 from GetCommItem_T_Item where SEG3 = M_Demand_DetailedList_Draft.ItemCode1),0)),
					'Y', @tmpLevelStr,@project, (select Basicdata_DICT_CODE from Sys_Phase where Code = Stage), 1, @tmpDesStr, TaskCode, Null, '天津航天长征火箭制造有限公司',
					 @can, @tmpAddrStr--车间在cust表的编号Account_Num, Loacatton_ID
					, (select UserName from Sys_UserInfo_PWD where ID = M_Demand_DetailedList_Draft.User_ID)
					, @curdate, 'TJ-WZ', Import_Date, '' -- CUX_DM_CERTIFICATION_C的Code,
					, @tmpCertStr, Null, Drawing_No
				from M_Demand_DetailedList_Draft where Id=@MDDLDID
			
				update M_Demand_DetailedList_Draft set MDPId=@MDPLId,MDML_Id=(case when MDML_Id is null or MDML_Id='' then @MDML_Id
					else MDML_Id+','+@MDML_Id end),Material_State=1 where Id=@MDDLDID
			end
		end
		else if @Change_State = '4' begin
			if exists( select * from tempdb..sysobjects where id = OBJECT_ID('tempdb..#test')) drop table #test
			create table #test
			(
				IndexID int identity(1,1),
				ID nvarchar(50),
				NumCasesSum decimal(18,4),
				DemandNumSum decimal(18,4)
			)
			Insert into #test
			select ID, NumCasesSum  ,DemandNumSum 
			from M_Demand_Merge_List 
			where Correspond_Draft_Code = @MDDLDID and (NumCasesSum != 0 or DemandNumSum != 0) and Is_Submit='true'
			order by ID

			declare @NumCasesSumDiff decimal(18,4) = (@NumCasesSumSubmit - @NumCasesSum) , 
					@DemandNumSumDiff decimal(18,4) = (@DemandNumSumSubmit - @DemandNumSum)
			
			while (select count(*) from #test) > 0 begin
				declare @numcasessum1 decimal(18,4), @demandnumsum1 decimal(18,4), @mdmlid1 nvarchar(50)
				select top 1 @numcasessum1 = NumCasesSum, @demandnumsum1 = DemandNumSum, @mdmlid1 = ID from #test order by IndexID desc

				if @NumCasesSumDiff > @numcasessum1 begin
					Insert into M_Change_Record (Change_Code, Change_Evidence_Id, Correspond_Draft_Code, MDPId, MDMId, Change_Date, Change_State, [User_ID], Column_Changed, Original_Value, Changed_Value)
					values  (@Change_Code, @Change_Evidence_Id , @MDDLDID, @MDPLId, @mdmlid1 , @curdate, @Change_State, @userId, 'PIECE', @numcasessum1, '0')

					Update M_Demand_Merge_List set NumCasesSum = 0 ,Quantity_Left=0 where ID = @mdmlid1

					select @NumCasesSumDiff = @NumCasesSumDiff - @numcasessum1	
				end
				else if @NumCasesSumDiff != 0 begin
					Insert into M_Change_Record (Change_Code, Change_Evidence_Id, Correspond_Draft_Code, MDPId, MDMId, Change_Date, Change_State, [User_ID], Column_Changed, Original_Value, Changed_Value)
					values  (@Change_Code, @Change_Evidence_Id , @MDDLDID, @MDPLId, @mdmlid1 , @curdate, @Change_State, @userId, 'PIECE', @numcasessum1, @numcasessum1 - @NumCasesSumDiff)

					Update M_Demand_Merge_List set NumCasesSum = @numcasessum1 - @NumCasesSumDiff ,Quantity_Left=@numcasessum1 - @NumCasesSumDiff where ID = @mdmlid1

					select @NumCasesSumDiff = 0
				end

				if @DemandNumSumDiff >  @demandnumsum1 begin
					Insert into M_Change_Record (Change_Code, Change_Evidence_Id, Correspond_Draft_Code, MDPId, MDMId, Change_Date, Change_State, [User_ID], Column_Changed, Original_Value, Changed_Value)
					values  (@Change_Code, @Change_Evidence_Id , @MDDLDID, @MDPLId, @mdmlid1 , @curdate, @Change_State, @userId, 'QUANTITY', @demandnumsum1, '0')

					Update M_Demand_Merge_List set DemandNumSum = 0 ,DemandNum_Left=0 where ID = @mdmlid1

					select @DemandNumSumDiff = @DemandNumSumDiff - @DemandNumSum1	
				end
				else if @DemandNumSumDiff != 0 begin
					Insert into M_Change_Record (Change_Code, Change_Evidence_Id, Correspond_Draft_Code, MDPId, MDMId, Change_Date, Change_State, [User_ID], Column_Changed, Original_Value, Changed_Value)
					values  (@Change_Code, @Change_Evidence_Id , @MDDLDID, @MDPLId, @mdmlid1 , @curdate, @Change_State, @userId, 'QUANTITY', @demandnumsum1 , @demandnumsum1 - @DemandNumSumDiff)

					Update M_Demand_Merge_List set DemandNumSum = @demandnumsum1 - @DemandNumSumDiff,DemandNum_Left = @demandnumsum1 - @DemandNumSumDiff where ID = @mdmlid1

					select @DemandNumSumDiff = 0
				end

				if @NumCasesSumDiff = 0 and @DemandNumSumDiff = 0 delete #test
				else delete #test where IndexID = (select top 1 IndexID from #test order by IndexID desc)
			end

			update M_Demand_DetailedList_Draft set Material_State=1 where Id=@MDDLDID
		end
		else if @Change_State = '5' begin
			if @NeedsStr ='' and @DegreStr = ''and @LevelStr = '' and @DesStr = '' and @AddrStr = ''and @CertStr = ''and @DateStr = '' begin
				select top 1 @tmpNeedsStr = Special_Needs , @tmpDegreStr = Urgency_Degre, @tmpLevelStr = Secret_Level
					, @tmpDesStr = Use_Des , @tmpAddrStr = Shipping_Address, @tmpCertStr = Certification, @tmpDateStr = DemandDate
				from M_Demand_Merge_List
				where Correspond_Draft_Code = @MDDLDID and (NumCasesSum != 0 or DemandNumSum != 0) and Is_Submit = 'true' order by ID desc
			end

			insert M_Demand_Merge_List(Correspond_Draft_Code,Drawing_No,PackId,TaskId,DraftId,TechnicsLine,ItemCode1,NumCasesSum,
				Mat_Unit,Quantity,Rough_Size,Dinge_Size,DemandDate,MaterialsDes,Stage,Unit_Price,Sum_Price,Is_Submit,Special_Needs,Urgency_Degre,
				Secret_Level,Use_Des,Shipping_Address,Certification,[User_ID],Submit_Date,MDPId,DemandNumSum,Rough_Spec,TaskCode,
				MaterialDept,Mat_Rough_Weight,TDM_Description,
				ORG_ID, USER_RQ_NUMBER, USER_RQ_ID, USER_RQ_LINE_ID, RQ_TYPE, [ITEM_CODE],[SPECIAL_REQUEST] , [QUANTITY1], [PIECE],
				 [DIMENSION],[MANUFACTURER], [RQ_DATE], [URGENCY_LEVEL], [REQUESTER],[Requester_Phone], [ITEM_REVISION], [UNANIMOUS_BATCH]
				, [SECURITY_LEVEL] ,[PROJECT], [PHASE] ,[BATCH_QTY], [USAGE] ,[TASK] ,[SUBJECT] , [CUSTOMER_NAME],
				 [CUSTOMER_ACCOUNT_NUMBER], [DELIVERY_ADDRESS]
				, [PREPARER] , [SUBMISSION_DATE], [SYSTEM_CODE], [CREATION_DATE], [ATTRIBUTE2], [ATTRIBUTE3], [ATTRIBUTE4], [ATTRIBUTE5], Material_Name,Quantity_Left,DemandNum_Left)
			select @MDDLDID,Drawing_No,PackId,TaskId,DraftId,Technics_Line,ItemCode1,NumCasesSum,Mat_Unit,Quantity,Rough_Size,Dinge_Size,
				@tmpDateStr,MaterialsDes,Stage,@Unit_Price,@Sum_Price,1,@tmpNeedsStr,@tmpDegreStr,@tmpLevelStr,@tmpDesStr,@tmpAddrStr,
				@tmpCertStr,@userId,@curdate,@MDPLId,DemandNumSum,Rough_Spec,TaskCode,MaterialDept,Mat_Rough_Weight,TDM_Description,
				'81', @MDPL_Code, @MDPLId, @@IDENTITY, '清单需求', ItemCode1, @tmpNeedsStr, DemandNumSum, NumCasesSum, Rough_Size,@tmpManufacturerStr,
				 @tmpDateStr, @tmpDegreStr, (select UserName from Sys_UserInfo_PWD where ID = @userId )
				, (select Phone from Sys_UserInfo_PWD where ID = @userId )
				, (select isnull((select SEG8 from GetCommItem_T_Item where SEG3 = M_Demand_DetailedList_Draft.ItemCode1),0)),'Y'
				, @tmpLevelStr, @project, (select Basicdata_DICT_CODE from Sys_Phase where Code = Stage), 1, @tmpDesStr, TaskCode, Null, '天津航天长征火箭制造有限公司',@can, @tmpAddrStr
				, (select UserName from Sys_UserInfo_PWD where ID = M_Demand_DetailedList_Draft.User_ID)
				,@curdate, 'TJ-WZ', Import_Date, '' -- CUX_DM_CERTIFICATION_C的Code,
				, @tmpCertStr, Null, Drawing_No, Material_Name,NumCasesSum,DemandNumSum from M_Demand_DetailedList_Draft where Id=@MDDLDID
			set @MDML_Id= @@IDENTITY

			--更新MDPId,MDML_Id及Material_State列
			update M_Demand_DetailedList_Draft set MDPId=@MDPLId,MDML_Id=(case when MDML_Id is null or MDML_Id='' then @MDML_Id
				else MDML_Id+','+@MDML_Id end),Material_State=1 where Id=@MDDLDID
		
			Insert into WriteReqOrder_T_List (ORG_ID, USER_RQ_NUMBER, USER_RQ_ID, USER_RQ_LINE_ID, RQ_TYPE
				, [ITEM_CODE],[SPECIAL_REQUEST] , [QUANTITY], [PIECE], [DIMENSION],[MANUFACTURER], [RQ_DATE], [URGENCY_LEVEL], [REQUESTER],[Requester_Phone], [ITEM_REVISION], [UNANIMOUS_BATCH]
				, [SECURITY_LEVEL] ,[PROJECT], [PHASE] ,[BATCH_QTY], [USAGE] ,[TASK] ,[SUBJECT] , [CUSTOMER_NAME],
				 [CUSTOMER_ACCOUNT_NUMBER], [DELIVERY_ADDRESS]
				, [PREPARER] , [SUBMISSION_DATE], [SYSTEM_CODE], [CREATION_DATE], [ATTRIBUTE2], [ATTRIBUTE3], [ATTRIBUTE4], [ATTRIBUTE5])
			select '81', @user_rq_number_start + '-' + convert(nvarchar(50),@MDPLId) + '-' + convert(nvarchar(50),@MDML_Id), @MDPLId, @MDML_Id, '清单需求'
				, ItemCode1, @tmpNeedsStr, DemandNumSum, NumCasesSum, Rough_Size,@tmpManufacturerStr, @tmpDateStr, @tmpDegreStr
				, (select UserName from Sys_UserInfo_PWD where ID = @userId )
				, (select Phone from Sys_UserInfo_PWD where ID = @userId )
				, (select isnull((select SEG8 from GetCommItem_T_Item where SEG3 = M_Demand_DetailedList_Draft.ItemCode1),0)),
				'Y', @tmpLevelStr, @project, (select Basicdata_DICT_CODE from Sys_Phase where Code = Stage), 1, @tmpDesStr, TaskCode, Null, '天津航天长征火箭制造有限公司',
				 @can, @tmpAddrStr--车间在cust表的编号Account_Num, Loacatton_ID
				, (select UserName from Sys_UserInfo_PWD where ID = M_Demand_DetailedList_Draft.User_ID)
				, @curdate, 'TJ-WZ', Import_Date, '' -- CUX_DM_CERTIFICATION_C的Code,
				, @tmpCertStr, Null, Drawing_No
			from M_Demand_DetailedList_Draft where Id=@MDDLDID
			
			update M_Demand_DetailedList_Draft set MDPId=@MDPLId,MDML_Id=(case when MDML_Id is null or MDML_Id='' then @MDML_Id
				else MDML_Id+','+@MDML_Id end),Material_State=1 where Id=@MDDLDID
		end
	end
	
	declare @groupid decimal(18,0) 
	(select  @groupid = isnull(max(Group_ID),0) + 1 from WriteRcoOrder_SentHeader)
	
	insert into WriteRcoOrder_SentHeader (USER_RCO_HEADER_NO, ORG_ID, RCO_STATUS, USER_RQ_ID,USER_RQ_LINE_ID,  
		REASON, REQUESTER, PREPARER, CREATION_DATE,  SUBMISSION_DATE,CHANGE_OPTION,  SYSTEM_CODE, GROUP_ID,  MDPLID)
	select distinct (select isnull(Code_Name,'TRCO') from Sys_CodeBuild where Code_Des = '需求变更'), '81','已提交',
		(select MDPId from M_Demand_Merge_List where ID = M_Change_Record.MDMId), MDMId,
		(select Convert(nvarchar(50),ChangeList_Code) + Convert(nvarchar(50),Remark) from M_Change_List where ID = M_Change_Record.Change_Evidence_Id),		
		((select UserName from Sys_UserInfo_PWD where ID = (select [User_ID] from M_Demand_DetailedList_Draft where ID = (select Correspond_Draft_Code from M_Demand_Merge_List where ID = M_Change_Record.MDMId)))),
		(select UserName from Sys_UserInfo_PWD where ID = @userId),
		@curdate, @curdate, 'CHANGE','TJ-WZ', @groupid ,@MDPLId
	from M_Change_Record
	where MDPId = @MDPLId

	Update WriteRcoOrder_SentHeader 
	set USER_RCO_HEADER_NO = USER_RCO_HEADER_NO + '-' + Convert(nvarchar(50), User_RQ_ID) + '-' + Convert(nvarchar(50),USER_RQ_LINE_ID) + '-' + convert(nvarchar(50),USER_RCO_HEADER_ID)
	where MDPLID =  Convert(nvarchar(50),@MDPLId)
	
	Insert Into WriteRcoOrder_SentLine (USER_RCO_LINE_ID, USER_RCO_HEADER_ID, LINE_NUM, COLUMN_CHANGED, ORIGINAL_VALUE, CHANGED_VALUE, SYSTEM_CODE, GROUP_ID)
	select ID, (select USER_RCO_HEADER_ID from WriteRcoOrder_SentHeader where  MDPLID = @MDPLId and USER_RQ_LINE_ID = MDMId)
			, (select isnull(max(Line_Num),0) + 1 from WriteRcoOrder_SentLine where USER_RCO_LINE_ID = MDMId) --变更需求行的第几次变化
			, Column_Changed , Original_Value , Changed_Value, 'TJ-WZ', ID --(select top 1 Group_ID from WriteRcoOrder_SentHeader where MDPLID =@MDPLId)
	from M_Change_Record
	where  MDPID = @MDPLId

	select @MDPLId , @MDPL_Code
if @@error<>0
	rollback transaction T
else
	commit transaction T





