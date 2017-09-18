namespace Camc.Web.Library
{
    using System;
    using System.Data;

    public abstract class DBInterface
    {
        protected DBInterface()
        {
        }

        public abstract void BeginTrans();
        public abstract bool CheckExistData(string SQL);
        public abstract void CloseConnection();
        public abstract void CommitTrans();
        public abstract bool Execute(string SQL);
        public abstract DataTable Execute(string SQL, bool ReturnDataTable);
        public abstract DataSet ExecuteDs(string SQL, bool ReturnDataSet);
        public abstract string GetSingleValue(string SQL);
        public abstract void OpenConnection();
        public abstract void RollbackTrans();

        public abstract IDbConnection Connection { get; }
    }
}

