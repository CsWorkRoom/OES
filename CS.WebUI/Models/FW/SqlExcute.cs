using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CS.WebUI.Models.FW
{
    public class SqlExcute
    {
        public int DB_ID { get; set; }

        public string SQL_CODE { get; set; }

        /// <summary>
        /// 待替换变量的json串
        /// </summary>
        public string QUERY_STRING{ get; set; }
    }
}