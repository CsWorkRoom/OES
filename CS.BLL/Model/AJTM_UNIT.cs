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
    /// <summary>
    /// 单位信息
    /// </summary>
    public class AJTM_UNIT : BBaseQuery
    {
        /// <summary>
        /// 单例
        /// </summary>
        public static AJTM_UNIT Instance = new AJTM_UNIT();
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public AJTM_UNIT()
        {
            this.IsAddIntoCache = true;
            this.TableName = "AJTM_UNIT";
            this.ItemName = "单位信息";
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
            /// 单位名称
            /// </summary>
            [Field(IsNotNull = true, Length = 128, IsIndex = true, IsIndexUnique = true, Comment = "单位名称")]
            public string NAME { get; set; }
            /// <summary>
            /// 数据库ID（为0时表示默认数据库）
            /// </summary>
            [Field(IsNotNull = false,  Comment = "主管单位")]
            public int PARENT_ID { get; set; }
            /// <summary>
            /// 机构类别
            /// </summary>
            [Field(IsNotNull = true, DefaultValue = "0", Comment = "机构类别")]
            public int SETUP_TYPE_ID { get; set; }
            /// <summary>
            /// 机构性质
            /// </summary>
            [Field(IsNotNull = true, DefaultValue = "0", Comment = "机构性质")]
            public int SETUP_NATRUE_ID { get; set; }
            /// <summary>
            /// 机构级别
            /// </summary>
            [Field(IsNotNull = true, DefaultValue = "0", Comment = "机构级别")]
            public int SETUP_LEVEL_ID { get; set; }
            /// <summary>
            /// 经费形式
            /// </summary>
            [Field(IsNotNull = true, DefaultValue = "0", Comment = "经费形式")]
            public int OUTLAY_MODE_ID { get; set; }
            /// <summary>
            /// 机构划属
            /// </summary>
            [Field(IsNotNull = true, DefaultValue = "0", Comment = "机构划属")]
            public int SETUP_RANGE_ID { get; set; }
            /// <summary>
            /// 参公情况
            /// </summary>
            [Field(IsNotNull = true, DefaultValue = "0", Comment = "参公情况")]
            public int IS_PUBLIC { get; set; }
            /// <summary>
            /// 内设机构数
            /// </summary>
            [Field(IsNotNull = true, DefaultValue = "0", Comment = "内设机构数")]
            public int DEP_NUM { get; set; }
            /// <summary>
            /// 是否启用
            /// </summary>
            [Field(IsNotNull = true, DefaultValue = "0", Comment = "是否启用")]
            public int IS_USE { get; set; }
            /// <summary>
            /// 内设机构领导正职核定数
            /// </summary>
            [Field(IsNotNull = false, DefaultValue = "0", Comment = "内设机构领导正职核定数")]
            public int WITHIN_MAIN_NUM { get; set; }
            /// <summary>
            /// 内设机构领导副职核定数
            /// </summary>
            [Field(IsNotNull = false, DefaultValue = "0", Comment = "内设机构领导副职核定数")]
            public int WITHIN_VICE_NUM { get; set; }
            /// <summary>
            /// 部门其他工作机构领导核定数
            /// </summary>
            [Field(IsNotNull = false, DefaultValue = "0", Comment = "部门其他工作机构领导核定数")]
            public int OTHER_NUM { get; set; }
            /// <summary>
            /// 厅局级正职核定数
            /// </summary>
            [Field(IsNotNull = false, DefaultValue = "0", Comment = "厅局级正职核定数")]
            public int OFFICE_MIAN_NUM { get; set; }
            /// <summary>
            /// 厅局级副职核定数
            /// </summary>
            [Field(IsNotNull = false, DefaultValue = "0", Comment = "厅局级副职核定数")]
            public int OFFICE_VICE_NUM { get; set; }
            /// <summary>
            /// 县处级正职核定数（领导)
            /// </summary>
            [Field(IsNotNull = false, DefaultValue = "0", Comment = "县处级正职核定数（领导)")]
            public int COUNTY_MIAN_L { get; set; }
            /// <summary>
            /// 县处级副职核定数（领导）
            /// </summary>
            [Field(IsNotNull = false, DefaultValue = "0", Comment = "县处级副职核定数（领导）")]
            public int COUNTY_VICE_L { get; set; }
            /// <summary>
            /// 乡科级正职核定数（领导）
            /// </summary>
            [Field(IsNotNull = false, DefaultValue = "0", Comment = "乡科级正职核定数（领导）")]
            public int VILLAGE_MIAN_L { get; set; }
            /// <summary>
            /// 乡科级副职核定数（领导）
            /// </summary>
            [Field(IsNotNull = false, DefaultValue = "0", Comment = "乡科级副职核定数（领导）")]
            public int VILLAGE_VICE_L { get; set; }
            /// <summary>
            /// 县处级正职核定数（中层）
            /// </summary>
            [Field(IsNotNull = false, DefaultValue = "0", Comment = "县处级正职核定数（中层）")]
            public int COUNTY_MIAN_MR { get; set; }
            /// <summary>
            /// 县处级副职核定数（中层）
            /// </summary>
            [Field(IsNotNull = false, DefaultValue = "0", Comment = "县处级副职核定数（中层）")]
            public int COUNTY_VICE_MR { get; set; }
            /// <summary>
            /// 乡科级副职核定数（中层）
            /// </summary>
            [Field(IsNotNull = false, DefaultValue = "0", Comment = "乡科级副职核定数（中层）")]
            public int VILLAGE_MIAN_MR { get; set; }
            /// <summary>
            /// 乡科级正职核定数（中层）
            /// </summary>
            [Field(IsNotNull = false, DefaultValue = "0", Comment = "乡科级正职核定数（中层）")]
            public int VILLAGE_VICE_MR { get; set; }
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

        #region 启用
        /// <summary>
        /// 启用
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int SetEnable(int id)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("IS_USE", 1);
            dic.Add("UPDATE_TIME", DateTime.Now);
            dic.Add("UPDATE_UID", SystemSession.UserID);
            return UpdateByKey(dic, id);
        }
        #endregion

        #region 禁用
        /// <summary>
        /// 禁用
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int SetUnable(int id)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("IS_USE", 0);
            dic.Add("UPDATE_TIME", DateTime.Now);
            dic.Add("UPDATE_UID", SystemSession.UserID);

            return UpdateByKey(dic, id);
        }
        #endregion
        /// <summary>
        /// 下拉树
        /// </summary>
        /// <returns></returns>
        public List<object> GetDropTree()
        {
            var dt = GetTableFields("ID,PARENT_ID,NAME");
            List<object> list = new List<object>();
            foreach (DataRow dr in dt.Rows)
            {
                list.Add(new
                {
                    id = dr["ID"],
                    pId = dr["PARENT_ID"],
                    name = dr["NAME"],
                    value = dr["ID"]
                });
            }
            return list;
        }
    }
}
