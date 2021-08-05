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
    public class AJTM_EDUACATION_HIS : BBaseQuery
    {
        /// <summary>
        /// 单例
        /// </summary>
        public static AJTM_EDUACATION_HIS Instance = new AJTM_EDUACATION_HIS();
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public AJTM_EDUACATION_HIS()
        {
            this.IsAddIntoCache = true;
            this.TableName = "AJTM_EDUACATION_HIS";
            this.ItemName = "基层教育历史";
            this.KeyField = "ID";
            this.OrderbyFields = "ID";
        }
        #endregion

        /// <summary>
        /// 模型
        /// </summary>
        public class Entity
        {
            /// <summary>
            /// ID
            /// </summary>
            public int ID { get; set; }
            /// <summary>
            /// 基层教育ID
            /// </summary>
            public int EDUACATION_ID { get; set; }
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
        public int Add(int eduacationId, string title, string excel, string path, string down,int DISTRICT_ID)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("TITLE", title);
            dic.Add("EDUACATION_ID", eduacationId);
            dic.Add("LUCKY_EXCEL", excel);
            dic.Add("EXCEL_PATH", path);
            dic.Add("EXCEL_DOWN", down);
            dic.Add("CREATE_UID", SystemSession.UserID);
            dic.Add("UPDATE_UID", SystemSession.UserID);
            dic.Add("CREATE_TIME", DateTime.Now);
            dic.Add("UPDATE_TIME", DateTime.Now);
            dic.Add("DISTRICT_ID", DISTRICT_ID);
            return Add(dic, true);
        }
    }
}
