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
    public class AJTM_AS_DETAIL : BBaseQuery
    {
        /// <summary>
        /// 单例
        /// </summary>
        public static AJTM_AS_DETAIL Instance = new AJTM_AS_DETAIL();
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public AJTM_AS_DETAIL()
        {
            this.IsAddIntoCache = true;
            this.TableName = "AJTM_AS_DETAIL";
            this.ItemName = "待上编";
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
            /// 批复时间
            /// </summary>
            [Field(IsNotNull = true, DefaultValue = "NOW", Comment = "批复时间")]
            public DateTime APPROVAL_TIME { get; set; }
            /// <summary>
            /// 会议名称
            /// </summary>
            [Field(IsNotNull = true, Length = 128, Comment = "会议名称")]
            public string MEETING { get; set; }
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
            /// 编制使用通知单ID
            /// </summary>
            [Field(IsNotNull = true, DefaultValue = "0", Comment = "编制使用通知单ID")]
            public int AS_APPLY_ID { get; set; }
            /// <summary>
            /// 编制使用通知单
            /// </summary>
            [Field(IsNotNull = true, Length = 128, Comment = "编制使用通知单")]
            public string AS_APPLY_NO { get; set; }
            /// <summary>
            /// 用编类型ID
            /// </summary>
            [Field(IsNotNull = true, DefaultValue = "0", Comment = "用编类型ID")]
            public int AS_TYPE_ID { get; set; }
            /// <summary>
            /// 编制类型
            /// </summary>
            [Field(IsNotNull = true, Length = 128, Comment = "编制类型")]
            public string AS_TYPE { get; set; }
            /// <summary>
            /// 编制用途ID
            /// </summary>
            [Field(IsNotNull = true, DefaultValue = "0", Comment = "编制用途")]
            public int AS_PURPOSE_ID { get; set; }
            /// <summary>
            /// 编制用途
            /// </summary>
            [Field(IsNotNull = true, Length = 128, Comment = "编制用途")]
            public string AS_PURPOSE { get; set; }
            /// <summary>
            /// 编制详细用途
            /// </summary>
            [Field(IsNotNull = true, Length = 512, Comment = "编制详细用途")]
            public string AS_PURPOSE_REMARK { get; set; }
            /// <summary>
            /// 用编号码
            /// </summary>
            [Field(IsNotNull = true, Length = 512, Comment = "用编号码")]
            public string AS_NO { get; set; }
            /// <summary>
            /// 批准数量
            /// </summary>
            [Field(IsNotNull = true, DefaultValue = "0", Comment = "批准数量")]
            public int APPROVAL_NUM { get; set; }

            /// <summary>
            /// 创建时间
            /// </summary>
            [Field(IsNotNull = true, DefaultValue = "NOW", Comment = "创建时间")]
            public DateTime CREATE_TIME { get; set; }

            /// <summary>
            /// 使用时间
            /// </summary>
            [Field(IsNotNull = true, DefaultValue = "NOW", Comment = "使用时间")]
            public DateTime USE_TIME { get; set; }
            /// <summary>
            /// 放弃时间
            /// </summary>
            [Field(IsNotNull = true, DefaultValue = "NOW", Comment = "放弃时间")]
            public DateTime CANCEL_TIME { get; set; }

        }
        #endregion

        /// <summary>
        /// 获取刚创建得待上编
        /// </summary>
        /// <param name="UnitId"></param>
        /// <returns></returns>
        public DataTable GetCrateAsDetail(int UnitId)
        {
            return GetTableFields("ID,AS_APPLY_ID,AS_APPLY_NO,AS_TYPE_ID,AS_TYPE,AS_NO", " UNIT_ID=? AND USE_TIME IS NULL AND CANCEL_TIME IS NULL", new object[] { UnitId });
        }
        /// <summary>
        /// 获取正在使用得带上编
        /// </summary>
        /// <param name="UnitId"></param>
        /// <returns></returns>
        public DataTable GetCancelAsDetail(int UnitId)
        {
            return GetTableFields("ID,AS_APPLY_ID,AS_APPLY_NO,AS_TYPE_ID,AS_TYPE,AS_NO", " UNIT_ID=? AND USE_TIME IS NOT NULL AND CANCEL_TIME IS NULL", new object[] { UnitId });
        }

        public string GetAsNo(int i)
        {
            return "N" + DateTime.Now.ToString("yyyy") + i.ToString().PadLeft(5, '0');
        }

        public int GetCurrentNo()
        {
            return GetCount(" TO_CHAR(CREATE_TIME,'YYYY')=?", new object[] { DateTime.Now.ToString("yyyy") });
        }
    }
}
