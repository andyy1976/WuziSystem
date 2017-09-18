using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using mms.CommonHelper;
using mms.Dal;
using mms.Models;

namespace mms.BLL
{
    public class UserService
    {
        /// <summary>
        /// 同步域用户信息
        /// </summary>
        /// <returns></returns>
        public bool SynchUser(string servername, string port, string username, string password)
        {
            try
            {
                //获得域用户信息
                ADHelper adHelper = new ADHelper(servername, port, username, password);
                //IEnumerable<string[]> orgUnits = adHelper.GetOrganizationalUnit();
                IEnumerable<ADUserInfo> userInfos = adHelper.GetUserUnit();

                

                //把用户信息更新到User表中
                List<UserModel> userModels = GetAllUsers();

                List<int> adUserIds = new List<int>();

                foreach (ADUserInfo adUserInfo in userInfos)
                {
                    //遍历ADUserInfo列表,查看用户是否已经录入数据库，如果否执行添加
                    int userID = -1;
                    UserModel outUserModel;
                    string adpath = ADHelper.TranslateToPath(adUserInfo.ADPath);
                    if (!IsUserInDB(userModels, adUserInfo.AccountName, out outUserModel))
                    {
                        userID = UserDataAccess.CreateUser(new UserModel()
                        {
                            UserName = adUserInfo.AccountName,
                            DisplayName = adUserInfo.UserName,
                            AdPath = adpath,
                            CreateDate = DateTime.Now
                        });
                    }
                    else
                    {
                        userID = outUserModel.UserID;
                        if (outUserModel.AdPath != adpath)
                        {
                            //UserDataAccess.UpdateUserADPath(outUserModel.UserID, adpath);
                        }
                    }
                    adUserIds.Add(userID);
                }

                //查找数据库中存在，但域服务器中不存在的用户标志为锁定状态
                List<int> lockedUserIds = new List<int>();
                foreach (var userModel in userModels)
                {
                    if (!adUserIds.Contains(userModel.UserID))
                    {
                        lockedUserIds.Add(userModel.UserID);
                    }
                }
                //UserDataAccess.LockUser(lockedUserIds);

                return true;
            }
            catch
            {
                return false;
            }

        }
        /// <summary>
        /// 判断用户是否在数据库中
        /// </summary>
        /// <param name="userModels"></param>
        /// <param name="username"></param>
        /// <param name="outUserModel"></param>
        /// <returns></returns>
        private bool IsUserInDB(IEnumerable<UserModel> userModels, string username, out UserModel outUserModel)
        {
            outUserModel = new UserModel();
            foreach (var userModel in userModels)
            {
                if (userModel.UserName.Equals(username))
                {
                    outUserModel = userModel;
                    //如果用户被锁了，就解锁
                    if (userModel.IsLocked)
                    {
                        UserDataAccess.UnLockUser(outUserModel.UserID);
                    }
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 获得所有用户
        /// </summary>
        /// <returns></returns>
        public List<UserModel> GetAllUsers()
        {
            return UserDataAccess.GetAllUsers();
        }

        /// <summary>
        /// 获取所有当前存在的用户
        /// </summary>
        /// <returns></returns>
        public List<UserModel> GetExistUsers()
        {
            return GetAllUsers().Where(t => t.IsLocked == false).ToList();
        }
    }
}