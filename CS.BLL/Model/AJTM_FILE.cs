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
    public class AJTM_FILE:BBaseQuery
    {
        /// <summary>
        /// 单例
        /// </summary>
        public static AJTM_FILE Instance = new AJTM_FILE();
        public AJTM_FILE()
        {
            this.IsAddIntoCache = true;
            this.TableName = "AJTM_FILE";
            this.ItemName = "文件附件";
            this.KeyField = "ID";
            this.OrderbyFields = "ID";
        }

        public class Entity
        {
            /// <summary>
            /// ID
            /// </summary>
            public int ID { get; set; }
            /// <summary>
            /// 名称
            /// </summary>
            public string NAME { get; set; }

            /// <summary>
            /// 路径
            /// </summary>
            public string PATH { get; set; }

            /// <summary>
            /// 用户ID
            /// </summary>
            public int USER_ID { get; set; }

            /// <summary>
            /// 大小
            /// </summary>
            public int LENGTH { get; set; }
            /// <summary>
            /// 添加时间
            /// </summary>

            public string UPLOAD_TIME { get; set; }
            /// <summary>
            /// 相对路径
            /// </summary>

            public string URL { get; set; }
            /// <summary>
            /// 文件类型
            /// </summary>

            public string FILE_TYPE { get; set; }
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Entity Add(Entity entity)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("NAME", entity.NAME);
            dic.Add("PATH", entity.PATH);
            dic.Add("LENGTH", entity.LENGTH);
            dic.Add("URL", entity.URL);
            dic.Add("USER_ID", SystemSession.UserID);
            dic.Add("UPLOAD_TIME", DateTime.Now);
            dic.Add("FILE_TYPE", entity.FILE_TYPE);
            entity.ID = Add(dic);
            return entity;
        }
        /// <summary>
        /// 获取文件后缀名
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string GetSuffixByPath(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                return string.Empty;
            }
            int index = filePath.LastIndexOf('.');
            if (index < 0)
            {
                return "";
            }
            return filePath.Substring(index + 1);
        }
    }
    
}
