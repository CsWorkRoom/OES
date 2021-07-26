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
            [Field(IsNotNull = false, Comment = "主管单位")]
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
            /// 其他预留
            /// </summary>
            [Field(IsNotNull = true, DefaultValue = "0", Comment = "其他预留")]
            public int RESERVE_NUM { get; set; }
            /// <summary>
            /// 领导备注
            /// </summary>
            [Field(IsNotNull = true, Length = 1000, Comment = "领导备注")]
            public string LEADER_REAMRK { get; set; }
            /// <summary>
            /// 其他预留备注
            /// </summary>
            [Field(IsNotNull = true, Length = 1000, Comment = "其他预留备注")]
            public string RESERVE_REAMRK { get; set; }
            /// <summary>
            /// 其他备注
            /// </summary>
            [Field(IsNotNull = true, Length = 1000, Comment = "其他备注")]
            public string REMARK { get; set; }
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
            var dt = GetTableFields("ID,PARENT_ID,NAME", " IS_USE = 1", new object[] { });
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="unitId"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetUnitByIdForShow(int unitId = 0)
        {
            string sql = string.Format(@"SELECT A.ID,
                   A.NAME,
                   B.NAME AS PANRET,
                   C.NAME SETUP_NATRUE,
                   D.NAME SETUP_TYPE,
                   E.NAME SETUP_LEVEL,
                   E.LEVEL_ID SETUP_LEVEL_SEQ,
                   F.NAME OUTLAY_MODE,
                   G.NAME SETUP_RANGE,
                   decode(a.IS_PUBLIC,1,'参公','') AS IS_PUBLIC,
                   A.DEP_NUM,
                   H.UP_NUM,
                   I.DOWN_NUM,
                    A.WITHIN_MAIN_NUM,
                    A.WITHIN_VICE_NUM,
                    A.OTHER_NUM,
                    A.OFFICE_MIAN_NUM,
                    A.OFFICE_VICE_NUM,
                    A.COUNTY_MIAN_L  ,
                    A.COUNTY_VICE_L  ,
                    A.VILLAGE_MIAN_L ,
                    A.VILLAGE_VICE_L ,
                    A.COUNTY_MIAN_MR ,
                    A.COUNTY_VICE_MR ,
                    A.VILLAGE_MIAN_MR,
                    A.VILLAGE_VICE_MR
              FROM AJTM_UNIT A
                   LEFT JOIN AJTM_UNIT B ON (A.PARENT_ID = B.ID)
                   LEFT JOIN AJTM_SETUP_NATRUE C ON (A.SETUP_NATRUE_ID = C.ID)
                   LEFT JOIN AJTM_SETUP_TYPE D ON (A.SETUP_TYPE_ID = D.ID)
                   LEFT JOIN AJTM_SETUP_LEVEL E ON (A.SETUP_LEVEL_ID = E.ID)
                   LEFT JOIN AJTM_OUTLAY_MODE F ON (A.OUTLAY_MODE_ID = F.ID)
                   LEFT JOIN AJTM_SETUP_RANGE G ON (A.SETUP_RANGE_ID = G.ID)
                   LEFT JOIN (SELECT UNIT_ID,COUNT(1) AS UP_NUM FROM AJTM_AS_PERSONNEL WHERE ACTION = '上编' AND UNIT_ID = {0} GROUP BY UNIT_ID)  H ON(A.ID = H.UNIT_ID)
                   LEFT JOIN (SELECT UNIT_ID,COUNT(1) AS DOWN_NUM FROM AJTM_AS_PERSONNEL WHERE ACTION = '下编' AND UNIT_ID = {0} GROUP BY UNIT_ID) I ON(A.ID = I.UNIT_ID)
            WHERE A.IS_USE = 1 AND A.ID = {0}
            ", unitId);
            using (BDBHelper dbHelper = new BDBHelper())
            {
                var dt = dbHelper.ExecuteDataTable(sql);
                Dictionary<string, string> dic = new Dictionary<string, string>();
                if (dt.Rows.Count > 0)
                {
                    var dr = dt.Rows[0];
                    dic.Add("ID", dr["ID"].ToString());
                    dic.Add("NAME", dr["NAME"].ToString());
                    dic.Add("PANRET", dr["PANRET"].ToString());
                    dic.Add("SETUP_NATRUE", dr["SETUP_NATRUE"].ToString());
                    dic.Add("SETUP_TYPE", dr["SETUP_TYPE"].ToString());
                    dic.Add("SETUP_LEVEL", dr["SETUP_LEVEL"].ToString());
                    dic.Add("SETUP_LEVEL_SEQ", dr["SETUP_LEVEL_SEQ"].ToString());
                    dic.Add("OUTLAY_MODE", dr["OUTLAY_MODE"].ToString());
                    dic.Add("SETUP_RANGE", dr["SETUP_RANGE"].ToString());
                    dic.Add("IS_PUBLIC", dr["IS_PUBLIC"].ToString());
                    dic.Add("DEP_NUM", dr["DEP_NUM"].ToString());
                    dic.Add("UP_NUM", dr["UP_NUM"].ToString());
                    dic.Add("DOWN_NUM", dr["DOWN_NUM"].ToString());
                    dic.Add("WITHIN_MAIN_NUM", dr["WITHIN_MAIN_NUM"].ToString());
                    dic.Add("WITHIN_VICE_NUM", dr["WITHIN_VICE_NUM"].ToString());
                    dic.Add("OTHER_NUM", dr["OTHER_NUM"].ToString());
                    dic.Add("OFFICE_MIAN_NUM", dr["OFFICE_MIAN_NUM"].ToString());
                    dic.Add("OFFICE_VICE_NUM", dr["OFFICE_VICE_NUM"].ToString());
                    dic.Add("COUNTY_MIAN_L", dr["COUNTY_MIAN_L"].ToString());
                    dic.Add("COUNTY_VICE_L", dr["COUNTY_VICE_L"].ToString());
                    dic.Add("VILLAGE_MIAN_L", dr["VILLAGE_MIAN_L"].ToString());
                    dic.Add("VILLAGE_VICE_L", dr["VILLAGE_VICE_L"].ToString());
                    dic.Add("COUNTY_MIAN_MR", dr["COUNTY_MIAN_MR"].ToString());
                    dic.Add("COUNTY_VICE_MR", dr["COUNTY_VICE_MR"].ToString());
                    dic.Add("VILLAGE_MIAN_MR", dr["VILLAGE_MIAN_MR"].ToString());
                    dic.Add("VILLAGE_VICE_MR", dr["VILLAGE_VICE_MR"].ToString());
                }
                else
                {
                    dic.Add("ID", "");
                    dic.Add("NAME", "");
                    dic.Add("PANRET", "");
                    dic.Add("SETUP_NATRUE", "");
                    dic.Add("SETUP_TYPE", "");
                    dic.Add("SETUP_LEVEL", "");
                    dic.Add("SETUP_LEVEL_SEQ", "");
                    dic.Add("OUTLAY_MODE", "");
                    dic.Add("SETUP_RANGE", "");
                    dic.Add("IS_PUBLIC", "");
                    dic.Add("DEP_NUM", "");
                    dic.Add("UP_NUM", "");
                    dic.Add("DOWN_NUM", "");
                    dic.Add("WITHIN_MAIN_NUM", "");
                    dic.Add("WITHIN_VICE_NUM", "");
                    dic.Add("OTHER_NUM", "");
                    dic.Add("OFFICE_MIAN_NUM", "");
                    dic.Add("OFFICE_VICE_NUM", "");
                    dic.Add("COUNTY_MIAN_L", "");
                    dic.Add("COUNTY_VICE_L", "");
                    dic.Add("VILLAGE_MIAN_L", "");
                    dic.Add("VILLAGE_VICE_L", "");
                    dic.Add("COUNTY_MIAN_MR", "");
                    dic.Add("COUNTY_VICE_MR", "");
                    dic.Add("VILLAGE_MIAN_MR", "");
                    dic.Add("VILLAGE_VICE_MR", "");
                }
                return dic;
            }
        }



        public DataTable GetUnitAndParent(int unitId)
        {
            string sql = string.Format(@"SELECT A.ID,
                   A.NAME,
                   A.PARENT_ID,
                   B.NAME AS PANRET
              FROM AJTM_UNIT A
                   LEFT JOIN AJTM_UNIT B ON (A.PARENT_ID = B.ID)
            WHERE A.ID = {0}
            ", unitId);
            using (BDBHelper dbHelper = new BDBHelper())
            {
                return dbHelper.ExecuteDataTable(sql);
            }
        }

        /// <summary>
        /// 获取单位信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetTableForExcel()
        {
            string sql = @"
               SELECT A.ID,
                     A.PARENT_ID,
                     A.NAME,
                     B.NAME AS RANGE_NAME,
                     C.NAME AS LEVEL_NAME,
                     C.LEVEL_ID AS SETUP_LEVEL_SEQ
                FROM AJTM_UNIT A
                     LEFT JOIN AJTM_SETUP_RANGE B ON (A.SETUP_RANGE_ID = B.ID)
                     LEFT JOIN AJTM_SETUP_LEVEL C ON (A.SETUP_LEVEL_ID = C.ID)
            WHERE A.IS_USE = 1 AND C.LEVEL_ID <=3
            ORDER BY A.ID, A.PARENT_ID ASC
            ";
            using (BDBHelper db = new BDBHelper())
            {
                return db.ExecuteDataTable(sql);
            }
        }


        public class TableForExcel
        {
            public int ID { get; set; }
            public int PARENT_ID { get; set; }
            public string NAME { get; set; }
            public string RANGE_NAME { get; set; }
            public string LEVEL_NAME { get; set; }
        }

        public IList<TableForExcel> GetListForExcel()
        {
            var dt = GetTableForExcel();
            var list = new List<TableForExcel>();
            foreach (DataRow dr in dt.Rows)
            {
                list.Add(new TableForExcel()
                {
                    ID = Convert.ToInt32(dr["ID"]),
                    PARENT_ID = Convert.ToInt32(dr["PARENT_ID"]),
                    NAME = dr["NAME"].ToString(),
                    RANGE_NAME = dr["RANGE_NAME"].ToString(),
                    LEVEL_NAME = dr["LEVEL_NAME"].ToString()
                });
            }
            return list;
        }


        //public DataTable GetUnitInfo()
        //{
        //    string sql = @"
        //       SELECT * FROM VIEW_UNIT_INFO A LEFT JOIN VIEWS_UNIT_AS B ON(A.ID = B.UNIT_ID)
        //        LEFT JOIN VIEWS_PERSONNEL C ON(A.ID= C.UNIT_ID)
        //        LEFT JOIN VIEWS_AS_DETAIL D ON(A.ID =D.UNIT_ID)
        //    ";
        //    using (BDBHelper db = new BDBHelper())
        //    {
        //        return db.ExecuteDataTable(sql);
        //    }
        //}

        public DataTable GetUnitInfo()
        {
            string sql = @"
              SELECT a.id parent_id,
                     a.name panret_name,
                     b.id,
                     b.name,
                     b.setup_range_id,
                     D.SETUP_NATRUE,
                     D.SETUP_TYPE,
                     D.SETUP_LEVEL,
                     D.SETUP_LEVEL_SEQ,
                     D.OUTLAY_MODE,
                     D.SETUP_RANGE,
                     D.IS_PUBLIC,
                     D.LEADER_UNIT_NUM,
                     D.LEADER_SJ_NUM,
                     D.LEADER_NULL_NUM,
                     D.OFFICE_MIAN_NUM,
                     D.OFFICE_VICE_NUM,
                     D.COUNTY_MIAN_L,
                     D.COUNTY_VICE_L,
                     D.VILLAGE_MIAN_L,
                     D.VILLAGE_VICE_L,
                     D.WITHIN_MAIN_NUM,
                     D.WITHIN_MAIN_SP_NUM,
                     D.WITHIN_VICE_NUM,
                     D.WITHIN_VICE_SP_NUM,
                     D.COUNTY_MIAN_MR,
                     D.COUNTY_VICE_MR,
                     D.VILLAGE_MIAN_MR,
                     D.VILLAGE_VICE_MR,
                     D_XZ_NUM,
                     D_SY_YBSY_NUM,
                     D_SY_CGSY_NUM,
                     D_GQ_KZ_NUM,
                     D_ZF_KY_NUM,
                     D_YR_KY_NUM,
                     P_XZ_YBXZ_NUM,
                     P_XZ_ZFZX_NUM,
                     P_SY_YBSY_NUM,
                     P_SY_CGSY_NUM,
                     P_SY_CBSY_NUM,
                     P_SY_ZZSY_NUM,
                     P_GQ_KZ_NUM,
                     P_ZF_KY_NUM,
                     P_YR_KY_NUM,
                     XZ_YBXZ_NUM,
                     XZ_ZFZX_NUM,
                     SY_YBSY_NUM,
                     SY_CGSY_NUM,
                     SY_CBSY_NUM,
                     SY_ZZSY_NUM,
                     GQ_KZ_NUM,
                     ZF_KY_NUM,
                     YR_KY_NUM
                FROM ajtm_unit a
                     LEFT JOIN ajtm_unit b ON (a.id = b.parent_id OR a.id = b.id)
                      LEFT JOIN (select parent_id,count(1) as nun from ajtm_unit where parent_id <> 0 group by parent_id) c ON (a.id = c.parent_id)
                     LEFT JOIN VIEW_UNIT_INFO D ON (B.ID = D.ID)
                     LEFT JOIN VIEWS_UNIT_AS E ON (D.ID = E.UNIT_ID)
                     LEFT JOIN VIEWS_PERSONNEL F ON (D.ID = F.UNIT_ID)
                     LEFT JOIN VIEWS_AS_DETAIL G ON (D.ID = G.UNIT_ID)
               WHERE C.nun IS NOT NULL
               ORDER BY parent_id ASC,id ASC
            ";
            using (BDBHelper db = new BDBHelper())
            {
                var dt = db.ExecuteDataTable(sql);
                dt.Columns.Add("NO1");
                dt.Columns.Add("NO2");
                int currNo = 1;
                int currParentId = 0;
                int currParentNo = 1;
                foreach (DataRow dr in dt.Rows)
                {
                    //待修改
                    var SetupRange = Convert.ToInt32(dr["setup_range_id"]);
                    if (false)
                    {

                    }
                    else
                    {
                        dr["NO1"] = currNo;
                        currNo++;
                    }
                    //匹配
                    int parent_id = Convert.ToInt32(dr["parent_id"]);
                    if (currParentId == parent_id)
                    {
                        dr["NO2"] = currParentNo;
                        currParentNo++;
                    }
                    else
                    {
                        currParentId = parent_id;
                        currParentNo = 1;
                    }

                }

                return dt;
            }
        }

        public string PATH = "../File/UNIT_REPORT";
    }
}
