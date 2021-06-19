using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CS.Library.BaseQuery;
using CS.Common.FW;
using CS.BLL.FW;

namespace CS.BLL.Model
{
    public class AJTM_LEADER : BBaseQuery
    {
        /// <summary>
        /// 单例
        /// </summary>
        public static AJTM_LEADER Instance = new AJTM_LEADER();
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public AJTM_LEADER()
        {
            this.IsAddIntoCache = true;
            this.TableName = "AJTM_LEADER";
            this.ItemName = "领导信息";
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
            /// 创建者ID
            /// </summary>
            [Field(IsNotNull = true, DefaultValue = "0", Comment = "领导类型")]
            public int LEADER_TYPE_ID { get; set; }
            /// <summary>
            /// 用编单位
            /// </summary>
            [Field(IsNotNull = true, DefaultValue = "0", Comment = "用编单位ID")]
            public int UNIT_ID { get; set; }
            /// <summary>
            /// 用编单位
            /// </summary>
            [Field(IsNotNull = true, Length = 128, Comment = "用编单位")]
            public string UNIT_NAME { get; set; }
            /// <summary>
            /// 修改者者ID
            /// </summary>
            [Field(IsNotNull = true, DefaultValue = "0", Comment = "申报单位ID")]
            public int UNIT_PARENT_ID { get; set; }
            /// <summary>
            /// 用编单位
            /// </summary>
            [Field(IsNotNull = true, Length = 128, Comment = "申报单位")]
            public string UNIT_PARENT { get; set; }
            /// <summary>
            /// 领导级别ID
            /// </summary>
            [Field(IsNotNull = true, DefaultValue = "0", Comment = "领导级别ID")]
            public int LAEDER_LEVEL_ID { get; set; }
            /// <summary>
            /// 领导级别
            /// </summary>
            [Field(IsNotNull = true, Length = 128, Comment = "领导级别")]
            public string LEADER_LEVEL { get; set; }
            /// <summary>
            /// 领导职务
            /// </summary>
            [Field(IsNotNull = true, Length = 128, IsIndex = true, IsIndexUnique = true, Comment = "领导职务")]
            public string LEADER_JOB { get; set; }

            /// <summary>
            /// 是否占编
            /// </summary>
            [Field(IsNotNull = true, DefaultValue = "0", Comment = "是否占编")]
            public short IS_AS { get; set; }
            /// <summary>
            /// 是否在职
            /// </summary>
            [Field(IsNotNull = true, DefaultValue = "0", Comment = "是否在职")]
            public short IS_USE { get; set; }
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
