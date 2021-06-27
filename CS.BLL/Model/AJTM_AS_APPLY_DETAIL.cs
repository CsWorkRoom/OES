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
    public class AJTM_AS_APPLY_DETAIL : BBaseQuery
    {
        /// <summary>
        /// 单例
        /// </summary>
        public static AJTM_AS_APPLY_DETAIL Instance = new AJTM_AS_APPLY_DETAIL();
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public AJTM_AS_APPLY_DETAIL()
        {
            this.IsAddIntoCache = true;
            this.TableName = "AJTM_AS_APPLY_DETAIL";
            this.ItemName = "申报明细";
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
            /// 申报ID
            /// </summary>
            [Field(IsNotNull = true, DefaultValue = "0", Comment = "申报ID")]
            public int AS_APPLY_ID { get; set; }
            /// <summary>
            /// 用编类型
            /// </summary>
            [Field(IsNotNull = true, DefaultValue = "0", Comment = "用编类型")]
            public int AS_TYPE_ID { get; set; }
            /// <summary>
            /// 编制用途
            /// </summary>
            [Field(IsNotNull = true, DefaultValue = "0", Comment = "编制用途")]
            public int AS_PURPOSE_ID { get; set; }
            /// <summary>
            /// 编制详细用途
            /// </summary>
            [Field(IsNotNull = true, Length = 512, Comment = "编制详细用途")]
            public string AS_PURPOSE_REMARK { get; set; }
            /// <summary>
            /// 申报数量
            /// </summary>
            [Field(IsNotNull = true, DefaultValue = "0", Comment = "申报数量")]
            public int APPLY_NUM { get; set; }
            /// <summary>
            /// 批准数量
            /// </summary>
            [Field(IsNotNull = true, DefaultValue = "0", Comment = "批准数量")]
            public int APPROVAL_NUM { get; set; }
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

        /// <summary>
        /// 过去
        /// </summary>
        /// <param name="UnitId"></param>
        /// <returns></returns>
        public object GetTableByApplyId(int ApplyId)
        {
            return GetTable(" AS_APPLY_ID =?", new object[] { ApplyId });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public IList<Entity> GetApplyDetailByIDS(string ids)
        {
            return GetList<Entity>(" AS_APPLY_ID IN (?)", ids);
        }
    }
}
