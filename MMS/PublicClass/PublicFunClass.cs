using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mms.PublicClass
{
    public class PublicFunClass 
    {
        public static bool ValidIsNotDecimal(string str)
        {
            var flag = true;
            try
            {
                str = Convert.ToDecimal(str).ToString();
                if (string.IsNullOrEmpty(str))
                    flag = false;
            }
            catch
            {
                flag = false;
            }
            return flag;
        }
    }
}