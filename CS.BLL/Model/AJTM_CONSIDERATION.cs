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
    public class AJTM_CONSIDERATION : BBaseQuery
    {
        /// <summary>
        /// 单例
        /// </summary>
        public static AJTM_CONSIDERATION Instance = new AJTM_CONSIDERATION();
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public AJTM_CONSIDERATION()
        {
            this.IsAddIntoCache = true;
            this.TableName = "AJTM_CONSIDERATION";
            this.ItemName = "审议表";
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
            public string NAME { get; set; }
            /// <summary>
            /// 用编申报IDS
            /// </summary>
            [Field(IsNotNull = true, Length = 500, Comment = "审议表名")]
            public string IDS { get; set; }
            /// <summary>
            /// 用编审议表(文件路径)
            /// </summary>
            [Field(IsNotNull = true, Length = 500, Comment = "用编审议表(文件路径)")]
            public string PATH { get; set; }
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


        public string PATH_BASE = "../File/CONSIDERATION/";

        public string PATH_FILENAME = "Template.xls";
        /// <summary>
        /// 获取审议信息
        /// </summary>
        /// <param name="IDs"></param>
        /// <returns></returns>
        public DataTable GetApplyConsideration(string IDs)
        {
            string sql = @"
               SELECT A.ID,A.UNIT_NAME,A.UNIT_PARENT,C.NAME SETUP_LEVEL, D.NAME SETUP_TYPE,DECODE(B.IS_PUBLIC,1,'是','否') IS_PUBLIC,E.VERIFICATION_NUM,F.ACTUAL_NUM,
                0 AS LEADER_NULL_NUM,
                G.AS_DEAIL_NUM,
                0 AS PRINCIPLE_RESERVE_NUM,
                0 AS ORTHER_NUM,
                0 AS USABLE_AS_NUM,
                A.APPLY_NUM,
                0 AS SUGGEST_NUM,
                0 AS SURPLUS_NUM,
                '' AS REASON,
                '' AS REMARK
                FROM AJTM_AS_APPLY A 
                LEFT JOIN AJTM_UNIT B ON(A.UNIT_ID = B.ID) 
                LEFT JOIN AJTM_SETUP_LEVEL C ON(B.SETUP_LEVEL_ID = C.ID)
                LEFT JOIN AJTM_SETUP_TYPE D ON(B.SETUP_TYPE_ID =D.ID)
                LEFT JOIN (
                    SELECT UNIT_ID,SUM(VERIFICATION_NUM) VERIFICATION_NUM FROM AJTM_UNIT_AS GROUP BY UNIT_ID
                )E ON(A.UNIT_ID = E.UNIT_ID)
                LEFT JOIN VIEW_STATISTICS_UPDOWN F ON(A.UNIT_ID =F.UNIT_ID)
                LEFT JOIN (
                    SELECT UNIT_ID,COUNT(1) AS AS_DEAIL_NUM FROM AJTM_AS_DETAIL WHERE USE_TIME IS NULL GROUP BY UNIT_ID
                )G ON(A.UNIT_ID = G.UNIT_ID)
              WHERE A.ID IN(?)
            ";
            using (BDBHelper dbHelper = new BDBHelper())
            {
                return dbHelper.ExecuteDataTableParams(sql, new object[] { IDs });
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetConsiderationName()
        {
            var index = GetCount(" TO_CHAR(CREATE_TIME,'YYYY')=? ", new object[] { DateTime.Now.ToString("yyyy") });
            index += 1;
            return "审议表[" + DateTime.Now.ToString("yyyy") + "]" + index + "号"; ;
        }
    }
}
