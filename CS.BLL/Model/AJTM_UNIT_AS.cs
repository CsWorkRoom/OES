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
    public class AJTM_UNIT_AS:BBaseQuery
    {
        /// <summary>
        /// 单例
        /// </summary>
        public static AJTM_UNIT_AS Instance = new AJTM_UNIT_AS();
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public AJTM_UNIT_AS()
        {
            this.IsAddIntoCache = true;
            this.TableName = "AJTM_UNIT_AS";
            this.ItemName = "单位编制表";
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
            [Field(IsNotNull = true, DefaultValue = "0", Comment = "编制类型")]
            public int AS_TYPE_ID { get; set; }
            /// <summary>
            /// 单位
            /// </summary>
            [Field(IsNotNull = true, DefaultValue = "0", Comment = "单位")]
            public int UNIT_ID { get; set; }
            /// <summary>
            /// 核定数
            /// </summary>
            [Field(IsNotNull = true, DefaultValue = "0", Comment = "核定数")]
            public int VERIFICATION_NUM { get; set; }
            /// <summary>
            /// 期初数
            /// </summary>
            [Field(IsNotNull = true, DefaultValue = "0", Comment = "期初数")]
            public int BEGIN_NUM { get; set; }
            ///// <summary>
            ///// 创建者ID
            ///// </summary>
            //[Field(IsNotNull = true, DefaultValue = "0", Comment = "创建者ID")]
            //public int CREATE_UID { get; set; }

            ///// <summary>
            ///// 修改者者ID
            ///// </summary>
            //[Field(IsNotNull = true, DefaultValue = "0", Comment = "修改者者ID")]
            //public int UPDATE_UID { get; set; }

            ///// <summary>
            ///// 创建时间
            ///// </summary>
            //[Field(IsNotNull = true, DefaultValue = "NOW", Comment = "创建时间")]
            //public DateTime CREATE_TIME { get; set; }

            ///// <summary>
            ///// 修改时间
            ///// </summary>
            //[Field(IsNotNull = true, DefaultValue = "NOW", Comment = "修改时间")]
            //public DateTime UPDATE_TIME { get; set; }

        }
        #endregion

        /// <summary>
        /// 过去
        /// </summary>
        /// <param name="UnitId"></param>
        /// <returns></returns>
        public object GetTableByUnitId(int UnitId)
        {
            return GetTable(" UNIT_ID =?", new object[] { UnitId });
        }
    }
}
