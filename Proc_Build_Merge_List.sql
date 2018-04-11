USE [mms]
GO
/****** Object:  StoredProcedure [dbo].[Proc_Build_Merge_List]    Script Date: 2018-04-11 09:31:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[Proc_Build_Merge_List]
(
@DraftIdStr varchar(max)
)
as
declare @sql nvarchar(max)=''
set @sql='select vmddd.*,
	--(select top 1 CUST_ACCOUNT_ID from V_Get_Sys_Dept_ShipAddrByDeptID where DeptCode=vmddd.MaterialDept) as Shipping_Addr_Id,
	--(select top 1 LOCATION_ID from V_Get_Sys_Dept_ShipAddrByDeptID where DeptCode=vmddd.MaterialDept) as Shipping_Addr_Id,
	(select top 1 KeyWord from Sys_Dict
		join Sys_Dept_ShipAddr on Sys_Dept_ShipAddr.Shipping_Addr_Id = ''2-''+ Convert(nvarchar(50),Sys_Dict.KeyWordCode)
		 where TypeID = ''2'' and Dept_Id = (select ID from Sys_DeptEnum where DeptCode = vmddd.MaterialDept)) as Shipping_Addr_Id,
	(select top 1 DICT_CODE from GetBasicdata_T_Item where DICT_CLASS=''CUX_DM_URGENCY_LEVEL'') as Urgency_Degre,
	(select top 1 SecretLevel_Name from Sys_SecretLevel where Is_Del=0 and SecretLevel_Code=''1'') as Secret_Level,
	
	''010'' as Use_Des
	,''Y'' as Certification ,'''' as Manufacturer
	from V_M_Demand_DetailedList_Draft vmddd 
	--left join V_Get_Sys_Dept_ShipAddrByDeptID vgsds on vmddd.MaterialDept=vgsds.DeptCode 
	where id in('+@DraftIdStr+')'
print @sql
exec(@sql)





