using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mms.Models
{
    /// <summary>
    /// 用户
    /// </summary>
    [Serializable]
    public class UserModel
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 域
        /// </summary>
        public string Domain { get; set; }

        /// <summary>
        /// 显示名
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 用户在域中的路径
        /// </summary>
        public string AdPath { get; set; }

        /// <summary>
        ///密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 用户是否锁住
        /// </summary>
        public bool IsLocked { get; set; }

        /// <summary>
        /// 创建用户日期
        /// </summary>
        public DateTime? CreateDate { get; set; }

        /// <summary>
        /// 用户对应的角色
        /// </summary>
        public List<RoleModel> Roles { get; set; }

        /// <summary>
        /// 用户对应的角色的字符串
        /// </summary>
        public string RolesString
        {
            get
            {
                return Roles.Aggregate(string.Empty, (current, roleModel) => current + (roleModel.RoleName + ";"));
            }
        }

        /// <summary>
        /// 是否被锁定的字符串
        /// </summary>
        public string IsLockedString
        {
            get
            {
                return IsLocked ? "是" : "否";
            }
        }
    }
}