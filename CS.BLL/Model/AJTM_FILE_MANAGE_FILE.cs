using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CS.Library.BaseQuery;
using CS.Common.FW;
using CS.BLL.FW;
using System.Data;

namespace CS.BLL.Model
{
    public class AJTM_FILE_MANAGE_FILE : BBaseQuery
    {
        /// <summary>
        /// 单例
        /// </summary>
        public static AJTM_FILE_MANAGE_FILE Instance = new AJTM_FILE_MANAGE_FILE();
        /// <summary>
        /// 
        /// </summary>
        public AJTM_FILE_MANAGE_FILE()
        {
            this.IsAddIntoCache = true;
            this.TableName = "AJTM_FILE_MANAGE_FILE";
            this.ItemName = "文件管理-附件";
            this.KeyField = "FILE_MANAGE_ID";
            this.OrderbyFields = "FILE_MANAGE_ID";
        }
    }
}
