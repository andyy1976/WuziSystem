using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mms.Models
{
    /// <summary>
    /// 角色
    /// </summary>
    [Serializable]
    public class RoleModel
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        public int RoleID { get; set; }

        /// <summary>
        /// 角色名
        /// </summary>
        public string RoleName { get; set; }

        /// <summary>
        /// 角色值，是功能ID用逗号分隔的字符串
        /// </summary>
        public string RoleValue { get; set; }

        /// <summary>
        /// 角色对应的用户列表
        /// </summary>
        public List<UserModel> Users { get; set; }
    }
}