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


        //public string SaveExcal()
        //{
        //    //Library.Export.ExcelFile xls = new Library.Export.ExcelFile("/Files/Consideration/");
        //    //DataTable dt = new DataTable();
        //    //xls.ToDataTable("Temp.xls", ref dt, 3);
        //    //xls.
        //}
    }
}
