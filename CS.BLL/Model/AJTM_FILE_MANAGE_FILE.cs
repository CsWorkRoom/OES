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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="fileManageId"></param>
        /// <returns></returns>
        public void Add(string ids,int fileManageId)
        {
            Delete(" FILE_MANAGE_ID = ?", new object[] { fileManageId });
            string[] idArr = ids.Split(',');
            for(int i=0;i< idArr.Length; i++)
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic.Add("FILE_MANAGE_ID", fileManageId);
                dic.Add("FILE_ID", Convert.ToInt32(idArr[i]));
                Add(dic);
            }
        }
    }
}
