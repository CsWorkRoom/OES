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
    public class AJTM_EDUACATION : BBaseQuery
    {
        /// <summary>
        /// 单例
        /// </summary>
        public static AJTM_EDUACATION Instance = new AJTM_EDUACATION();
        /// <summary>
        /// 路径
        /// </summary>
        public static string PATH = "../File/EDUACATION/";
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public AJTM_EDUACATION()
        {
            this.IsAddIntoCache = true;
            this.TableName = "AJTM_EDUACATION";
            this.ItemName = "基层教育";
            this.KeyField = "ID";
            this.OrderbyFields = "ID";
        }
        #endregion


        public class Entity
        {
            /// <summary>
            /// ID
            /// </summary>
            public int ID { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int DISTRICT_ID { get; set; }
            /// <summary>
            /// 标题
            /// </summary>
            public string TITLE { get; set; }
            /// <summary>
            /// LUCKY_EXCEL
            /// </summary>
            public string LUCKY_EXCEL { get; set; }
            /// <summary>
            /// 上传文件地址
            /// </summary>
            public string EXCEL_PATH { get; set; }
            /// <summary>
            /// 下载地址
            /// </summary>
            public string EXCEL_DOWN { get; set; }
            /// <summary>
            /// 创建人
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
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="title"></param>
        /// <param name="excel"></param>
        /// <param name="path"></param>
        /// <param name="down"></param>
        /// <returns></returns>
        public int Add(string title, string excel, string path, string down, int districtId)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("TITLE", title);
            dic.Add("LUCKY_EXCEL", excel);
            dic.Add("EXCEL_PATH", path);
            dic.Add("EXCEL_DOWN", down);
            dic.Add("CREATE_UID", SystemSession.UserID);
            dic.Add("UPDATE_UID", SystemSession.UserID);
            dic.Add("CREATE_TIME", DateTime.Now);
            dic.Add("UPDATE_TIME", DateTime.Now);
            dic.Add("DISTRICT_ID", districtId);
            return Add(dic, true);
        }
        /// <summary>
        /// 历史
        /// </summary>
        /// <param name="id"></param>
        /// <param name="title"></param>
        /// <param name="excel"></param>
        /// <param name="path"></param>
        /// <param name="down"></param>
        /// <returns></returns>
        public int Update(int id, string title, string excel, string path, string down,int districtId)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("TITLE", title);
            dic.Add("LUCKY_EXCEL", excel);
            //dic.Add("EXCEL_PATH", path);
            dic.Add("EXCEL_DOWN", down);
            dic.Add("UPDATE_UID", SystemSession.UserID);
            dic.Add("UPDATE_TIME", DateTime.Now);
            dic.Add("DISTRICT_ID", districtId);
            return UpdateByKey(dic, id);
        }
    }
}
