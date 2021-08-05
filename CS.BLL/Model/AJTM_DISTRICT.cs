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
    public class AJTM_DISTRICT : BBaseQuery
    {
        /// <summary>
        /// 单例
        /// </summary>
        public static AJTM_DISTRICT Instance = new AJTM_DISTRICT();

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public AJTM_DISTRICT()
        {
            this.IsAddIntoCache = true;
            this.TableName = "AJTM_DISTRICT";
            this.ItemName = "行政归属";
            this.KeyField = "ID";
            this.OrderbyFields = "ID";
        }
        #endregion

        public class Entity
        {
            public int ID { get; set; }

            public int PARENT_ID { get; set; }

            public string NAME { get; set; }

            public string NAME_ABRIDGE { get; set; }
        }


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
