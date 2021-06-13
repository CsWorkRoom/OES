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
    public class AJTM_AS_APPLY : BBaseQuery
    {
        /// <summary>
        /// 单例
        /// </summary>
        public static AJTM_AS_APPLY Instance = new AJTM_AS_APPLY();
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public AJTM_AS_APPLY()
        {
            this.IsAddIntoCache = true;
            this.TableName = "AJTM_AS_APPLY";
            this.ItemName = "用编申请";
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
            [Field(IsNotNull = false, Length = 128, Comment = "用编文件文号(编制使用通知单)")]
            public string AS_APPLY_NO { get; set; }
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
            /// 来文时间
            /// </summary>
            [Field(IsNotNull = true, DefaultValue = "NOW", Comment = "创建时间")]
            public DateTime APPLY_TIME { get; set; }
            /// <summary>
            /// 联系人
            /// </summary>
            [Field(IsNotNull = false, Length = 512, Comment = "联系人")]
            public string ACCOUNT_PEOPLE { get; set; }
            /// <summary>
            /// 联系方式
            /// </summary>
            [Field(IsNotNull = false, Length = 512, Comment = "联系方式")]
            public string ACCOUNT_PHONE { get; set; }
            /// <summary>
            /// 是否为年度用编
            /// </summary>
            [Field(IsNotNull = true, DefaultValue = "0", Comment = "是否为年度用编")]
            public short IS_YEAR { get; set; }
            /// <summary>
            /// 申报总数
            /// </summary>
            [Field(IsNotNull = true, DefaultValue = "0", Comment = "申报总数")]
            public int APPLY_NUM { get; set; }
            /// <summary>
            /// 申报总数
            /// </summary>
            [Field(IsNotNull = true, DefaultValue = "0", Comment = "申报总数")]
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


        public string GetApplyNo()
        {
            var index = GetCount(" TO_CHAR(CREATE_TIME,'YYYY')=? AND AS_APPLY_NO IS NOT NULL", new object[] { DateTime.Now.ToString("yyyy") });
            index += 1;
            return "达编机减[" + DateTime.Now.ToString("yyyy") + "]" + index + "号"; ;
        }
    }
}
