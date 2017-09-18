using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using mms.Models;

namespace mms.Dal
{
    public class UserDataAccess
    {
        /// <summary>
        /// 创建用户
        /// </summary>
        /// <param name="userModel"></param>
        public static int CreateUser(UserModel userModel)
        {
            using (MMSDbDataContext db = new MMSDbDataContext())
            {
                Sys_UserInfo_PWD user = ConvertUserModelToUser(userModel);
                if (!string.IsNullOrEmpty(user.UserAccount))
                {
                    db.Sys_UserInfo_PWD.InsertOnSubmit(user);
                }

                db.SubmitChanges();

                return user.ID;
            }
        }

        /// <summary>
        /// 解锁用户
        /// </summary>
        /// <param name="userID"></param>
        public static void UnLockUser(int userID)
        {
            using (MMSDbDataContext db = new MMSDbDataContext())
            {
                Sys_UserInfo_PWD user = db.Sys_UserInfo_PWD.Single(t => t.ID == userID);
                //user.IsLockedOut = 0;
                db.SubmitChanges();
            }
        }

        /// <summary>
        /// 获得所有用户
        /// </summary>
        /// <returns></returns>
        public static List<UserModel> GetAllUsers()
        {
            List<UserModel> userModels = new List<UserModel>();
            using (MMSDbDataContext db = new MMSDbDataContext())
            {
                var users = from t in db.Sys_UserInfo_PWD select t;
                foreach (var user in users)
                {
                    UserModel userModel = ConvertUserToUserModel(user);
                    userModels.Add(userModel);
                }
            }

            return userModels;
        }

        /// <summary>
        /// 把用户数据库实体转为用户的业务模型
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static UserModel ConvertUserToUserModel(Sys_UserInfo_PWD user)
        {
            UserModel userModel = new UserModel();
            ConvertUserToUserModel(userModel, user);
            return userModel;
        }

        /// <summary>
        /// 把用户数据库实体转为用户的业务模型
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="user"></param>
        public static void ConvertUserToUserModel(UserModel userModel, Sys_UserInfo_PWD user)
        {
            userModel.UserID = user.ID;
            userModel.UserName = user.UserAccount;
            userModel.Password = user.PassWord;
            //userModel.CreateDate = user.CreationDate;
            //userModel.IsLocked = user.IsLockedOut == 0 ? false : true;
            userModel.DisplayName = user.UserName;
            //userModel.AdPath = user.AdPath;
            List<RoleModel> roleModels = new List<RoleModel>();
            //foreach (var userRole in user.UserRole)
            //{
            //    RoleModel roleModel = RoleDataAccess.ConvertRoleToRoleModel(userRole.Roles);
            //    roleModels.Add(roleModel);
            //}
            userModel.Roles = roleModels;
        }

        /// <summary>
        /// 把用户的业务模型转为用户数据库实体
        /// </summary>
        /// <param name="user"></param>
        /// <param name="userModel"></param>
        private static void ConvertUserModelToUser(Sys_UserInfo_PWD user, UserModel userModel)
        {
            Regex regex = new Regex("\\$$");
            Match match = regex.Match(userModel.UserName);
            if (match.Success)
            {
                return;
            }
            else
            {
                user.ID = userModel.UserID;
                user.UserAccount = userModel.UserName;
                user.PassWord =
                    FormsAuthentication.HashPasswordForStoringInConfigFile(
                        !string.IsNullOrEmpty(userModel.Password) ? userModel.Password : "123456", "md5");
                //user.CreationDate = userModel.CreateDate;
                //user.IsLockedOut = userModel.IsLocked ? 1 : 0;
                user.UserName = userModel.DisplayName;
                user.DomainAccount = userModel.UserName;
                //user.AdPath = userModel.AdPath;
            }
        }

        /// <summary>
        /// 把用户的业务模型转为用户数据库实体
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        private static Sys_UserInfo_PWD ConvertUserModelToUser(UserModel userModel)
        {
            Sys_UserInfo_PWD user = new Sys_UserInfo_PWD();
            ConvertUserModelToUser(user, userModel);
            return user;
        }
    }
}