using CS.Base.DBHelper;
using CS.Library.BaseQuery;
using CS.Common.FW;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CS.BLL.FW
{
    /// <summary>
    /// RDLC报表
    /// </summary>
    public class BF_RDLC_REPORT : BBaseQuery
    {
        /// <summary>
        /// 单例
        /// </summary>
        public static BF_RDLC_REPORT Instance = new BF_RDLC_REPORT();

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public BF_RDLC_REPORT()
        {
            this.IsAddIntoCache = true;
            this.TableName = "BF_RDLC_REPORT";
            this.ItemName = "RDLC报表";
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
            /// 报表名称
            /// </summary>
            [Field(IsNotNull = true, Length = 128, IsIndex = true, IsIndexUnique = true, Comment = "RDLC报表名称")]
            public string NAME { get; set; }

            /// <summary>
            /// 数据库ID（为0时表示默认数据库）
            /// </summary>
            [Field(IsNotNull = true, DefaultValue = "0", Comment = "数据库ID（为0时表示默认数据库）")]
            public int DB_ID { get; set; }


            /// <summary>
            /// 是否显示“导出”按钮
            /// </summary>
            [Field(IsNotNull = true, DefaultValue = "1", Comment = "是否显示“导出”按钮")]
            public Int16 IS_SHOW_EXPORT { get; set; }

            /// <summary>
            /// 是否显示“调试”按钮
            /// </summary>
            [Field(IsNotNull = true, DefaultValue = "1", Comment = "是否显示“调试”按钮")]
            public Int16 IS_SHOW_DEBUG { get; set; }

            /// <summary>
            /// SQL语句
            /// </summary>
            [Field(IsNotNull = true, Length = 4096, Comment = "SQL语句")]
            public string SQL_CODE { get; set; }

            /// <summary>
            /// RDLC代码体
            /// </summary>
            [Field(IsNotNull = true, Length = 4096, Comment = "RDLC代码体")]
            public string RDLC_CODE { get; set; }

            /// <summary>
            /// 表格上方的代码（允许HTML、CSS及JS）
            /// </summary>
            [Field(IsNotNull = true, Length = 4096, Comment = "表格上方的代码（允许HTML、CSS及JS）")]
            public string TOP_CODE { get; set; }

            /// <summary>
            /// 表格下方的代码（允许HTML、CSS及JS）
            /// </summary>
            [Field(IsNotNull = true, Length = 4096, Comment = "表格下方的代码（允许HTML、CSS及JS）")]
            public string BOTTOM_CODE { get; set; }

            /// <summary>
            /// 是否启用
            /// </summary>
            [Field(IsNotNull = true, DefaultValue = "1", Comment = "是否有效")]
            public Int16 IS_ENABLE { get; set; }

            /// <summary>
            /// 备注
            /// </summary>
            [Field(IsNotNull = false, Length = 512, Comment = "备注")]
            public string REMARK { get; set; }

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
            dic.Add("IS_ENABLE", 1);
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
            dic.Add("IS_ENABLE", 0);
            dic.Add("UPDATE_TIME", DateTime.Now);
            dic.Add("UPDATE_UID", SystemSession.UserID);

            return UpdateByKey(dic, id);
        }
        #endregion

        #region 查询列表
        public DataTable GetDataTable(int limit, int page, ref int count, string name, string orderByField = "CR.ID", string orderByType = "DESC")
        {
            string strWhere = "1=1";
            List<object> param = new List<object>();
            #region 添加参数
            if (string.IsNullOrWhiteSpace(name) == false)
                strWhere += " AND CR.NAME LIKE '%" + name.Replace('\'', ' ') + "%'";
            #endregion

            string strSql = "SELECT CR.ID,CR.NAME,DB.NAME DBNAME,(CASE IS_SHOW_EXPORT WHEN 1 THEN '是' ELSE '否' END )SHOWEXPORT,(CASE IS_SHOW_DEBUG WHEN 1 THEN '是' ELSE '否' END )SHOWDEBUG,SQL_CODE,(CASE IS_ENABLE WHEN 1 THEN '是' ELSE '否' END )IS_ENABLE,CR.CREATE_TIME,CR.update_time FROM BF_RDLC_REPORT CR LEFT JOIN BF_DATABASE DB on CR.DB_ID=DB.ID WHERE " + strWhere;
            //添加排序
            if (string.IsNullOrWhiteSpace(orderByField) == false)
                strSql += " ORDER BY " + orderByField + " " + (string.IsNullOrWhiteSpace(orderByType) == false ? orderByType : "");

            using (BDBHelper dbHelper = new BDBHelper())
            {
                if (limit == 0 && page == 0)
                {
                    return dbHelper.ExecuteDataTableParams(strSql);//不分页查询所有
                }
                //算总记录
                if (count == 0)
                {
                    string sqlCount = string.Format("SELECT COUNT(*) FROM ({0})", strSql);
                    count = dbHelper.ExecuteScalarIntParams(sqlCount, param);
                }
                return dbHelper.ExecuteDataTablePageParams(strSql, limit, page, param);
            }

        }
        #endregion
    }
}
