using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CS.Base.DBHelper;
using CS.Library.BaseQuery;
using CS.Common.FW;
using CS.BLL.FW;

namespace CS.BLL.Model
{
    public class AJTM_AS_DETAIL_STATUS : BBaseQuery
    {
        /// <summary>
        /// 单例
        /// </summary>
        public static AJTM_AS_DETAIL_STATUS Instance = new AJTM_AS_DETAIL_STATUS();
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public AJTM_AS_DETAIL_STATUS()
        {
            this.IsAddIntoCache = true;
            this.TableName = "AJTM_AS_DETAIL_STATUS";
            this.ItemName = "待上编状态变更表";
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
            /// 待上编ID
            /// </summary>
            [Field(IsNotNull = true, DefaultValue = "0", Comment = "创建者ID")]
            public int AS_DETAIL_ID { get; set; }
            /// <summary>
            /// 编制使用通知单ID
            /// </summary>
            [Field(IsNotNull = true, DefaultValue = "0", Comment = "编制使用通知单ID")]
            public int AS_APPLY_ID { get; set; }
            /// <summary>
            /// 创建者ID
            /// </summary>
            [Field(IsNotNull = false, Length = 128, Comment = "用编文件文号(编制使用通知单)")]
            public string AS_APPLY_NO { get; set; }
            /// <summary>
            /// 状态
            /// </summary>
            [Field(IsNotNull = true, Length = 10, IsIndex = true, IsIndexUnique = true, Comment = "状态")]
            public string STATUS { get; set; }
            /// <summary>
            /// 创建时间
            /// </summary>
            [Field(IsNotNull = true, DefaultValue = "NOW", Comment = "创建时间")]
            public DateTime STATUS_TIME { get; set; }
            /// <summary>
            /// 创建者ID
            /// </summary>
            [Field(IsNotNull = true, DefaultValue = "0", Comment = "创建者ID")]
            public int CREATE_UID { get; set; }

            /// <summary>
            /// 修改者者ID
            /// </summary>
            [Field(IsNotNull = true, DefaultValue = "0", Comment = "修改者者ID")]
            public int UPDATE_UID { get; set; }

            /// <summary>
            /// 创建时间
            /// </summary>
            [Field(IsNotNull = true, DefaultValue = "NOW", Comment = "创建时间")]
            public DateTime CREATE_TIME { get; set; }

            /// <summary>
            /// 修改时间
            /// </summary>
            [Field(IsNotNull = true, DefaultValue = "NOW", Comment = "修改时间")]
            public DateTime UPDATE_TIME { get; set; }

        }
        #endregion
    }
}

namespace CS.BLL.Model
{
    public enum ENUM_AS_DETAIL_STATUS
    {
        创建,
        使用,
        销号
    }
}