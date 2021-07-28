using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CS.Base.DBHelper;
using CS.Library.BaseQuery;
using CS.Common.FW;
using CS.BLL.FW;
using System.Data;


namespace CS.BLL.Model
{
    public class AJTM_FILE_MANAGE : BBaseQuery
    {
        /// <summary>
        /// 单例
        /// </summary>
        public static AJTM_FILE_MANAGE Instance = new AJTM_FILE_MANAGE();
        public AJTM_FILE_MANAGE()
        {
            this.IsAddIntoCache = true;
            this.TableName = "AJTM_FILE_MANAGE";
            this.ItemName = "文件管理";
            this.KeyField = "ID";
            this.OrderbyFields = "ID";
        }

        public class Entity
        {
            /// <summary>
            /// ID
            /// </summary>
            public int ID { get; set; }
            /// <summary>
            /// 标题
            /// </summary>
            public string TITLE { get; set; }
            /// <summary>
            /// 内容
            /// </summary>
            public string CONTENT { get; set; }

            /// <summary>
            /// 创建用户
            /// </summary>
            public int CREATE_UID { get; set; }
            /// <summary>
            /// 修改人
            /// </summary>

            public int UPDATE_UID { get; set; }
            /// <summary>
            /// 创建时间
            /// </summary>

            public DateTime CREATE_TIME { get; set; }
            /// <summary>
            /// 修改时间
            /// </summary>

            public DateTime UPDATE_TIME { get; set; }
            /// <summary>
            /// 是否在用
            /// </summary>

            public short IS_USE { get; set; }
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="tilte"></param>
        /// <param name="content"></param>
        /// <returns></returns>

        public int Add(string tilte,string content)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("TITLE", tilte);
            dic.Add("CONTENT", content);
            dic.Add("CREATE_UID", SystemSession.UserID);
            dic.Add("UPDATE_UID",SystemSession.UserID);
            dic.Add("CREATE_TIME", DateTime.Now);
            dic.Add("UPDATE_TIME", DateTime.Now);

           return Add(dic, true);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public int Update(string title,string content,int id)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("TITLE", title);
            dic.Add("CONTENT", content);
            dic.Add("UPDATE_UID", SystemSession.UserID);
            dic.Add("CREATE_TIME", DateTime.Now);

            return UpdateByKey(dic, id);
        }
    }
}
