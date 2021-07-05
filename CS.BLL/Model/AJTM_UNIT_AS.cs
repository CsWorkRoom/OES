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
    public class AJTM_UNIT_AS : BBaseQuery
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


        public class UnitAsShow
        {
            public int ID { get; set; }
            /// <summary>
            /// 编制类型
            /// </summary>
            public string TYPE { get; set; }
            /// <summary>
            /// 核定数
            /// </summary>
            public int VERIFICATION_NUM { get; set; }
            /// <summary>
            /// 期初数
            /// </summary>
            public int BEGIN_NUM { get; set; }
            /// <summary>
            /// 实际拥有数
            /// </summary>
            public int ACTUAL_NUM { get; set; }
        }

        /// <summary>
        /// 过去
        /// </summary>
        /// <param name="UnitId"></param>
        /// <returns></returns>
        public object GetTableByUnitId(int UnitId)
        {
            return GetTable(" UNIT_ID =?", new object[] { UnitId });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="UnitId"></param>
        /// <returns></returns>
        public List<UnitAsShow> GetListByUnitId(int UnitId)
        {
            string sql = string.Format(@"
                SELECT  ID,TYPE, BEGIN_NUM,VERIFICATION_NUM,(UP_NUM-DOWN_NUM)  AS ACTUAL_NUM  FROM(
                SELECT A.ID,B.NAME TYPE,BEGIN_NUM,VERIFICATION_NUM,NVL(C.UP_NUM,0) AS UP_NUM,NVL(D.DOWN_NUM,0) AS DOWN_NUM
                FROM AJTM_UNIT_AS A
                LEFT JOIN 
                AJTM_AS_TYPE B ON(A.AS_TYPE_ID = B.ID)
                LEFT JOIN
                (SELECT UNIT_ID,COUNT(1) AS UP_NUM FROM AJTM_AS_PERSONNEL WHERE ACTION = '上编' AND UNIT_ID = {0} GROUP BY UNIT_ID) C ON(A.UNIT_ID = C.UNIT_ID)
                LEFT JOIN
                (SELECT UNIT_ID,COUNT(1) AS DOWN_NUM FROM AJTM_AS_PERSONNEL WHERE ACTION = '下编' AND UNIT_ID = {0} GROUP BY UNIT_ID) D ON(A.UNIT_ID = D.UNIT_ID)
                WHERE A.UNIT_ID = {0})
            ", UnitId);

            using (BDBHelper dbHelper = new BDBHelper())
            {
                var dt = dbHelper.ExecuteDataTable(sql);
                var arr = new List<UnitAsShow>();
                foreach (DataRow dr in dt.Rows)
                {
                    arr.Add(new UnitAsShow()
                    {
                        ID = Convert.ToInt32(dr["ID"]),
                        TYPE = dr["TYPE"].ToString(),
                        BEGIN_NUM = Convert.ToInt32(dr["BEGIN_NUM"]),
                        VERIFICATION_NUM = Convert.ToInt32(dr["VERIFICATION_NUM"]),
                        ACTUAL_NUM = Convert.ToInt32(dr["ACTUAL_NUM"])
                    });
                }
                return arr;
            }
        }
    }
}
