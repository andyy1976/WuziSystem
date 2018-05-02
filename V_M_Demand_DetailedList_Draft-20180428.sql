SELECT   md.Technics_Line AS TechnicsLine, 
                (CASE WHEN md.Stage = 1 THEN 'M' WHEN md.Stage = 2 THEN 'C' WHEN md.Stage = 3 THEN 'S' WHEN md.Stage = 4 THEN
                 'D' END) AS stage1, 
                (CASE WHEN md.Material_State = 0 THEN '未提交' WHEN md.Material_State = 1 THEN '<b style="color:red;">已提交</b>'
                 WHEN md.Material_State = 2 THEN '可再次提交' ELSE '-' END) AS mstate, 
                (CASE WHEN md.ParentId = 0 THEN md.Material_Code ELSE CONVERT(nvarchar(10), md.ParentId) 
                + '-' + CONVERT(nvarchar(10), md.Material_Code) END) AS mcode, ppt.TaskDrawingCode, pp.Model, mdl.Draft_Code, 
                md.Id, md.VerCode, md.Class_Id, md.Object_Id, md.Stage, md.Material_State, md.Material_Tech_Condition, 
                md.Material_Code, md.ParentId, md.Material_Spec, md.TDM_Description, md.Material_Name, md.PackId, md.TaskId, 
                md.DraftId, md.Drawing_No, md.Technics_Line, md.Technics_Comment, md.Material_Mark, md.ItemCode1, 
                md.ItemCode2, md.MaterialsNum, md.Mat_Unit, md.LingJian_Type, md.Mat_Rough_Weight, md.Mat_Pro_Weight, 
                md.Mat_Weight, md.Mat_Efficiency, md.Mat_Comment, md.Mat_Technics, md.Rough_Spec, md.Rough_Size, 
                md.MaterialsDes, md.StandAlone, md.ThisTimeOperation, md.PredictDeliveryDate, md.DemandNumSum, 
                md.NumCasesSum, md.DemandDate, md.Quantity, md.Tech_Quantity, md.Memo_Quantity, md.Test_Quantity, 
                md.Required_Quantity, md.Other_Quantity, md.Ballon_No, md.Comment, md.Is_allow_merge, md.Import_Date, 
                md.User_ID, md.JSGS_Des, md.Is_del, md.TaskCode, md.MaterialDept, md.MissingDescription, md.MDPId, 
                md.CN_Material_State, md.MDML_Id, md.Combine_State, md.ParentId_For_Combine, md.Special_Needs, 
                md.Dinge_Size
FROM      dbo.M_Demand_DetailedList_Draft AS md INNER JOIN
                dbo.M_Draft_List AS mdl ON mdl.ID = md.DraftId INNER JOIN
                dbo.P_Pack_Task AS ppt ON ppt.TaskId = md.TaskId INNER JOIN
                dbo.P_Pack AS pp ON pp.PackId = md.PackId