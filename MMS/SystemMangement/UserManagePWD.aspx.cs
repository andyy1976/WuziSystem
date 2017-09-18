using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Data;
using System.Collections;
using Camc.Web.Library;
using System.Configuration;
using System.Web.Security;

namespace mms.SystemMangement
{
    public partial class UserManagePWD : System.Web.UI.Page
    {
        //初始化Grid数据源
        private DataTable GridSource
        {
            get
            {
                Object obj = this.ViewState["_gds"];
                if (obj != null)
                {
                    return (DataTable)obj;
                }
                else
                {
                    DataTable InitTable = new DataTable();
                    InitTable.Columns.Add("RowsId");
                    InitTable.Columns.Add("ID");
                    InitTable.Columns.Add("UserAccount");
                    InitTable.Columns.Add("DomainAccount");
                    InitTable.Columns.Add("UserName");
                    InitTable.Columns.Add("Dept");
                    InitTable.Columns.Add("IsDel");
                    InitTable.PrimaryKey = new DataColumn[] { InitTable.Columns["ID"] };       //设置RowsId列为主键，用于datatable删除
                    this.ViewState["_gds"] = InitTable;
                    return InitTable;
                }
            }
            set
            {
                this.ViewState["_gds"] = value;
                ((DataTable)this.ViewState["_gds"]).PrimaryKey = new DataColumn[] { ((DataTable)this.ViewState["_gds"]).Columns["ID"] };
            }
        }

        private static string DBConn;
        private DBInterface DBI;
        protected void Page_Load(object sender, EventArgs e)
        {
            DBConn = ConfigurationManager.ConnectionStrings["MaterialManagerSystemConnectionString"].ToString();
            DBI = DBFactory.GetDBInterface(DBConn);
            if (Session["UserName"] == null) { Response.Redirect("/Default.aspx"); }          
            if (!IsPostBack)
            {
                Common.CheckPermission(Session["UserName"].ToString(), "UserManagePWD", this.Page);
                GridSource = Common.AddTableRowsID(GetUserList(""));

                this.ViewState["lastSelectItem"] = "";

                //string strSql = " select * from Sys_UserInfo_PWD";
                string strSql = " select ID,(UserName + '  ' +DomainAccount) as name from Sys_UserInfo_PWD";
                DataTable dt = DBI.Execute(strSql, true);

                RCB_Where.DataSource = dt;
                RCB_Where.DataValueField = "ID";
                RCB_Where.DataTextField = "name";
                RCB_Where.DataBind();
            }
        }
        protected DataTable GetUserList(string strWhere)
        {
            string strSQL;
            strSQL = " select Sys_UserInfo_PWD.* from Sys_UserInfo_PWD left join Sys_DeptEnum on Convert(nvarchar(50),Sys_DeptEnum.ID) = Sys_UserInfo_PWD.Dept where 1=1 "
                + strWhere + " order by IsDel, Sys_DeptEnum.Dept, UserName";
            return DBI.Execute(strSQL, true);
        }

        protected void RadGrid_UserManage_PWD_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            RadGrid_UserManage_PWD.DataSource = GridSource;
        }

        protected void RadGrid_UserManage_PWD_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (!Common.IsHasRight(Session["UserName"].ToString(), "RadButton_AddRole"))
            {
                if (e.Item is GridCommandItem)
                {
                    e.Item.FindControl("RadButton_AddRole").Visible = false;
                }
            }
            if (e.Item is GridDataItem)
            {
                GridDataItem item = e.Item as GridDataItem;
                string id = item.GetDataKeyValue("ID").ToString();
                string strSQL = "declare @sql nvarchar(max) select @sql = '' select @sql = @sql + RoleName + '、' from Sys_UserInRole join Sys_RoleInfo on Sys_RoleInfo.ID = Sys_UserInRole.RoleID"
                    + " where UserID = '" + id + "'  select case when @sql = '' then '' else SUBSTRING(@sql,1,LEN(@sql) -1) end";
                string Roles = DBI.GetSingleValue(strSQL);
                item["RoleName"].Text = Roles;
            }
        }

        protected void RadGrid_UserManage_PWD_ItemCommand(object sender, GridCommandEventArgs e)
        {
            GridEditableItem item = e.Item as GridEditableItem;
            UserControl userControl = (UserControl)e.Item.FindControl(GridEditFormItem.EditFormUserControlID);

            RadTextBox RTB_UserAccount = userControl == null ? new RadTextBox() : userControl.FindControl("RTB_UserAccount") as RadTextBox;
            RadTextBox RTB_DomainAccount = userControl == null ? new RadTextBox() : userControl.FindControl("RTB_DomainAccount") as RadTextBox;
            RadTextBox RTB_UserName = userControl == null ? new RadTextBox() : userControl.FindControl("RTB_UserName") as RadTextBox;
            RadTextBox RTB_PassWord = userControl == null ? new RadTextBox() : userControl.FindControl("RTB_PassWord") as RadTextBox;
            RadButton RB_PWD1 = userControl == null ? new RadButton() : userControl.FindControl("RB_PWD1") as RadButton;
            RadButton RB_PWD2 = userControl == null ? new RadButton() : userControl.FindControl("RB_PWD2") as RadButton;
            RadComboBox RCB_Dept = userControl == null ? new RadComboBox() : userControl.FindControl("RCB_Dept") as RadComboBox;
            CheckBoxList CBL_Role = userControl == null ? new CheckBoxList() : userControl.FindControl("CBL_Role") as CheckBoxList;
            RadTextBox RTB_Phone = userControl == null ? new RadTextBox() : userControl.FindControl("RTB_Phone") as RadTextBox;

            string strSQL = "";

            if (e.CommandName == "PerformInsert")
            {
                string UserAccount = RTB_UserAccount.Text.Trim();
                string DomainAccount = RTB_DomainAccount.Text.Trim();
                string UserName = RTB_UserName.Text.Trim();
                string PassWord = RTB_PassWord.Text.Trim();
                string Dept = RCB_Dept.SelectedValue.ToString();
                string MD5PassWord = FormsAuthentication.HashPasswordForStoringInConfigFile(PassWord, "md5");
                string Phone = RTB_Phone.Text.Trim();

                if (UserAccount == "")
                {
                    RadNotificationAlert.Text = "失败！请输入账户名称";
                    RadNotificationAlert.Show();

                    e.Canceled = true;
                    return;
                }
                if (UserName == "")
                {
                    RadNotificationAlert.Text = "失败！请输入用户名称";
                    RadNotificationAlert.Show();

                    e.Canceled = true;
                    return;
                }

                DBI.OpenConnection();
                try
                {
                    DBI.BeginTrans();
                    strSQL = " if (select count(*) from Sys_UserInfo_PWD where UserAccount = '" + UserAccount + "') = 0 begin";
                    strSQL += " if (select count(*) from Sys_UserInfo_PWD where DomainAccount = '" +
                              DomainAccount + "' and '" + DomainAccount + "' != '') = 0 begin";
                    strSQL += " declare @id int insert into Sys_UserInfo_PWD (UserAccount,DomainAccount, PassWord, UserName, Dept, IsDel, Phone)";
                    strSQL += " values ('" + UserAccount + "','" + DomainAccount +
                              "','" + MD5PassWord + "','" + UserName + "','" + Dept + "','false', '" + Phone + "') select @@identity";
                    strSQL += " end else begin select '-1' end end else begin select '0' end";
                    string UserId = DBI.GetSingleValue(strSQL).ToString();
                    if (UserId == "0")
                    {
                        RadNotificationAlert.Text = "失败！账户名称已经存在，请更换另一个";
                        RadNotificationAlert.Show();

                        e.Canceled = true;
                    }
                    else if (UserId == "-1")
                    {
                        RadNotificationAlert.Text = "失败！域帐号已经存在，请更换另一个";
                        RadNotificationAlert.Show();

                        e.Canceled = true;
                    }
                    else
                    {
                        for (int i = 0; i < CBL_Role.Items.Count; i++)
                        {
                            if (CBL_Role.Items[i].Selected == true)
                            {
                                strSQL = " Insert into Sys_UserInRole (UserId,RoleID) values('" + UserId + "','" +
                                         CBL_Role.Items[i].Value.ToString() + "')";
                                DBI.Execute(strSQL);
                            }
                        }
                        RadNotificationAlert.Text = "添加成功！";
                        RadNotificationAlert.Show();

                        GridSource = Common.AddTableRowsID(GetUserList(""));
                    }

                    DBI.CommitTrans();
                }
                catch (Exception ex)
                {
                    DBI.RollbackTrans();
                    RadNotificationAlert.Text = "失败！" + ex.Message.ToString();
                    RadNotificationAlert.Show();
                }
                finally
                {
                    DBI.CloseConnection();
                }
            }
            if (e.CommandName == "Update")
            {
                string id = item.GetDataKeyValue("ID").ToString();
                string UserAccount = RTB_UserAccount.Text.Trim();
                string DomainAccount = RTB_DomainAccount.Text.Trim();
                string UserName = RTB_UserName.Text.Trim();
                string PassWord = RTB_PassWord.Text.Trim();
                string Dept = RCB_Dept.SelectedValue.ToString();
                string MD5PassWord = FormsAuthentication.HashPasswordForStoringInConfigFile(PassWord, "md5");
                string Phone = RTB_Phone.Text.Trim();

                if (UserAccount == "")
                {
                    RadNotificationAlert.Text = "失败！请输入账户名称";
                    RadNotificationAlert.Show();
                    e.Canceled = true;
                    return;
                }
                if (UserName == "")
                {
                    RadNotificationAlert.Text = "失败！请输入用户名称";
                    RadNotificationAlert.Show();
                    e.Canceled = true;
                    return;
                }

                DBI.OpenConnection();
                try
                {
                    DBI.BeginTrans();
                    strSQL = " if (select count(*) from Sys_UserInfo_PWD where UserAccount = '" + UserAccount + "' and ID <> '" + id + "') = 0 begin";
                    strSQL += " if (select count(*) from Sys_UserInfo_PWD where DomainAccount = '" + DomainAccount +
                              "' and ID <> '" + id + "' and '" + DomainAccount + "' != '') = 0 begin";
                    strSQL += " Update Sys_UserInfo_PWD set UserAccount = '" + UserAccount + "', DomainAccount = '" +
                              DomainAccount + "', UserName = '" + UserName + "', Dept = '" + Dept + "', Phone = '" + Phone + "'";
                    if (RB_PWD2.Checked == true)
                    {
                        strSQL += " , PassWord = '" + MD5PassWord + "'";
                    }
                    strSQL += " where ID = '" + id + "' select '" + id + "'";
                    strSQL += " end else begin select '-1' end end else begin select '0' end";
                    string UserId = DBI.GetSingleValue(strSQL).ToString();
                    if (UserId == "0")
                    {
                        RadNotificationAlert.Text = "失败！账户名称已经存在，请更换另一个";
                        RadNotificationAlert.Show();
                        e.Canceled = true;
                    }
                    else if (UserId == "-1")
                    {
                        RadNotificationAlert.Text = "失败！域帐号已经存在，请更换另一个";
                        RadNotificationAlert.Show();
                        e.Canceled = true;
                    }
                    else
                    {
                        strSQL = " delete Sys_UserInRole where UserId = '" + UserId + "'";
                        DBI.Execute(strSQL);
                        for (int i = 0; i < CBL_Role.Items.Count; i++)
                        {
                            if (CBL_Role.Items[i].Selected == true)
                            {
                                strSQL = " Insert into Sys_UserInRole (UserId,RoleID) values('" + UserId + "','" +
                                         CBL_Role.Items[i].Value.ToString() + "')";
                                DBI.Execute(strSQL);
                            }
                        }

                        GridSource = Common.AddTableRowsID(GetUserList(""));

                        RadNotificationAlert.Text = "修改成功！";
                        RadNotificationAlert.Show();
                    }

                    DBI.CommitTrans();
                }
                catch (Exception ex)
                {
                    DBI.RollbackTrans();
                    RadNotificationAlert.Text = "失败！" + ex.Message.ToString();
                    RadNotificationAlert.Show();
                }
                finally
                {
                    DBI.CloseConnection();
                }
            }
            if (e.CommandName == "Delete")
            {
                if (RadGrid_UserManage_PWD.SelectedItems.Count > 0)
                {
                    string id = (RadGrid_UserManage_PWD.SelectedItems[0] as GridDataItem).GetDataKeyValue("ID").ToString();
                    strSQL = " Update Sys_UserInfo_PWD set IsDel = 'true' where Id = '" + id + "'";
                    DBI.Execute(strSQL);

                    GridSource = Common.AddTableRowsID(GetUserList(""));
                    RadNotificationAlert.Text = "删除成功！";
                    RadNotificationAlert.Show();
                }
                else
                {
                    RadNotificationAlert.Text = "失败！请选择要删除的行";
                    RadNotificationAlert.Show();
                }
            }
        }

        protected DataTable GetOneUPWD(string ID)
        {
            string strSQL;
            try
            {
                //strSQL = "select a.ID, a.UserAccount, a.UserName, a.Dept, c.RoleName from Sys_UserInfo_PWD as a " +
                //         "left join Sys_UserInRole as b on a.ID=b.UserID " +
                //         "left join Sys_RoleInfo as c on b.RoleID=c.ID Where ID like '%" + ID + "%'";
                //return DBI.Execute(strSQL, true);

                strSQL = " select * from Sys_UserInfo_PWD"
                    + " where ID = '" + ID + "'";
                return DBI.Execute(strSQL, true);
            }
            catch (Exception e)
            {
                throw new Exception("获取人员列表时出现异常" + e.Message.ToString());
            }
        }

        protected DataTable GetUserListByUserName(string userName)
        {
            string strSQL = " select Sys_UserInfo_PWD.* from  Sys_UserInfo_PWD left join Sys_DeptEnum on Convert(nvarchar(50),Sys_DeptEnum.ID) = Sys_UserInfo_PWD.Dept"
                    + " where UserName like '%" + userName + "' order by IsDel, Sys_DeptEnum.Dept, UserName";
            return DBI.Execute(strSQL, true);
        }

        protected void RadComboBoxUserPWD_ItemDataBound(object sender, RadComboBoxItemEventArgs e)
        {
            e.Item.Text = string.Concat(e.Item.Text.ToLower().Split(' ')[0], "");
        }

        protected void RadComboBoxUserPWD_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            string selectName = e.Value.ToString();
            if (selectName == "0")
            {
                this.ViewState["lastSelectItem"] = selectName;
                GridSource = Common.AddTableRowsID(GetUserList(""));
                this.RadGrid_UserManage_PWD.Rebind();
            }
            else
            {
                this.ViewState["lastSelectItem"] = selectName;
                GridSource = Common.AddTableRowsID(GetOneUPWD(selectName));
                this.RadGrid_UserManage_PWD.Rebind();
            }
            
        }

        protected void RTB_UserName_TextChanged(object sender, EventArgs e)
        {
            string userName = (sender as RadTextBox).Text;
            GridSource = Common.AddTableRowsID(GetUserListByUserName(userName));
            this.RadGrid_UserManage_PWD.Rebind();
        }

        protected void RadAjaxManager1_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            if (e.Argument == "Rebind")
            {
                GridSource = Common.AddTableRowsID(GetUserList(""));
                RadGrid_UserManage_PWD.Rebind();
            }
            else
            {
                throw new Exception("刷新页面出错，请按F5刷新！");
            }
        }

        protected void RCB_Where_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            string where = RCB_Where.Text;
            string Id = RCB_Where.SelectedValue;

            string strWhere = "";
            if (Id == "")
            {
                strWhere = " and (UserName like '%" + where + "%' or UserAccount like '%" + where + "' or DomainAccount like '%" + where + "%')";
            }
            else
            {
                strWhere = " and Sys_UserInfo_PWD.ID = '" + Id + "'";
            }
            GridSource = Common.AddTableRowsID(GetUserList(strWhere));
            RadGrid_UserManage_PWD.Rebind();
        }
		
		 protected void RadButton_ExportExcel_Click(object sender, EventArgs e)
        {
            RadGrid_UserManage_PWD.ExportSettings.FileName = "部门信息列表" + DateTime.Now.ToString("yyyy-MM-dd");
            RadGrid_UserManage_PWD.MasterTableView.ExportToExcel();
        }

        protected void RadButton_ExportWord_Click(object sender, EventArgs e)
        {
            RadGrid_UserManage_PWD.ExportSettings.FileName = "部门信息列表" + DateTime.Now.ToString("yyyy-MM-dd");
            RadGrid_UserManage_PWD.MasterTableView.ExportToWord();
        }

        protected void RadButton_ExportPdf_Click(object sender, EventArgs e)
        {
            RadGrid_UserManage_PWD.ExportSettings.FileName = "部门信息列表" + DateTime.Now.ToString("yyyy-MM-dd");
            RadGrid_UserManage_PWD.ExportSettings.IgnorePaging = true;
            RadGrid_UserManage_PWD.MasterTableView.ExportToPdf();
            RadGrid_UserManage_PWD.ExportSettings.IgnorePaging = false;
        }
    }
}