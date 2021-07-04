﻿using System;
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

    public class AJTM_AS_PURPOSE : BBaseQuery
    {
        /// <summary>
        /// 单例
        /// </summary>
        public static AJTM_AS_PURPOSE Instance = new AJTM_AS_PURPOSE();
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public AJTM_AS_PURPOSE()
        {
            this.IsAddIntoCache = true;
            this.TableName = "AJTM_AS_PURPOSE";
            this.ItemName = "编制用途";
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
            [Field(IsNotNull = true, Length = 128, IsIndex = true, IsIndexUnique = true, Comment = "编制用途名称")]
            public string NAME { get; set; }
            /// <summary>
            /// 备注
            /// </summary>
            [Field(IsNotNull = false, Length = 512, Comment = "备注")]
            public string REMARK { get; set; }
            /// <summary>
            /// 是否在用
            /// </summary>
            [Field(IsNotNull = true, DefaultValue = "1", Comment = "是否在用")]
            public int IS_USE { get; set; }
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
        /// 获取下拉
        /// </summary>
        /// <returns></returns>
        public DataTable GetDropDown()
        {
            return GetTableFields("ID,NAME");
        }

        /// <summary>
        /// 获取下拉
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, string> GetDicDropDown()
        {
            return GetDictionary("ID", "NAME", " IS_USE = 1", new object[] { });
        }


        public string GetNames(int ids)
        {
            var dt = GetTableFields("NAME", " ID in (" + ids + ")", new object[] { });
            string[] list = dt.AsEnumerable().Select(x => x.Field<string>("NAME")).ToArray();
            return string.Join(",", list);
        }
    }
}
