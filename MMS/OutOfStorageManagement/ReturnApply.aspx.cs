using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Camc.Web.Library;
using System.Configuration;
using Telerik.Web.UI;
using mms.ReturnApplyService;

namespace mms.OutOfStorageManagement
{
    public partial class ReturnApply : System.Web.UI.Page
    {
        static string DBContractConn;
        DBInterface DBI;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserName"] == null || Session["UserId"] == null)
            {
                Response.Redirect("/Default.aspx");
            }
            DBContractConn = ConfigurationManager.ConnectionStrings["MaterialManagerSystemConnectionString"].ToString();
            DBI = DBFactory.GetDBInterface(DBContractConn);
            if (!IsPostBack)
            {
                Common.CheckPermission(Session["UserName"].ToString(), "ReturnApply", this.Page);

                string strWhere = " and ((select realnum - (select isnull(sum(nnum),0) from ReturnApply_T_Item where StockBill_T_ItemID = StockBill_T_Item.ID))> 0 or " +
                    " (select realitemnum - (select isnull(sum(itemnum),0) from ReturnApply_T_Item where StockBill_T_ItemID = StockBill_T_Item.ID)) > 0)";
                Session["GridSource"] = GetReleaseStockBill_T_Item(strWhere);

                string strSQL = " select * from Sys_UserInfo_PWD where Isdel = 'false' and DomainAccount != '' and DomainAccount is not null and Dept = (select Dept from Sys_UserInfo_PWD where ID = '" + Session["UserId"].ToString() + "')" +
                    " and ID in (select UserID from Sys_UserInRole where RoleID in (select ID from Sys_RoleInfo where RoleName like '%车%间%调%度%员%' and Is_Del ='false')) ";
                DataTable dtdd = DBI.Execute(strSQL, true);
                RDDL_DiaoDu.DataSource = dtdd;
                RDDL_DiaoDu.DataTextField = "UserName";
                RDDL_DiaoDu.DataValueField = "DomainAccount";
                RDDL_DiaoDu.DataBind();

                strSQL = " select * from Sys_UserInfo_PWD where IsDel = 'false' and DomainAccount != '' and DomainAccount is not null" +
                    " and ID in (select UserId from Sys_UserInRole where RoleID in (select ID from Sys_RoleInfo where RoleName like '%型%号%计%划%员%'))";
                DataTable dtxh = DBI.Execute(strSQL, true);
                RDDL_XingHao.DataSource = dtxh;
                RDDL_XingHao.DataTextField = "UserName";
                RDDL_XingHao.DataValueField = "DomainAccount";
                RDDL_XingHao.DataBind();

                strSQL = " select * from Sys_UserInfo_PWD where IsDel = 'false' and DomainAccount != '' and DomainAccount is not null" +
                    " and ID in (select UserId from Sys_UserInRole where RoleID in (select ID from Sys_RoleInfo where RoleName like '%物%资%计%划%员%'))";
                DataTable dtwz = DBI.Execute(strSQL, true);
                RDDL_WuZi.DataSource = dtwz;
                RDDL_WuZi.DataTextField = "UserName";
                RDDL_WuZi.DataValueField = "DomainAccount";
                RDDL_WuZi.DataBind();

                strSQL = " select * from Sys_UserInfo_PWD where ID = '" + Session["UserId"].ToString() + "'";
                DataTable dtuserinfo = DBI.Execute(strSQL, true);
                RTB_ReturnApplyUser.Text = dtuserinfo.Rows[0]["UserName"].ToString();
                RTB_ContactInformation.Text = dtuserinfo.Rows[0]["Phone"].ToString();
                RDP_ApplicationTime.SelectedDate = DateTime.Today;
            }
        }

        protected void RadGrid1_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            RadGrid1.DataSource = Session["GridSource"];
        }

        protected DataTable GetReleaseStockBill_T_Item(string strWhere)
        {
            DataTable dt = new DataTable();
            string strSQL = " select  ReleaseStockBill_T_Item.*, case when ReleaseStockBill_T_Item.jc_jstype = 'A' then '实物签收+>结算单据签收' when ReleaseStockBill_T_Item.jc_jstype = 'B' then '实物签收'" +
                " when  ReleaseStockBill_T_Item.jc_jstype = 'C' then '结算单据签收' else ReleaseStockBill_T_Item.jc_jstype end as jc_jstype1" +
                " , case when ((select realnum - (select isnull(sum(nnum),0) from ReturnApply_T_Item where StockBill_T_ItemID = StockBill_T_Item.ID and State = '1'))> 0 or " +
                " (select realitemnum - (select isnull(sum(itemnum),0) from ReturnApply_T_Item where StockBill_T_ItemID = StockBill_T_Item.ID and State = '1')) > 0) then 'false' else 'true' end as ReturnApplyState" +
                " , 'false' as ReturnApply" +
                " , (select realnum - (select isnull(sum(nnum),0) from ReturnApply_T_Item where StockBill_T_ItemID = StockBill_T_Item.ID and State = '1')) as CanReturnnnum" +
                " , (select realitemnum - (select isnull(sum(itemnum),0) from ReturnApply_T_Item where StockBill_T_ItemID = StockBill_T_Item.ID and State = '1')) as CanReturnitemnum" +
                " , '0' as nnum, '0' as itemnum, '' as returnnote, StockBill_T_Item.ID as StockBill_T_ItemID" +
                " from StockBill_T_Item join ReleaseStockBill_T_Item on StockBill_T_Item.ReleaseStockBill_T_ItemID = ReleaseStockBill_T_Item.ID where 1 = 1" + strWhere + " order by billdate desc";
            dt = Common.AddTableRowsID(DBI.Execute(strSQL, true));
            return dt;
        }

        protected void RB_Search_Click(object sender, EventArgs e)
        {
            string billno = RTB_billno.Text.Trim();
            string Startbilldate = RDP_Startbilldate.SelectedDate.ToString();
            string Endbilldate = RDP_Endbilldate.SelectedDate.ToString();
            string invcode = RTB_invcode.Text.Trim();
            string invname = RTB_invname.Text.Trim();
            string hgz_no = RTB_hgz_no.Text.Trim();
            string jc_rwh = RTB_jc_rwh.Text.Trim();
            string jc_th = RTB_jc_th.Text.Trim();
            string hgz_zjdbillno = RTB_hgz_zjdbillno.Text.Trim();
            string hgz_zydh = RTB_hgz_zydh.Text.Trim();
            string ReturnApply = RDDL_ReturnApplyState.SelectedValue.ToString();

            string strWhere = "";
            if (billno != "")
            {
                strWhere += " and billno like '%" + billno + "%'";
            }
            if (Startbilldate != "" && Startbilldate != null)
            {
                strWhere += " and billdate >= '" + Convert.ToDateTime(Startbilldate).ToString("yyyy-MM-dd") + "'";
            }
            if (Endbilldate != "" && Endbilldate != null)
            {
                strWhere += " and billdate <= '" + Convert.ToDateTime(Endbilldate).ToString("yyyy-MM-dd") + "'";
            }
            if (invcode != "")
            {
                strWhere += " and invcode like '%" + invcode + "%'";
            }
            if (invname != "")
            {
                strWhere += " and invname like '%" + invname + "%'";
            }
            if (hgz_no != "")
            {
                strWhere += " and hgz_no like '%" + hgz_no + "%'";
            }
            if (jc_rwh != "")
            {
                strWhere += " and jc_rwh like '%" + jc_rwh + "%'";
            }
            if (jc_th != "")
            {
                strWhere += " and jc_th like '%" + jc_th + "%'";
            }
            if (hgz_zjdbillno != "")
            {
                strWhere += " and hgz_zjdbillno like '%" + hgz_zjdbillno + "%'";
            }
            if (hgz_zydh != "")
            {
                strWhere += " and hgz_zydh like '%" + hgz_zydh + "%'";
            }
            if (ReturnApply != "0")
            {
                if (ReturnApply == "1")
                {
                    strWhere += " and (select count(*) from Stockbill_T_Item where ReleaseStockBill_T_ItemID = ReleaseStockBill_T_Item.ID)  = 0";
                }
                else
                {
                    strWhere += " and (select count(*) from Stockbill_T_Item where ReleaseStockBill_T_ItemID = ReleaseStockBill_T_Item.ID)  > 0";
                }
            }
            Session["GridSource"] = GetReleaseStockBill_T_Item(strWhere);
            RadGrid1.Rebind();
        }

        protected void RB_ReturnApply_CheckedChanged(object sender, EventArgs e)
        {
            RadButton RB = sender as RadButton;
            string id = (RB.Parent.Parent as GridDataItem).GetDataKeyValue("ID").ToString();
            (Session["GridSource"] as DataTable).Select("ID='" + id + "'")[0]["ReturnApply"] = RB.Checked.ToString().ToLower();
        }

        protected void RB_AllReturnApply_CheckedChanged(object sender, EventArgs e)
        {
            RadButton RB = sender as RadButton;

            foreach (var item in RadGrid1.Items)
            {
                if (item is GridDataItem)
                {
                    string id = (item as GridDataItem).GetDataKeyValue("ID").ToString();
                    ((item as GridDataItem).FindControl("RB_ReturnApply") as RadButton).Checked = RB.Checked;
                    if ((Session["GridSource"] as DataTable).Select("ID='" + id + "'")[0]["ReturnApplyState"].ToString() == "false")
                    {
                        (Session["GridSource"] as DataTable).Select("ID='" + id + "'")[0]["ReturnApply"] = RB.Checked.ToString().ToLower();
                    }
                }
            }
        }

        protected void RTB_returnnote_TextChanged(object sender, EventArgs e)
        {
            RadTextBox returnnote = sender as RadTextBox;
            string id = (returnnote.Parent.Parent as GridDataItem).GetDataKeyValue("ID").ToString();
            (Session["GridSource"] as DataTable).Select("ID='" + id + "'")[0]["returnnote"] = returnnote.Text.Trim();
        }

        protected void RTB_nnum_TextChanged(object sender, EventArgs e)
        {
            RadTextBox nnum = sender as RadTextBox;
            string id = (nnum.Parent.Parent as GridDataItem).GetDataKeyValue("ID").ToString();
            (Session["GridSource"] as DataTable).Select("ID='" + id + "'")[0]["nnum"] = nnum.Text.Trim();
        }

        protected void RTB_itemnum_TextChanged(object sender, EventArgs e)
        {
            RadTextBox itemnum = sender as RadTextBox;
            string id = (itemnum.Parent.Parent as GridDataItem).GetDataKeyValue("ID").ToString();
            (Session["GridSource"] as DataTable).Select("ID='" + id + "'")[0]["itemnum"] = itemnum.Text.Trim();
        }

        protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                string id = (e.Item as GridDataItem).GetDataKeyValue("ID").ToString();
                DataRow datarow = (Session["GridSource"] as DataTable).Select("ID='" + id + "'")[0];
                RadButton RB_ReturnApply = (e.Item as GridDataItem).FindControl("RB_ReturnApply") as RadButton;
                if (Convert.ToDouble(datarow["CanReturnnnum"]) == 0 && Convert.ToDouble(datarow["CanReturnitemnum"]) == 0)
                {
                    RB_ReturnApply.Enabled = false;
                }
                else {
                    bool ReturnApply = Convert.ToBoolean(datarow["ReturnApply"]);
                    if (ReturnApply == true)
                    {
                        RB_ReturnApply.Checked = true;
                    }
                }
            }
        }

        protected void RadGrid1_ItemCommand(object sender, GridCommandEventArgs e)
        {
            var db = new MMSDbDataContext();
            if (e.CommandName == "ReturnApply")
            {
                #region 使用领料单退库

                if (RadGrid1.SelectedItems.Count == 0)
                {
                    RadNotificationAlert.Text = "请选择退回的行！";
                    RadNotificationAlert.Show();
                    return;
                }

                var ReturnApplyUser = RTB_ReturnApplyUser.Text.Trim();
                var ApplicationTime = RDP_ApplicationTime.SelectedDate;
                var ContactInformation = RTB_ContactInformation.Text.Trim();
                var returnnote = RTB_returnnote.Text.Trim();
                var nnum = RTB_nnum.Text.Trim();
                var itemnum = RTB_itemnum.Text.Trim();
                var DiaoDu = RDDL_DiaoDu.SelectedValue;
                var XingHao = RDDL_XingHao.SelectedValue;
                var WuZi = RDDL_WuZi.SelectedValue;

                if (ReturnApplyUser == "")
                {
                    RadNotificationAlert.Text = "请输入：退库人";
                    RadNotificationAlert.Show();
                    return;
                }
                try
                {
                    Convert.ToDateTime(ApplicationTime);
                }
                catch
                {
                    RadNotificationAlert.Text = "请输入：退库时间";
                    RadNotificationAlert.Show();
                    return;
                }
                if (returnnote == "")
                {
                    RadNotificationAlert.Text = "请输入：退库原因";
                    RadNotificationAlert.Show();
                    return;
                }
                if (nnum == "")
                {
                    RadNotificationAlert.Text = "请输入：退库数量";
                    RadNotificationAlert.Show();
                    return;
                }
                if (itemnum == "")
                {
                    RadNotificationAlert.Text = "请输入：退库件数";
                    RadNotificationAlert.Show();
                    return;
                }
                if (DiaoDu == "")
                {
                    RadNotificationAlert.Text = "请选择 车间调度员";
                    RadNotificationAlert.Show();
                    return;
                }
                if (XingHao == "")
                {
                    RadNotificationAlert.Text = "请选择 型号计划员";
                    RadNotificationAlert.Show();
                    return;
                }
                if (WuZi == "")
                {
                    RadNotificationAlert.Text = "请选择 物资计划员";
                    RadNotificationAlert.Show();
                    return;
                }

                try
                {
                    Convert.ToDouble(nnum);
                }
                catch {
                    RadNotificationAlert.Text = "退库数量：请输入数字";
                    RadNotificationAlert.Show();
                    return;
                }
                try
                {
                    Convert.ToDouble(itemnum);
                }
                catch
                {
                    RadNotificationAlert.Text = "退库件数：请输入数字";
                    RadNotificationAlert.Show();
                    return;
                }

                var CanReturnnnum = (RadGrid1.SelectedItems[0] as GridDataItem)["CanReturnnnum"].Text;
                var CanReturnitemnum = (RadGrid1.SelectedItems[0] as GridDataItem)["CanReturnitemnum"].Text;
                if (Convert.ToDouble(nnum) <= 0)
                {
                    RadNotificationAlert.Text = "退库数量：不可以小于等于0";
                    RadNotificationAlert.Show();
                    return;
                }
                if (Convert.ToDouble(nnum) > Convert.ToDouble(CanReturnnnum))
                {
                    RadNotificationAlert.Text = "退库数量：不可以大于可退数量";
                    RadNotificationAlert.Show();
                    return;
                }
                if (Convert.ToDouble(itemnum) <= 0)
                {
                    RadNotificationAlert.Text = "退库件数：不可以小于等于0";
                    RadNotificationAlert.Show();
                    return;
                }
                if (Convert.ToDouble(itemnum) > Convert.ToDouble(CanReturnitemnum))
                {
                    RadNotificationAlert.Text = "退库件数：不可以大于可退件数";
                    RadNotificationAlert.Show();
                    return;
                }

                DataRow datarow = (Session["GridSource"] as DataTable).Select("ID='" + (RadGrid1.SelectedItems[0] as GridDataItem).GetDataKeyValue("ID") + "'")[0];

                string billno = datarow["billno"].ToString(),           //是出库单的单据号，还是本系统的退库单的单据号
                        billdate = DateTime.Today.ToString("yyyy-MM-dd"),
                        replacecorp = datarow["replacecorp"].ToString(),        //是不是 出库单中的客户单位， 领用单位，还是其他单位 ，暂取 领用单位
                        replacedept = "",                                       //
                        replaceuser = RTB_ReturnApplyUser.Text.Trim(),
                        accountcorp = datarow["accountcorp"].ToString(),           //暂取 出库单中的客户单位
                        accountmode = datarow["accountmode"].ToString(),
                        distraddress = datarow["distraddress"].ToString(),          //暂取 出库单的配送地
                        jc_Phone = datarow["jc_phone"].ToString(),                  //暂取 出库单的联系人/联系电话
                        invcode = datarow["invcode"].ToString(),                                   
                        vdefstr1 = "",
                        cgeneralbid = datarow["cgeneralbid"].ToString(),
                        cgeneralhid = datarow["cgeneralhid"].ToString(),
                        pk_Zrr = "",
                        stockBill_T_ItemID = datarow["stockBill_T_ItemID"].ToString();

                var item = new returnapply_T_Item()
                {
                    billno = billno,
                    billdate = billdate,
                    replacecorp = replacecorp,
                    replacedept = replacedept,
                    replaceuser = replaceuser,
                    accountcorp = accountcorp,
                    accountmode = accountmode,
                    distraddress = distraddress,
                    jc_Phone = jc_Phone,
                    invcode = invcode,
                    nnum =  Convert.ToDecimal(nnum),
                    itemnum = Convert.ToDecimal(itemnum),
                    returnnote = returnnote,
                    vdefstr1 = vdefstr1,
                    cgeneralbid = cgeneralbid,
                    cgeneralhid = cgeneralhid,
                    pk_Zrr = pk_Zrr,
                    stockBill_T_ItemID = stockBill_T_ItemID,
                    SentID = "0",
                    State = 1
                };
                db.returnapply_T_Item.InsertOnSubmit(item);
                db.SubmitChanges();

                nnum = (0 - Convert.ToDecimal(nnum)).ToString();
                itemnum = (0 - Convert.ToDecimal(itemnum)).ToString();

                var maidold = datarow["billno"].ToString();               
                var maold = db.MaterialApplication.SingleOrDefault(p => p.Id.ToString() == maidold.ToString());
                
                MaterialApplication ma = new MaterialApplication();
                if (maold == null)
                {
                    ma.Applicant = ReturnApplyUser;
                    ma.ApplicationTime = ApplicationTime;
                    ma.AppState = 1;
                    ma.CN_Material_State = datarow["invstatus"].ToString();
                    ma.ContactInformation = ContactInformation;
                    ma.Dept = "";                                                              /////////////////////////
                    ma.DiaoDuApprove = DiaoDu;
                    ma.Draft_Code = "";
                    ma.Drawing_No = datarow["jc_th"].ToString();
                    //ma.FeedingTime = "";                                                         ////////////////
                    //ma.Id = "";
                    ma.Is_Del = false;
                    ma.Is_ReturnApply = true;
                    //ma.IsConfirm = "";                                                           ////////////////
                    //ma.IsDispatch = "";                                                            //////////////
                    ma.ItemCode = datarow["invcode"].ToString();
                    //ma.Mat_Rough_Weight = "";                                                   /////////////////
                    ma.Mat_Unit = datarow["invmeasname"].ToString();
                    try
                    {
                        ma.Material_Id = Convert.ToInt32(datarow["rq_bodyid"].ToString());              ////////////////
                    }
                    catch { }
                    ma.Material_Mark = datarow["invxhcpdh"].ToString();
                    ma.Material_Name = datarow["invname"].ToString();
                    ma.Material_Tech_Condition = datarow["techconditions"].ToString();
                    ma.MaterialType = "";                                                              /////////////////
                    ma.PleaseTakeQuality = nnum;
                    ma.Quantity = Convert.ToInt32(itemnum);
                    ma.Remark = returnnote;
                    ma.retrunapply_T_Item_ID = datarow["ID"].ToString();
                    ma.ReturnReason = "";
                    ma.Rough_Size = datarow["jc_cc"].ToString();
                 //   ma.Dinge_Size = datarow["jc_cc"].ToString();
                    ma.Rough_Spec = datarow["invscale"].ToString();
                    ma.TaskCode = datarow["jc_rwh"].ToString();
                    ma.TheMaterialWay = "";                                                        ///////////////////
                    ma.TuiKuContext = datarow["billno"].ToString() + "-" + datarow["crowno"].ToString();
                    ma.Type = -1;                                                                ////////////////////
                    ma.UserAccount = DBI.GetSingleValue("select DomainAccount from Sys_UserInfo_PWD where ID = '" + Session["UserId"].ToString() + "'");
                    ma.UserId = Convert.ToInt32(Session["UserId"]);
                    ma.WuZiJiHuaYuanApprove = WuZi;
                    ma.XingHaoJiHuaYuanApprove = XingHao;
                }
                else {
                    ma.Applicant = ReturnApplyUser;
                    ma.ApplicationTime = ApplicationTime;
                    ma.AppState = 1;
                    ma.CN_Material_State = maold.CN_Material_State;
                    ma.ContactInformation = ContactInformation;
                    ma.Dept = maold.Dept;
                    ma.DiaoDuApprove = DiaoDu;
                    ma.Draft_Code = maold.Draft_Code;
                    ma.Drawing_No = maold.Drawing_No;
                    ma.FeedingTime = maold.FeedingTime;
                    //ma.Id = "";
                    ma.Is_Del = false;
                    ma.Is_ReturnApply = true;
                    ma.IsConfirm = maold.IsConfirm;
                    ma.IsDispatch = maold.IsDispatch; 
                    ma.ItemCode = maold.ItemCode;
                    ma.Mat_Rough_Weight = maold.Mat_Rough_Weight; 
                    ma.Mat_Unit = maold.Mat_Unit;
                    ma.Material_Id =maold.Material_Id;
                    ma.Material_Mark = maold.Material_Mark;
                    ma.Material_Name = maold.Material_Name;
                    ma.Material_Tech_Condition =maold.Material_Tech_Condition;
                    ma.MaterialType = maold.MaterialType;
                    ma.PleaseTakeQuality = nnum;
                    ma.Quantity = Convert.ToInt32(itemnum);
                    ma.Remark = returnnote;
                    ma.retrunapply_T_Item_ID = datarow["ID"].ToString();
                    ma.ReturnReason = "";
                    ma.Rough_Size = maold.Rough_Size;
                    ma.Rough_Spec = maold.Rough_Spec;
                 //   ma.Dinge_Size = maold.Dinge_Size;
                    ma.TaskCode =maold.TaskCode;
                    ma.TheMaterialWay = maold.TheMaterialWay; 
                    ma.TuiKuContext = datarow["billno"].ToString() + "-" + datarow["crowno"].ToString();
                    ma.Type = maold.Type; 
                    ma.UserAccount = DBI.GetSingleValue("select DomainAccount from Sys_UserInfo_PWD where ID = '" + Session["UserId"].ToString() + "'");
                    ma.UserId = Convert.ToInt32(Session["UserId"]);
                    ma.WuZiJiHuaYuanApprove = WuZi;
                    ma.XingHaoJiHuaYuanApprove = XingHao;
                }
                
                db.MaterialApplication.InsertOnSubmit(ma);
                db.SubmitChanges();
                string maid = ma.Id.ToString();

                K2BLL k2Bll = new K2BLL();
                var result = k2Bll.StartNewProcess(maid);
                
                if (result == "")
                {
                    RadWindowConfirm.Style.Add("display", "none");
                    RadNotificationAlert.Text = "成功进入流程平台";
                    RadNotificationAlert.Show();

                    (Session["GridSource"] as DataTable).Select("ID='" + (RadGrid1.SelectedItems[0] as GridDataItem).GetDataKeyValue("ID") + "'")[0]["CanReturnitemnum"] = (Convert.ToDouble(CanReturnitemnum) + Convert.ToDouble(itemnum)).ToString();
                    (Session["GridSource"] as DataTable).Select("ID='" + (RadGrid1.SelectedItems[0] as GridDataItem).GetDataKeyValue("ID") + "'")[0]["CanReturnnnum"] = (Convert.ToDouble(CanReturnitemnum) + Convert.ToDouble(nnum)).ToString();
                    RadGrid1.Rebind();
                }
                else {
                    item.State = 0;
                    ma.Is_Del = true;
                    db.SubmitChanges();

                    RadNotificationAlert.Text = result;
                    RadNotificationAlert.Show();
                }
               

                #endregion

                #region   使用ReturnApplyStart退库
                //int row_Count = 0;
                //DataRow[] datarows = (Session["GridSource"] as DataTable).Select("ReturnApplystate='false' and ReturnApply = 'true'");
                //return;

                //for (int i = 0; i < datarows.Length; i++)
                //{
                //    DataRow datarow = datarows[i];
                //    string id = datarow["ID"].ToString();
                //    bool ReturnApplystate = Convert.ToBoolean(datarow["ReturnApplyState"]);
                //    bool ReturnApply = Convert.ToBoolean(datarow["ReturnApply"]);
                //    if (ReturnApplystate == false)
                //    {
                //        if (ReturnApply == true)
                //        {
                //            string rowsid = datarow["RowsId"].ToString();
                //            string returnnote = datarow["returnnote"].ToString();
                //            if (returnnote == "")
                //            {
                //                RadNotificationAlert.Text = "序号" + rowsid + "行，没有录入退库原因"; ;
                //                RadNotificationAlert.Show();
                //                return;
                //            }

                //            string nnum = datarow["nnum"].ToString();
                //            if (nnum == "")
                //            {
                //                RadNotificationAlert.Text = "序号" + rowsid + "行，没有录入退库数量"; ;
                //                RadNotificationAlert.Show();
                //                return;
                //            }
                //            try
                //            {
                //                Convert.ToDouble(nnum);
                //                if (Convert.ToDouble(nnum) > Convert.ToDouble(datarow["CanReturnnnum"]))
                //                {
                //                    RadNotificationAlert.Text = "序号" + rowsid + "行，退库数量：不可以大于可退回数量"; ;
                //                    RadNotificationAlert.Show();
                //                    return;
                //                }
                //                if (Convert.ToDouble(nnum) <= 0)
                //                {
                //                    RadNotificationAlert.Text = "序号" + rowsid + "行，退库数量：不可以小于0"; ;
                //                    RadNotificationAlert.Show();
                //                    return;
                //                }
                //            }
                //            catch
                //            {
                //                RadNotificationAlert.Text = "序号" + rowsid + "行，退库数量：请输入数字"; ;
                //                RadNotificationAlert.Show();
                //                return;
                //            }

                //            string itemnum = datarow["itemnum"].ToString();
                //            if (itemnum == "")
                //            {
                //                RadNotificationAlert.Text = "序号" + rowsid + "行，没有录入退库件数"; ;
                //                RadNotificationAlert.Show();
                //                return;
                //            }
                //            try
                //            {
                //                Convert.ToDouble(itemnum);
                //                if (Convert.ToDouble(itemnum) > Convert.ToDouble(datarow["CanReturnitemnum"]))
                //                {
                //                    RadNotificationAlert.Text = "序号" + rowsid + "行，退库件数：不可以大于可退回件数"; ;
                //                    RadNotificationAlert.Show();
                //                    return;
                //                }
                //                if (Convert.ToDouble(itemnum) < 0)
                //                {
                //                    RadNotificationAlert.Text = "序号" + rowsid + "行，退库件数：不可以小于0"; ;
                //                    RadNotificationAlert.Show();
                //                    return;
                //                }
                //            }
                //            catch
                //            {
                //                RadNotificationAlert.Text = "序号" + rowsid + "行，退库件数：请输入数字"; ;
                //                RadNotificationAlert.Show();
                //                return;
                //            }
                //            row_Count++;
                //        }
                //    }
                //}
                //if (row_Count == 0)
                //{
                //    RadNotificationAlert.Text = "失败！请选择退库行";
                //    RadNotificationAlert.Show();
                //    return;
                //}


                //DBI.OpenConnection();
                //try
                //{
                //    DBI.BeginTrans();
                //    string customerSysCode = "TJ_WZ", customerSyspwd = "TJ_WZ", customerSysIp = "10.20.232.48", customerSysPort = "80";    ///数据
                //    string strSQL = "";
                //    //string strSQL = " insert into ReturnApply_Sent (customerSysCode, customerSyspwd, customerSysIp, customerSysPort, row_Count)" +
                //    //    " values ('" + customerSysCode + "','" + customerSyspwd + "','" + customerSysIp + "','" + customerSysPort + "','" + row_Count.ToString() + "')" +
                //    //    " select @@identity";
                //    //string sentID = DBI.GetSingleValue(strSQL).ToString();

                //    var sent = new returnapply_Sent()
                //    {
                //        customerSysCode = customerSysCode,
                //        customerSyspwd = customerSyspwd,
                //        customerSysIp = customerSysIp,
                //        customerSysPort = customerSysPort,
                //        row_Count = row_Count,
                //    };
                //    db.returnapply_Sent.InsertOnSubmit(sent);
                //    db.SubmitChanges();
                //    string sentID = sent.ID.ToString();

                //    for (int i = 0; i < datarows.Length; i++)
                //    {
                //        DataRow datarow = datarows[i];
                //        string id = datarow["ID"].ToString();
                //        bool ReturnApplystate = Convert.ToBoolean(datarow["ReturnApplyState"]);
                //        bool ReturnApply = Convert.ToBoolean(datarow["ReturnApply"]);
                //        if (ReturnApplystate == false)
                //        {
                //            if (ReturnApply == true)
                //            {
                //                string billno = datarow["billno"].ToString(),           //是出库单的单据号，还是本系统的退库单的单据号
                //                    billdate = DateTime.Today.ToString("yyyy-MM-dd"),
                //                    replacecorp = datarow["replacecorp"].ToString(),        //是不是 出库单中的客户单位， 领用单位，还是其他单位 ，暂取 领用单位
                //                    replacedept = "",                                       //
                //                    replaceuser = RTB_ReturnApplyUser.Text.Trim(),
                //                    accountcorp = datarow["accountcorp"].ToString(),           //暂取 出库单中的客户单位
                //                    accountmode = datarow["accountmode"].ToString(),
                //                    distraddress = datarow["distraddress"].ToString(),          //暂取 出库单的配送地
                //                    jc_Phone = datarow["jc_phone"].ToString(),                  //暂取 出库单的联系人/联系电话
                //                    invcode = datarow["invcode"].ToString(),
                //                    nnum = datarow["nnum"].ToString(),
                //                    itemnum = datarow["itemnum"].ToString(),
                //                    returnnote = datarow["returnnote"].ToString(),
                //                    vdefstr1 = "",
                //                    cgeneralbid = datarow["cgeneralbid"].ToString(),
                //                    cgeneralhid = datarow["cgeneralhid"].ToString(),
                //                    pk_Zrr = "",
                //                    stockBill_T_ItemID = datarow["stockBill_T_ItemID"].ToString();

                //                //strSQL = " Insert into ReturnApply_T_Item (billno,  billdate, replacecorp, replacedept, replaceuser," +
                //                //    " accountcorp, accountmode, distraddress, jc_Phone, invcode, nnum, itemnum, returnnote, vdefstr1," +
                //                //    " cgeneralbid, cgeneralhid, pk_Zrr, stockBill_T_ItemID, SentID)" +
                //                //    " values('" + billno + "','" + billdate + "','" + replacecorp + "','" + replacedept + "'" +
                //                //    " ,'" + replaceuser + "','" + accountcorp + "','" + accountmode + "','" + distraddress + "','" + jc_Phone + "','" + invcode + "'" +
                //                //    " ,'" + nnum + "','" + itemnum + "','" + returnnote + "','" + vdefstr1 + "','" + cgeneralbid + "'," +
                //                //    " '" + cgeneralhid + "','" + pk_Zrr + "','" + stockBill_T_ItemID + "','" + sentID + "')";
                //                //DBI.Execute(strSQL);

                //                var item = new returnapply_T_Item()
                //                {
                //                    billno = billno,
                //                    billdate = billdate,
                //                    replacecorp = replacecorp,
                //                    replacedept = replacedept,
                //                    replaceuser = replaceuser,
                //                    accountcorp = accountcorp,
                //                    accountmode = accountmode,
                //                    distraddress = distraddress,
                //                    jc_Phone = jc_Phone,
                //                    invcode = invcode,
                //                    nnum = 0 - Convert.ToDecimal(nnum),
                //                    itemnum = 0 - Convert.ToDecimal(itemnum),
                //                    returnnote = returnnote,
                //                    vdefstr1 = vdefstr1,
                //                    cgeneralbid = cgeneralbid,
                //                    cgeneralhid = cgeneralhid,
                //                    pk_Zrr = pk_Zrr,
                //                    stockBill_T_ItemID = stockBill_T_ItemID,
                //                    SentID = sentID
                //                };
                //                db.returnapply_T_Item.InsertOnSubmit(item);
                //                db.SubmitChanges();
                //            }
                //        }
                //    }

                //    ReturnApplyStart(sentID);

                //    string strWhere = " and ((select realnum - (select isnull(sum(nnum),0) from ReturnApply_T_Item where StockBill_T_ItemID = StockBill_T_Item.ID))> 0 or " +
                //     " (select realitemnum - (select isnull(sum(itemnum),0) from ReturnApply_T_Item where StockBill_T_ItemID = StockBill_T_Item.ID)) > 0)";
                //    Session["GridSource"] = GetReleaseStockBill_T_Item(strWhere);
                //    RadGrid1.Rebind();
                //    RadNotificationAlert.Text = "退库成功！";
                //    RadNotificationAlert.Show();

                //    DBI.CommitTrans();
                //}
                //catch (Exception ex)
                //{
                //    DBI.RollbackTrans();
                //    RadNotificationAlert.Text = "退库失败！" + ex.Message.ToString();
                //    RadNotificationAlert.Show();
                //}
                //finally
                //{
                //    DBI.CloseConnection();
                //} 
                #endregion

            }
        }

        protected void ReturnApplyStart(string sentID)
        {
            var db = new MMSDbDataContext();
            ReturnApplyServicePortTypeClient Client = new ReturnApplyServicePortTypeClient();

            var sent = (from p in db.returnapply_Sent where p.ID == Convert.ToInt32(sentID) select p).Take(1).ToList();
            var item = (from p in db.returnapply_T_Item where p.SentID == sentID select p).ToList();
         
            ReturnApplyParamBillinfo[] info = new ReturnApplyParamBillinfo[item.Count];
            for (int i = 0; i < item.Count; i++)
            {
                ReturnApplyParamBillinfo info1 = new ReturnApplyParamBillinfo() {
                    accountcorp = item[i].accountcorp,
                    accountmode = item[i].accountmode ,
                    billdate = item[i].billdate,
                    billno = item[i].billno,
                    cgeneralbid = item[i].cgeneralbid,
                    cgeneralhid = item[i].cgeneralhid,
                    distraddress = item[i].distraddress,
                    invcode = item[i].invcode,
                    itemnum = item[i].itemnum,
                    itemnumSpecified=true,
                    jc_Phone = item[i].jc_Phone,
                    nnum = item[i].nnum,
                    nnumSpecified = true,
                    pk_Zrr = item[i].pk_Zrr,
                    replacecorp = item[i].replacecorp,
                    replacedept = item[i].replacedept,
                    replaceuser = item[i].replaceuser,
                    returnnote = item[i].returnnote,
                    userSysBillBid = item[i].userSysBillBid.ToString() ,
                    userSysBillHid = item[i].userSysBillHid.ToString(),
                    vdefdou1 = item[i].vdefdou1,
                    vdefdou10 = item[i].vdefdou10,
                    vdefdou10Specified = true,
                    vdefdou1Specified = true,
                    vdefdou2 = item[i].vdefdou2,
                    vdefdou2Specified =true,
                    vdefdou3 = item[i].vdefdou3,
                    vdefdou3Specified = true,
                    vdefdou4 = item[i].vdefdou4,
                    vdefdou4Specified = true,
                    vdefdou5 = item[i].vdefdou5,
                    vdefdou5Specified = true,
                    vdefdou6 = item[i].vdefdou6,
                    vdefdou6Specified = true,
                    vdefdou7 = item[i].vdefdou7,
                    vdefdou7Specified = true,
                    vdefdou8 = item[i].vdefdou8 ,
                    vdefdou8Specified=true,
                    vdefdou9 = item[i].vdefdou9,
                    vdefdou9Specified = true,
                    vdefstr1 = item[i].vdefstr1,
                    vdefstr10 = item[i].vdefstr10,
                    vdefstr2 = item[i].vdefstr2,
                    vdefstr3 = item[i].vdefstr3,
                    vdefstr4 = item[i].vdefstr4 ,
                    vdefstr5 = item[i].vdefstr5,
                    vdefstr6 = item[i].vdefstr6 ,
                    vdefstr7 = item[i].vdefstr7,
                    vdefstr8 = item[i].vdefstr8,
                    vdefstr9 = item[i].vdefstr9
                      
                };
                info[i] = info1;
            }

            ReturnApplyParam param = new ReturnApplyParam()
            {
                customerSysCode = sent[0].customerSysCode,
                customerSysIp = sent[0].customerSysIp,
                customerSysPort = sent[0].customerSysPort,
                customerSyspwd = sent[0].customerSyspwd,
                returnApplyRequestBillinfo = info,
                row_Count = info.Length,
                row_CountSpecified = true,
                vdefDouble1 = sent[0].vdefDouble1,
                vdefDouble1Specified = true,
                vdefDouble2 = sent[0].vdefDouble2,
                vdefDouble2Specified = true,
                vdefDouble3 = sent[0].vdefDouble3,
                vdefDouble3Specified = true,
                vdefInteger1 = sent[0].vdefInteger1,
                vdefInteger1Specified = true,
                vdefInteger2 = sent[0].vdefInteger2,
                vdefInteger2Specified = true,
                vdefInteger3 = sent[0].vdefInteger3,
                vdefInteger3Specified = true,
                vdefString1 = sent[0].vdefString1,
                vdefString2 = sent[0].vdefString2,
                vdefString3 = sent[0].vdefString3
            };

            ReturnApplyResponse[] reclist = Client.returnApplyInfo(param);

            for (int i = 0; i < reclist.Length; i++)
            {
                ReturnApply_Rec rec = new ReturnApply_Rec() { 
                    billNo = reclist[i].billNo,
                    errorInfo = reclist[i].errorInfo,
                    returnresult = reclist[i].returnResult,
                    userSysBillBid = reclist[i].userSysBillBid,
                    userSysBillHid = reclist[i].userSysBillHid,
                    vdefDouble1= reclist[i].vdefDouble1,
                    vdefDouble10 =reclist[i].vdefDouble10,
                    vdefDouble2 = reclist[i].vdefDouble2,
                    vdefDouble3 = reclist[i].vdefDouble3,
                    vdefDouble4 = reclist[i].vdefDouble4,
                    vdefDouble5 = reclist[i].vdefDouble5,
                    vdefDouble6 = reclist[i].vdefDouble6,
                    vdefDouble7 = reclist[i].vdefDouble7,
                    vdefDouble8 = reclist[i].vdefDouble8,
                    vdefDouble9 = reclist[i].vdefDouble9,
                    vdefString1 = reclist[i].vdefString1,
                    vdefString10= reclist[i].vdefString10,
                    vdefString2 = reclist[i].vdefString2,
                    vdefString3 = reclist[i].vdefString3,
                    vdefString4 = reclist[i].vdefString4,
                    vdefString5 = reclist[i].vdefString5,
                    vdefString6 = reclist[i].vdefString6,
                    vdefString7 = reclist[i].vdefString7,
                    vdefString8 = reclist[i].vdefString8,
                    vdefString9 = reclist[i].vdefString9                                          
                };
                db.ReturnApply_Rec.InsertOnSubmit(rec);
                db.SubmitChanges();

                if (reclist[i].returnResult == "E")
                {
                    returnapply_T_Item updateitem = db.returnapply_T_Item.SingleOrDefault(up => up.ID == Convert.ToInt32(reclist[i].userSysBillBid));
                    updateitem.State = 0;
                }
                db.SubmitChanges(); 
            }
           
        }

	    protected void RadButton_ExportExcel_Click(object sender, EventArgs e)
        {
            RadGrid1.ExportSettings.FileName = "物流中心出库单列表" + DateTime.Now.ToString("yyyy-MM-dd");
            RadGrid1.MasterTableView.ExportToExcel();
        }

        protected void RadButton_ExportWord_Click(object sender, EventArgs e)
        {
            RadGrid1.ExportSettings.FileName = "物流中心出库单列表" + DateTime.Now.ToString("yyyy-MM-dd");
            RadGrid1.MasterTableView.ExportToWord();
        }

        protected void RadButton_ExportPdf_Click(object sender, EventArgs e)
        {
            RadGrid1.ExportSettings.FileName = "物流中心出库单列表" + DateTime.Now.ToString("yyyy-MM-dd");
            RadGrid1.ExportSettings.IgnorePaging = true;
            RadGrid1.MasterTableView.ExportToPdf();
            RadGrid1.ExportSettings.IgnorePaging = false;
        }
    }
}