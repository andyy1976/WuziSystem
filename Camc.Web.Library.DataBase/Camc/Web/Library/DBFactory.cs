namespace Camc.Web.Library
{
    using System;
    using System.Configuration;

    public class DBFactory
    {
        public static DBInterface GetDBInterface()
        {
            DBInterface interface2;
            try
            {
                string dBConnectionString = ConfigurationSettings.AppSettings["DefaultDBConnectionString"];
                interface2 = new SQLServer(dBConnectionString);
            }
            catch (Exception exception)
            {
                throw new Exception("GetDBInterface方法异常：" + exception.Message);
            }
            return interface2;
        }

        public static DBInterface GetDBInterface(string DBConnectionString)
        {
            DBInterface interface2;
            try
            {
                interface2 = new SQLServer(DBConnectionString);
            }
            catch (Exception exception)
            {
                throw new Exception("GetDBInterface方法异常：" + exception.Message);
            }
            return interface2;
        }
    }
}

