namespace Camc.Web.Library
{
    using System;
    using System.Data;
    using System.Data.SqlClient;

    internal class SQLServer : DBInterface
    {
        private bool inTrans;
        private SqlConnection SQLConnection;
        private SqlTransaction SQLTrans;

        public SQLServer(string DBConnectionString)
        {
            try
            {
                this.SQLConnection = new SqlConnection(DBConnectionString);
            }
            catch (Exception exception)
            {
                throw new Exception("SQLServer类构造函数异常：" + exception.Message);
            }
        }

        public override void BeginTrans()
        {
            try
            {
                if (this.SQLConnection.State.ToString().ToUpper() != "OPEN")
                {
                    throw new Exception("数据库连接未打开");
                }
                this.SQLTrans = this.SQLConnection.BeginTransaction();
                this.inTrans = true;
            }
            catch (Exception exception)
            {
                throw new Exception("BeginTrans方法异常：" + exception.Message);
            }
        }

        public override bool CheckExistData(string SQL)
        {
            bool flag;
            try
            {
                if ((this.SQLConnection.State.ToString().ToUpper() != "OPEN") && !this.inTrans)
                {
                    this.OpenConnection();
                }
                SqlCommand command = new SqlCommand {
                    Connection = this.SQLConnection
                };
                if (this.inTrans)
                {
                    command.Transaction = this.SQLTrans;
                }
                DataSet dataSet = new DataSet();
                SqlDataAdapter adapter = new SqlDataAdapter();
                command.CommandText = SQL;
                adapter.SelectCommand = command;
                adapter.Fill(dataSet);
                if ((this.SQLConnection.State.ToString().ToUpper() == "OPEN") && !this.inTrans)
                {
                    this.CloseConnection();
                }
                if (dataSet.Tables[0].Rows.Count > 0)
                {
                    return true;
                }
                flag = false;
            }
            catch (Exception exception)
            {
                throw new Exception("CheckExistData方法异常：" + exception.Message);
            }
            return flag;
        }

        public override void CloseConnection()
        {
            try
            {
                if (this.SQLConnection.State.ToString().ToUpper() == "OPEN")
                {
                    this.SQLConnection.Close();
                }
            }
            catch (Exception exception)
            {
                throw new Exception("关闭数据库连接失败：" + exception.Message);
            }
        }

        public override void CommitTrans()
        {
            try
            {
                if (!this.inTrans)
                {
                    throw new Exception("数据库事务还未开始");
                }
                this.SQLTrans.Commit();
                this.inTrans = false;
            }
            catch (Exception exception)
            {
                throw new Exception("CommitTrans方法异常：" + exception.Message);
            }
        }

        public override bool Execute(string SQL)
        {
            bool flag;
            try
            {
                if ((this.SQLConnection.State.ToString().ToUpper() != "OPEN") && !this.inTrans)
                {
                    this.OpenConnection();
                }
                SqlCommand command = new SqlCommand {
                    Connection = this.SQLConnection
                };
                if (this.inTrans)
                {
                    command.Transaction = this.SQLTrans;
                }
                command.CommandText = SQL;
                command.ExecuteNonQuery();
                if ((this.SQLConnection.State.ToString().ToUpper() == "OPEN") && !this.inTrans)
                {
                    this.CloseConnection();
                }
                flag = true;
            }
            catch (Exception exception)
            {
                throw new Exception("Execute方法异常：" + exception.Message);
            }
            return flag;
        }

        public override DataTable Execute(string SQL, bool ReturnDataTable)
        {
            DataTable table;
            try
            {
                if ((this.SQLConnection.State.ToString().ToUpper() != "OPEN") && !this.inTrans)
                {
                    this.OpenConnection();
                }
                SqlCommand command = new SqlCommand {
                    Connection = this.SQLConnection
                };
                if (this.inTrans)
                {
                    command.Transaction = this.SQLTrans;
                }
                DataSet dataSet = new DataSet();
                SqlDataAdapter adapter = new SqlDataAdapter();
                command.CommandText = SQL;
                adapter.SelectCommand = command;
                adapter.Fill(dataSet);
                if ((this.SQLConnection.State.ToString().ToUpper() == "OPEN") && !this.inTrans)
                {
                    this.CloseConnection();
                }
                table = dataSet.Tables[0];
            }
            catch (Exception exception)
            {
                throw new Exception("Execute方法异常：" + exception.Message);
            }
            return table;
        }

        public override DataSet ExecuteDs(string SQL, bool ReturnDataSet)
        {
            DataSet set2;
            try
            {
                if ((this.SQLConnection.State.ToString().ToUpper() != "OPEN") && !this.inTrans)
                {
                    this.OpenConnection();
                }
                SqlCommand command = new SqlCommand {
                    Connection = this.SQLConnection
                };
                if (this.inTrans)
                {
                    command.Transaction = this.SQLTrans;
                }
                DataSet dataSet = new DataSet();
                SqlDataAdapter adapter = new SqlDataAdapter();
                command.CommandText = SQL;
                adapter.SelectCommand = command;
                adapter.Fill(dataSet);
                if ((this.SQLConnection.State.ToString().ToUpper() == "OPEN") && !this.inTrans)
                {
                    this.CloseConnection();
                }
                set2 = dataSet;
            }
            catch (Exception exception)
            {
                throw new Exception("Execute方法异常：" + exception.Message);
            }
            return set2;
        }

        public override string GetSingleValue(string SQL)
        {
            string str2;
            try
            {
                if ((this.SQLConnection.State.ToString().ToUpper() != "OPEN") && !this.inTrans)
                {
                    this.OpenConnection();
                }
                SqlCommand command = new SqlCommand {
                    Connection = this.SQLConnection
                };
                if (this.inTrans)
                {
                    command.Transaction = this.SQLTrans;
                }
                DataSet dataSet = new DataSet();
                SqlDataAdapter adapter = new SqlDataAdapter();
                command.CommandText = SQL;

       
                adapter.SelectCommand = command;
               
                adapter.Fill(dataSet);
                if ((this.SQLConnection.State.ToString().ToUpper() == "OPEN") && !this.inTrans)
                {
                    this.CloseConnection();
                }
                string str = string.Empty;
                dataSet.CaseSensitive = false;
              
                if (dataSet!=null &&dataSet.Tables.Count > 0)
                {
                    if (dataSet.Tables[0].Rows.Count > 0)
                    {
                        str = dataSet.Tables[0].Rows[0][0].ToString();
                    }
                }
               
                if ((str != null) && (str.Length > 0))
                {
                    return str;
                }
                str2 = null;
            }
            catch (Exception exception)
            {
                throw new Exception("GetSingleValue方法异常：" + exception.Message);
            }
            return str2;
        }

        public override void OpenConnection()
        {
            try
            {
                if (this.SQLConnection.State.ToString().ToUpper() != "OPEN")
                {
                    this.SQLConnection.Open();
                }
            }
            catch (Exception exception)
            {
                throw new Exception("打开数据库连接失败：" + exception.Message);
            }
        }

        public override void RollbackTrans()
        {
            try
            {
                if (!this.inTrans)
                {
                    throw new Exception("数据库事务还未开始");
                }
                this.SQLTrans.Rollback();
                this.inTrans = false;
            }
            catch (Exception exception)
            {
                throw new Exception("RollbackTrans方法异常：" + exception.Message);
            }
        }

        public override IDbConnection Connection
        {
            get
            {
                if (this.SQLConnection != null)
                {
                    return this.SQLConnection;
                }
                return null;
            }
        }
    }
}

