using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mms.Models
{
    public class TreeNodeModel
    {
        public TreeNodeModel()
        {
            this.Text = "";
        }
        public TreeNodeModel(string text)
        {
            this.Text = text;
        }
        /// <summary>
        /// 子节点集合
        /// </summary>
        private List<TreeNodeModel> m_ChildNodes = new List<TreeNodeModel>();
        public List<TreeNodeModel> ChildNodes
        {
            get
            {
                return m_ChildNodes;
            }
            set
            {
                m_ChildNodes = value;
            }
        }

        /// <summary>
        /// 用户列表
        /// </summary>
        private List<ADUserInfo> m_Users = new List<ADUserInfo>();
        public List<ADUserInfo> Users
        {
            get
            {
                return m_Users;
            }
            set
            {
                m_Users = value;
            }
        }

        /// <summary>
        /// 树节点名称
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Tag存储
        /// </summary>
        public object Tag { get; set; }
    }
}