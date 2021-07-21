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
    public class AJTM_LEADER_UNIT : BBaseQuery
    {
        /// <summary>
        /// 单例
        /// </summary>
        public static AJTM_LEADER_UNIT Instance = new AJTM_LEADER_UNIT();
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public AJTM_LEADER_UNIT()
        {
            this.IsAddIntoCache = true;
            this.TableName = "AJTM_LEADER_UNIT";
            this.ItemName = "单位领导信息";
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
            /// 领导类型
            /// </summary>
            [Field(IsNotNull = true, DefaultValue = "0", Comment = "领导类型")]
            public int LEADER_TYPE_ID { get; set; }
            /// <summary>
            /// 领导类型
            /// </summary>
            [Field(IsNotNull = true, Length = 128, Comment = "领导类型")]
            public string LEADER_TYPE { get; set; }
            /// <summary>
            /// 用编单位
            /// </summary>
            [Field(IsNotNull = true, DefaultValue = "0", Comment = "用编单位ID")]
            public int UNIT_ID { get; set; }
            /// <summary>
            /// 用编单位
            /// </summary>
            [Field(IsNotNull = true, Length = 128, Comment = "用编单位")]
            public string UNIT_NAME { get; set; }
            /// <summary>
            /// 修改者者ID
            /// </summary>
            [Field(IsNotNull = true, DefaultValue = "0", Comment = "申报单位ID")]
            public int UNIT_PARENT_ID { get; set; }
            /// <summary>
            /// 用编单位
            /// </summary>
            [Field(IsNotNull = true, Length = 128, Comment = "申报单位")]
            public string UNIT_PARENT { get; set; }
            /// <summary>
            /// 数量
            /// </summary>
            [Field(IsNotNull = true, DefaultValue = "0", Comment = "数量")]
            public int NUM { get; set; }
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
        /// <param name="unitID"></param>
        /// <returns></returns>
        public IList<Entity> GetListEntityByUnitId(int unitId)
        {
            return GetList<Entity>(" UNIT_ID=?", new object[] { unitId });
        }


        public IList<Entity> GetListEntity()
        {
            return GetList<Entity>();
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="LeaderTypeID">领导类型ID</param>
        /// <param name="LeaderType">领导类型</param>
        /// <param name="UnitID">单位ID</param>
        /// <param name="UnitName">单位</param>
        /// <param name="UnitParentId">主管部门ID</param>
        /// <param name="UnitParent">主管部门</param>
        /// <param name="Num">数量</param>
        /// <returns></returns>
        public int Add(int LeaderTypeID, string LeaderType, int UnitID, string UnitName, int UnitParentId, string UnitParent, int Num)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("LEADER_TYPE_ID", LeaderTypeID);
            dic.Add("LEADER_TYPE", LeaderType);
            dic.Add("UNIT_NAME", UnitName);
            dic.Add("UNIT_ID", UnitID);
            dic.Add("UNIT_PARENT_ID", UnitParentId);
            dic.Add("UNIT_PARENT", UnitParent);
            dic.Add("NUM", Num);
            dic.Add("CREATE_UID", SystemSession.UserID);
            dic.Add("UPDATE_UID", SystemSession.UserID);
            dic.Add("CREATE_TIME", DateTime.Now);
            dic.Add("UPDATE_TIME", DateTime.Now);
            return Add(dic);
        }



        public int SplusNum(int LeaderTypeId, int UnitId)
        {
            var dt = GetTableFields("ID,NUM", " LEADER_TYPE_ID=? AND UNIT_ID=?", LeaderTypeId, UnitId);
            if (dt.Rows.Count > 0)
            {
                var ID = Convert.ToInt32(dt.Rows[0][1]);
                var NUM = Convert.ToInt32(dt.Rows[0][1]);
                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic.Add("NUM", NUM - 1);
                return UpdateByKey(dic, ID);
            }
            return -1;
        }

        public string PATH = "../File/LEADERUNIT";
    }
}
