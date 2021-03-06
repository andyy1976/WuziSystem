USE [mms]
GO
/****** Object:  StoredProcedure [dbo].[Proc_Get_ChangeType_ByMDDDLId]    Script Date: 2017-12-13 22:15:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[Proc_Get_ChangeType_ByMDDDLId]
(
@MDDLDID nvarchar(10),@resType nvarchar(2) output
)
as
--declare @ItemCode1 nvarchar(50)='',@Quantity int=0,@Mat_Unit nvarchar(10)='',@Rough_Size nvarchar(50),@Rough_Spec nvarchar(50),
--@ItemCode1_1 nvarchar(50)='',@Quantity_1 int=0,@Mat_Unit_1 nvarchar(10)='',@Rough_Size_1 nvarchar(50),@Rough_Spec_1 nvarchar(50),
--@cha int=0,@exist int=0,@exist1 int=0,@MDML_Id nvarchar(max)='',@index int=0,@sum int=0,@tmp_MDML_Id nvarchar(10)='',
--@QuantitySum int=0

--set @exist=(select count(*) from M_Demand_DetailedList_Draft where Id=@DraftId and Is_del=1)
--set @exist1=(select count(*) from M_Demand_Merge_List where Correspond_Draft_Code=@DraftId and Is_Change=0)
--if(@exist>0 and @exist1>0)
--begin
--	set @resType='1'
--end
--else
--begin
	--set @exist=(select count(*) from M_Demand_DetailedList_Draft where Id=@DraftId and Material_State=7)
	--set @exist1=(select count(*) from M_Demand_Merge_List where Correspond_Draft_Code=@DraftId and Is_Change=0)
	--if(@exist>0 and @exist1>0)
	--begin
	--	set @resType='2'
	--end
	--else
	--begin
	--	select @MDML_Id=MDML_Id,@Mat_Unit_1=Mat_Unit,@Rough_Size_1=Rough_Size,@ItemCode1_1=ItemCode1,@Rough_Spec_1=Rough_Spec,
	--		@Quantity_1=Quantity from M_Demand_DetailedList_Draft where id=@DraftId
	--	set @exist1=(select count(*) from M_Demand_Merge_List where Correspond_Draft_Code=@DraftId)
	--	if(@exist1>0)
	--	begin
	--		set @sum=(select [dbo].GetForNum_1(@MDML_Id))
	--		while @index<=@sum
	--		begin
	--			set @tmp_MDML_Id=(select DBO.SplitStr_1(@MDML_Id,','))
	--			set @MDML_Id=(select DBO.ReplaceStr_1(@MDML_Id,','))
	--			select @Mat_Unit=Mat_Unit,@Rough_Size=Rough_Size,@ItemCode1=ItemCode1,@Rough_Spec=Rough_Spec,@Quantity=Quantity
	--				from M_Demand_Merge_List where Correspond_Draft_Code=@DraftId and Id=@tmp_MDML_Id and Is_Change=0
	--			set @QuantitySum=@QuantitySum+@Quantity
	--			set @index=@index+1
	--		end
	--		--print @ItemCode1 print @Mat_Unit print @Rough_Size print @Rough_Spec 
	--		--print @ItemCode1_1  print @Mat_Unit_1 print @Rough_Size_1 print @Rough_Spec_1 
	--		set @ItemCode1=(select replace(@ItemCode1,' ',''))
	--		set @Rough_Size=(select replace(@Rough_Size,' ',''))
	--		set @Mat_Unit=(select replace(@Mat_Unit,' ',''))
	--		set @Rough_Spec=(select replace(@Rough_Spec,' ',''))
	--		if(@ItemCode1=@ItemCode1_1 and @Rough_Size=@Rough_Size_1 and @Mat_Unit=@Mat_Unit_1 and @Rough_Spec=@Rough_Spec_1)
	--		begin
	--			set @cha=@Quantity_1-@QuantitySum
	--			if(@cha<0)
	--				set @resType='4'
	--			else if(@cha>0)
	--				set @resType='5'
	--			else 
	--				set @resType=''
	--		end
	--		else
	--		begin
	--			set @resType='3'
	--		end
	--	end
	--	else
	--		set @resType=''
	--end
--end
	--变更类型，1：删除，2：不需要提交，3：属性变更，4：材料减少，5：材料增加，空：第一次提交
	if (select count(*) from M_Demand_Merge_List where Correspond_Draft_Code = @MDDLDID and Is_Submit = 'true') = 0 begin
		select @resType = ''
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
		from M_Demand_Merge_List where Correspond_Draft_Code = @MDDLDID and Is_Submit='true' order by ID desc

		if @Is_Del = 1 and @Material_State in ('1','2','7') select @resType = '1'
		else begin
			if @Material_State = '7' select @resType = '2'
			else begin
				if @lastMDMLItemCode != @ItemCode or @lastMDMLRough_Spec != @Rough_Spec 
					or @lastMDMLRough_Size != @Rough_Size or @lastMDMLDinge_Size != @Dinge_Size  or @lastMDMLMat_Rough_Weigh != @Mat_Rough_Weigh
				select @resType = '3'
				else begin
					select @NumCasesSumSubmit = sum(NumCasesSum)
						, @DemandNumSumSubmit= sum(DemandNumSum)
					from M_Demand_Merge_List where Correspond_Draft_Code = @MDDLDID and Is_Submit= 'true'

					if @NumCasesSum > @NumCasesSumSubmit select @resType = '5'
					else if @NumCasesSum = @NumCasesSumSubmit select @resType = ''
					else select @resType = '4'
				end
			end
		end
	end
print @resType




