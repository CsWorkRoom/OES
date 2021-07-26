using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CS.Library.BaseQuery;
using CS.Common.FW;
using CS.BLL.FW;
using CS.Base.DBHelper;

namespace CS.BLL.Model
{
    public class AJTM_LEADER : BBaseQuery
    {
        /// <summary>
        /// 单例
        /// </summary>
        public static AJTM_LEADER Instance = new AJTM_LEADER();
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public AJTM_LEADER()
        {
            this.IsAddIntoCache = true;
            this.TableName = "AJTM_LEADER";
            this.ItemName = "领导信息";
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
            /// 领导级别ID
            /// </summary>
            [Field(IsNotNull = true, DefaultValue = "0", Comment = "领导级别ID")]
            public int LAEDER_LEVEL_ID { get; set; }
            /// <summary>
            /// 领导级别
            /// </summary>
            [Field(IsNotNull = true, Length = 128, Comment = "领导级别")]
            public string LEADER_LEVEL { get; set; }
            /// <summary>
            /// 领导职务
            /// </summary>
            [Field(IsNotNull = true, Length = 128, IsIndex = true, IsIndexUnique = true, Comment = "领导职务")]
            public string LEADER_JOB { get; set; }
            /// <summary>
            /// 领导名称
            /// </summary>
            [Field(IsNotNull = true, Length = 128, IsIndex = true, IsIndexUnique = true, Comment = "领导职务")]
            public string LEADER_NAME { get; set; }

            /// <summary>
            /// 是否占编
            /// </summary>
            [Field(IsNotNull = true, DefaultValue = "0", Comment = "是否占编")]
            public short IS_AS { get; set; }
            /// <summary>
            /// 是否在职
            /// </summary>
            [Field(IsNotNull = true, DefaultValue = "0", Comment = "是否在职")]
            public short IS_USE { get; set; }
            /// <summary>
            /// 是否兼任
            /// </summary>
            [Field(IsNotNull = true, DefaultValue = "0", Comment = "是否兼任")]
            public short IS_CONCURREENT_POST { get; set; }
            /// <summary>
            /// 是否占职
            /// </summary>
            [Field(IsNotNull = true, DefaultValue = "0", Comment = "是否占职")]
            public short IS_ORG { get; set; }
            /// <summary>
            /// 是否预留
            /// </summary>
            [Field(IsNotNull = true, DefaultValue = "0", Comment = "是否预留")]
            public short IS_RESERVE { get; set; }

            /// <summary>
            /// 是否初始化
            /// </summary>
            [Field(IsNotNull = true, DefaultValue = "0", Comment = "是否初始化")]
            public short IS_INIT { get; set; }
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


        public IList<Entity> GetListEntityByUnitId(int unitId)
        {
            return GetList<Entity>(" UNIT_ID=?", new object[] { unitId });
        }

        public IList<Entity> GetListEntity()
        {
            return GetList<Entity>();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="isUse">是否在用</param>
        /// <param name="isOrg">是否占职</param>
        /// <param name="isAs">是否兼任</param>
        /// <param name="isCp">是否预留</param>
        /// <returns></returns>
        public short JudgeReserve(int isUse, int isOrg, int isAs, int isCp)
        {
            if (isUse == 1 && isOrg == 1 && isAs == 1 && isCp == 0) return 0;
            return 1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        public int Add(Entity entity)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("LEADER_TYPE_ID", entity.LEADER_TYPE_ID);
            dic.Add("LEADER_TYPE", entity.LEADER_TYPE);
            dic.Add("UNIT_ID", entity.UNIT_ID);
            dic.Add("UNIT_NAME", entity.UNIT_NAME);
            dic.Add("UNIT_PARENT_ID", entity.UNIT_PARENT_ID);
            dic.Add("UNIT_PARENT", entity.UNIT_PARENT);
            dic.Add("LAEDER_LEVEL_ID", entity.LAEDER_LEVEL_ID);
            dic.Add("LEADER_LEVEL", entity.LEADER_LEVEL);
            dic.Add("LEADER_JOB", entity.LEADER_JOB);
            dic.Add("LEADER_NAME", entity.LEADER_NAME);
            dic.Add("IS_AS", entity.IS_AS);
            //
            if (string.IsNullOrEmpty(entity.LEADER_NAME)) entity.IS_USE = 0;
            else entity.IS_USE = 1;
            //
            dic.Add("IS_USE", entity.IS_USE);
            dic.Add("IS_CONCURREENT_POST", entity.IS_CONCURREENT_POST);
            dic.Add("IS_ORG", entity.IS_ORG);
            //
            entity.IS_RESERVE = JudgeReserve(entity.IS_USE, entity.IS_ORG,entity.IS_AS, entity.IS_CONCURREENT_POST);
            //
            dic.Add("IS_RESERVE", entity.IS_RESERVE);
            dic.Add("IS_INIT", 0);
            dic.Add("CREATE_UID", SystemSession.UserID);
            dic.Add("UPDATE_UID", SystemSession.UserID);
            dic.Add("CREATE_TIME", DateTime.Now);
            dic.Add("UPDATE_TIME", DateTime.Now);
            return Add(dic);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int UpdateInit(Entity entity)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("LAEDER_LEVEL_ID", entity.LAEDER_LEVEL_ID);
            dic.Add("LEADER_LEVEL", entity.LEADER_LEVEL);
            dic.Add("LEADER_JOB", entity.LEADER_JOB);
            dic.Add("LEADER_NAME", entity.LEADER_NAME);
            dic.Add("IS_AS", entity.IS_AS);
            //
            if (string.IsNullOrEmpty(entity.LEADER_NAME)) entity.IS_USE = 0;
            else entity.IS_USE = 1;
            //
            dic.Add("IS_USE", entity.IS_USE);
            dic.Add("IS_CONCURREENT_POST", entity.IS_CONCURREENT_POST);
            dic.Add("IS_ORG", entity.IS_ORG);
            //
            entity.IS_RESERVE = JudgeReserve(entity.IS_USE, entity.IS_ORG, entity.IS_AS, entity.IS_CONCURREENT_POST);
            //
            dic.Add("IS_RESERVE", entity.IS_RESERVE);
            dic.Add("UPDATE_UID", SystemSession.UserID);
            dic.Add("UPDATE_TIME", DateTime.Now);
            return UpdateByKey(dic, entity.ID);
        }

        public int UpdateNotInit(Entity entity)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("LEADER_TYPE_ID", entity.LEADER_TYPE_ID);
            dic.Add("LEADER_TYPE", entity.LEADER_TYPE);
            dic.Add("LAEDER_LEVEL_ID", entity.LAEDER_LEVEL_ID);
            dic.Add("LEADER_LEVEL", entity.LEADER_LEVEL);
            dic.Add("LEADER_JOB", entity.LEADER_JOB);
            dic.Add("LEADER_NAME", entity.LEADER_NAME);
            dic.Add("IS_AS", entity.IS_AS);
            //
            if (string.IsNullOrEmpty(entity.LEADER_NAME)) entity.IS_USE = 0;
            else entity.IS_USE = 1;
            //
            dic.Add("IS_USE", entity.IS_USE);
            dic.Add("IS_CONCURREENT_POST", entity.IS_CONCURREENT_POST);
            dic.Add("IS_ORG", entity.IS_ORG);
            //
            entity.IS_RESERVE = JudgeReserve(entity.IS_USE, entity.IS_ORG, entity.IS_AS, entity.IS_CONCURREENT_POST);
            //
            dic.Add("IS_RESERVE", entity.IS_RESERVE);
            dic.Add("IS_INIT", 0);
            dic.Add("UPDATE_UID", SystemSession.UserID);
            dic.Add("UPDATE_TIME", DateTime.Now);
            return UpdateByKey(dic, entity.ID);
        }


        public string GetLeaderRemark(int unitid)
        {
            string sql = string.Format(@"
                  SELECT wm_concat (remark) AS remark
                    FROM (  SELECT unit_id,
                                      leader_type
                                   || '('
                                   || leader_level
                                   || ')'
                                   || COUNT (1)
                                   || '名'
                                      AS remark,
                                   leader_type_id,
                                   leader_level,
                                   COUNT (1) AS num
                              FROM ajtm_leader
                             WHERE is_use = 1 AND is_reserve = 1 and unit_id={0}
                          GROUP BY unit_id,
                                   leader_type,
                                   leader_type_id,
                                   leader_level)
                GROUP BY unit_id
            ", unitid);

            using (BDBHelper dbHelper = new BDBHelper())
            {
                return dbHelper.ExecuteScalarString(sql);
            }
        }
    }
}
