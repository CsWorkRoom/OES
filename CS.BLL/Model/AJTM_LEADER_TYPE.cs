using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CS.Library.BaseQuery;
using CS.Common.FW;
using CS.BLL.FW;

namespace CS.BLL.Model
{
    public class AJTM_LEADER_TYPE : BBaseQuery
    {
        /// <summary>
        /// 单例
        /// </summary>
        public static AJTM_LEADER_TYPE Instance = new AJTM_LEADER_TYPE();
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public AJTM_LEADER_TYPE()
        {
            this.IsAddIntoCache = true;
            this.TableName = "AJTM_LEADER_TYPE";
            this.ItemName = "领导类型";
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
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Dictionary<int,string> GetDropDown()
        {
            return GetDictionary("ID", "NAME");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Entity> GetListEntity()
        {
            var arr = GetDictionary("ID", "NAME");
            List<Entity> entity = new List<Entity>();
            foreach(var item in arr)
            {
                entity.Add(new Entity()
                {
                    ID = item.Key,
                    NAME = item.Value
                });
            }
           
            return entity;
        }
        /// <summary>
        /// 获取加载模板
        /// </summary>
        /// <returns></returns>
        public object GetTemplate()
        {
            return GetTableFields("ID,NAME");
        }
    }
}
