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
     public class AJTM_CONSIDERATION_APPLY :BBaseQuery
    {
                /// <summary>
        /// 单例
        /// </summary>
        public static AJTM_CONSIDERATION_APPLY Instance = new AJTM_CONSIDERATION_APPLY();
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public AJTM_CONSIDERATION_APPLY()
        {
            this.IsAddIntoCache = true;
            this.TableName = "AJTM_CONSIDERATION_APPLY";
            this.ItemName = "用编审议表";
            this.KeyField = "ID";
            this.OrderbyFields = "ID";
        }
        #endregion

        #region 实体
        /// <summary>
        /// 实体
        /// </summary>
        public class Entity
        {
            /// <summary>
            /// ID 
            /// </summary>
            [Field(IsPrimaryKey = true, IsAutoIncrement = true, IsNotNull = true, Comment = "ID ")]
            public int ID { get; set; }
            /// <summary>
            /// 审议表名
            /// </summary>
            [Field(IsNotNull = true, Length = 100, Comment = "审议表名")]
            public int CONSIDERATION_ID { get; set; }
            /// <summary>
            /// 用编申报IDS
            /// </summary>
            [Field(IsNotNull = true, Length = 500, Comment = "审议表名")]
            public int AS_APPLY_ID { get; set; }
        }
        #endregion

    }
}
