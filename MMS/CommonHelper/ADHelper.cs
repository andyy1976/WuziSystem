using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Web;
using mms.Models;

namespace mms.CommonHelper
{
    /// <summary>
    /// 域用户和用户组获取
    /// </summary>
    public class ADHelper
    {
        private const string SamAccountName = "samaccountname";
        private const string Name = "name";
        private const string User = "user";
        private const string Path = "Path";
        private const string OrganizationalUnit = "organizationalUnit";
        private const string DisplayName = "displayname";

        public string ServerName { get; set; }
        public string Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public ADHelper(string servername, string port, string username, string password)
        {
            this.ServerName = servername;
            this.Password = password;
            this.UserName = username;
            this.Port = port;
        }

        #region 获得组织结构树
        /// <summary>
        /// 获得组织结构树
        /// </summary>
        /// <param name="adOUList"></param>
        /// <returns></returns>
        public static IEnumerable<TreeNodeModel> GetOrgUnitTree(IEnumerable<string[]> adOUList)
        {
            List<TreeNodeModel> rootNodes = new List<TreeNodeModel>();
            foreach (string[] s in adOUList)
            {
                int count = s.Length;
                bool isRootInList = false;
                TreeNodeModel rootNode = new TreeNodeModel(s[count - 1]);
                foreach (TreeNodeModel node in rootNodes)
                {
                    if (node.Text == s[count - 1])
                    {
                        isRootInList = true;
                        rootNode = node;
                    }
                }
                if (!isRootInList)
                {
                    rootNodes.Add(rootNode);
                }

                BuildOrgUnitTree(rootNode, s, count - 1);
            }

            return rootNodes;
        }

        /// <summary>
        /// 更具路径添加树节点
        /// </summary>
        /// <param name="parentNode"></param>
        /// <param name="content"></param>
        /// <param name="index"></param>
        private static void BuildOrgUnitTree(TreeNodeModel parentNode, IList<string> content, int index)
        {
            if (index < 0)
            {
                return;
            }
            if (parentNode.Text == content[index])
            {
                index = index - 1;
                //如果找到节点了，那么继续找下一个内容
                if (index >= 0 && IsContainText(parentNode.ChildNodes, content[index], ref parentNode))
                {
                    BuildOrgUnitTree(parentNode, content, index);
                }
                else
                {
                    AddTreeNode(index, parentNode, content);
                }
            }
            else
            {
                index = index - 1;
                AddTreeNode(index, parentNode, content);
            }
        }

        /// <summary>
        /// 追加树节点
        /// </summary>
        /// <param name="index"></param>
        /// <param name="treeNode"></param>
        /// <param name="content"></param>
        private static void AddTreeNode(int index, TreeNodeModel treeNode, IList<string> content)
        {
            while (index >= 0)
            {
                TreeNodeModel node = new TreeNodeModel(content[index]);
                treeNode.ChildNodes.Add(node);
                treeNode = node;
                index = index - 1;
            }
        }

        /// <summary>
        /// 判断几点是否在节点列表中存在
        /// </summary>
        /// <param name="treeNodeCollection"></param>
        /// <param name="content"></param>
        /// <param name="outPutTreeNode"></param>
        /// <returns></returns>
        private static bool IsContainText(IEnumerable<TreeNodeModel> treeNodeCollection, string content, ref TreeNodeModel outPutTreeNode)
        {
            foreach (TreeNodeModel treenode in treeNodeCollection)
            {
                if (treenode.Text == content)
                {
                    outPutTreeNode = treenode;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 获得组织结构列表
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string[]> GetOrganizationalUnit()
        {
            SearchResultCollection results = _ADHelper(OrganizationalUnit);
            IEnumerable<string[]> sRe = GetGetOrganizationalUnitResultList(results);

            results.Dispose();

            return sRe;
        }

        private static IEnumerable<string[]> GetGetOrganizationalUnitResultList(SearchResultCollection results)
        {
            List<string[]> orgUnitList = new List<string[]>();
            if (results.Count == 0)
                throw new Exception("域中没有任何组织结构");
            else
            {
                foreach (SearchResult result in results)
                {
                    string adPath = result.Path;
                    if (adPath.IndexOf("OU=") < 0)
                        continue;

                    string[] ous = GetOUs(adPath);
                    orgUnitList.Add(ous);
                }
            }
            return orgUnitList;
        }
        #endregion

        #region 获得用户信息
        public static void AttachUsertoOrgUnit(IEnumerable<TreeNodeModel> treeNodes, IEnumerable<ADUserInfo> userInfos)
        {
            foreach (var adUserInfo in userInfos)
            {
                TreeNodeModel orgNode = FindNodeByPath(treeNodes, adUserInfo.ADPath);
                orgNode.Users.Add(adUserInfo);
            }
        }

        private static TreeNodeModel FindNodeByPath(IEnumerable<TreeNodeModel> treeNodes, IList<string> path)
        {
            int count = path.Count;
            TreeNodeModel rootNode = null;
            //找到根节点
            foreach (TreeNodeModel treeNode in treeNodes)
            {
                if (treeNode.Text == path[count - 1])
                {
                    rootNode = treeNode;
                }
            }

            if (rootNode == null)
            {
                return null;
            }

            if (count == 1)
            {
                return rootNode;
            }
            else
            {
                TreeNodeModel tempNode = rootNode;
                for (int i = 0; i < count - 1; i++)
                {
                    string content = path[count - 2 - i];
                    foreach (TreeNodeModel node in tempNode.ChildNodes)
                    {
                        if (node.Text == content)
                        {
                            tempNode = node;
                            break;
                        }
                    }
                }

                return tempNode;
            }
        }

        /// <summary>
        /// 获得域用户列表
        /// </summary>
        /// <returns></returns>
        public List<ADUserInfo> GetUserUnit()
        {
            SearchResultCollection results = _ADHelper(User);
            List<ADUserInfo> sRe = GetGetUserUnitResults(results);

            results.Dispose();

            return sRe;
        }

        private static List<ADUserInfo> GetGetUserUnitResults(SearchResultCollection results)
        {
            List<ADUserInfo> adUserInfos = new List<ADUserInfo>();
            if (results.Count == 0)
            {
                return null;
            }
            else
            {
                foreach (SearchResult result in results)
                {
                    string adPath = result.Path;
                    if (adPath.IndexOf("OU=") < 0)
                        continue;
                    //“组织名,登录名,用户名,邮件地址”
                    // 分解Path得到组织名

                    ADUserInfo adUserInfo = new ADUserInfo();
                    adUserInfo.ADPath = GetOUs(adPath);

                    ResultPropertyCollection propColl = result.Properties;
                    if (propColl[SamAccountName].Count > 0)
                        adUserInfo.AccountName = propColl[SamAccountName][0].ToString();
                    if (propColl[Name].Count > 0)
                        adUserInfo.UserName = propColl[Name][0].ToString();

                    adUserInfos.Add(adUserInfo);
                }

            }
            return adUserInfos;
        }

        #endregion

        /// <summary>
        /// 获得连接
        /// </summary>
        /// <returns></returns>
        private DirectoryEntry GetDirectoryEntry()
        {
            if (Port != "")
            {
                return new DirectoryEntry("LDAP://" + ServerName + ":" + Port, UserName, Password);
            }
            else
            {
                return new DirectoryEntry("LDAP://" + ServerName, UserName, Password);
            }
        }

        /// <summary>
        /// 设置参数
        /// </summary>
        /// <param name="searcher"></param>
        /// <param name="fiterStr"></param>
        private void SetParameter(DirectorySearcher searcher, string fiterStr)
        {
            searcher.SearchScope = SearchScope.Subtree;
            searcher.PropertiesToLoad.AddRange(new[] { Name, Path, DisplayName, SamAccountName });
            searcher.Filter = "(objectclass=" + fiterStr + ")";
            searcher.PageSize = 1000;
        }

        private SearchResultCollection _ADHelper(string schemaClassNameToSearch)
        {
            DirectoryEntry searchRoot = GetDirectoryEntry();
            DirectorySearcher directorySearcher = new DirectorySearcher(searchRoot);
            SetParameter(directorySearcher, schemaClassNameToSearch);

            SearchResultCollection results = directorySearcher.FindAll();
            return results;
        }

        private static string[] GetOUs(string pathStr)
        {
            string[] sSplit = pathStr.Split(',');
            List<string> ouList = new List<string>();
            foreach (var s in sSplit)
            {
                if (s.IndexOf("OU=") < 0)
                    continue;

                ouList.Add(s.Split('=')[1]);
            }
            string[] ous = ouList.ToArray();

            return ous;
        }

        /// <summary>
        /// 把路径转为反串行化
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string[] TranslatePath(string path)
        {
            return path.Split(',');
        }

        /// <summary>
        /// 把路径串行化
        /// </summary>
        /// <param name="pathStrs"></param>
        /// <returns></returns>
        public static string TranslateToPath(string[] pathStrs)
        {
            string path = "";
            foreach (var pathStr in pathStrs)
            {
                path += pathStr + ",";
            }

            path = path.TrimEnd(',');

            return path;
        }
    }
}