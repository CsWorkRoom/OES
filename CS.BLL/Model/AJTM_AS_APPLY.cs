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
    public class AJTM_AS_APPLY : BBaseQuery
    {
        /// <summary>
        /// 单例
        /// </summary>
        public static AJTM_AS_APPLY Instance = new AJTM_AS_APPLY();
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public AJTM_AS_APPLY()
        {
            this.IsAddIntoCache = true;
            this.TableName = "AJTM_AS_APPLY";
            this.ItemName = "用编申请";
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
            /// 用编文件文号(编制使用通知单)
            /// </summary>
            [Field(IsNotNull = false, Length = 128, Comment = "用编文件文号(编制使用通知单)")]
            public string AS_APPLY_NO { get; set; }
            /// <summary>
            /// 用编通知数文件
            /// </summary>
            [Field(IsNotNull = false, Length = 128, Comment = "用编通知数文件")]
            public string AS_APPLY_PATH { get; set; }
            /// <summary>
            /// 用编通知数文件
            /// </summary>
            [Field(IsNotNull = false, Length = 128, Comment = "用编通知数文件")]
            public string AS_APPLY_PATH2 { get; set; }
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
            /// 来文文件名
            /// </summary>
            [Field(IsNotNull = true, Length = 128, Comment = "来文文件名")]
            public string APPLY_FILE { get; set; }
            /// <summary>
            /// 来文文件号
            /// </summary>
            [Field(IsNotNull = true, Length = 128, Comment = "来文文件名")]
            public string APPLY_FILE_NO { get; set; }
            /// <summary>
            /// 来文时间
            /// </summary>
            [Field(IsNotNull = true, DefaultValue = "NOW", Comment = "来文时间")]
            public DateTime APPLY_TIME { get; set; }
            /// <summary>
            /// 联系人
            /// </summary>
            [Field(IsNotNull = false, Length = 512, Comment = "联系人")]
            public string ACCOUNT_PEOPLE { get; set; }
            /// <summary>
            /// 联系方式
            /// </summary>
            [Field(IsNotNull = false, Length = 512, Comment = "联系方式")]
            public string ACCOUNT_PHONE { get; set; }

            /// <summary>
            /// 人员来源
            /// </summary>
            [Field(IsNotNull = true, DefaultValue = "0", Comment = "人员来源")]
            public int PERSONNER_SOURCE { get; set; }
            /// <summary>
            /// 是否为年度用编
            /// </summary>
            [Field(IsNotNull = true, DefaultValue = "0", Comment = "是否为年度用编")]
            public short IS_YEAR { get; set; }
            /// <summary>
            /// 申报总数
            /// </summary>
            [Field(IsNotNull = true, DefaultValue = "0", Comment = "申报总数")]
            public int APPLY_NUM { get; set; }
            /// <summary>
            /// 申报总数
            /// </summary>
            [Field(IsNotNull = true, DefaultValue = "0", Comment = "申报总数")]
            public int APPROVAL_NUM { get; set; }
            /// <summary>
            /// 状态
            /// </summary>
            [Field(IsNotNull = false, Length = 512, Comment = "状态")]
            public string STATUS { get; set; }
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
        public string GetApplyNo()
        {
            var index = GetCount(" TO_CHAR(CREATE_TIME,'YYYY')=? AND AS_APPLY_NO IS NOT NULL", new object[] { DateTime.Now.ToString("yyyy") });
            index += 1;
            return "达市编控〔" + DateTime.Now.ToString("yyyy") + "〕" + index + "号"; 
        }

        public string GetApplyNo(int i)
        {
            var index = GetCount(" TO_CHAR(CREATE_TIME,'YYYY')=? AND AS_APPLY_NO IS NOT NULL", new object[] { DateTime.Now.ToString("yyyy") });
            index = index + i + 1;
            return "达市编控〔" + DateTime.Now.ToString("yyyy") + "〕" + index + "号";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ApplyNo"></param>
        /// <returns></returns>
        public string[] analysisApplyNo(string ApplyNo)
        {
            string applyNo = ApplyNo;
            applyNo = applyNo.Replace("达市编控〔", "");
            applyNo = applyNo.Replace("号", "");

            return applyNo.Split('〕');
        }
        public List<string> GetApplyList()
        {
            var index = GetCount(" TO_CHAR(CREATE_TIME,'YYYY')=? AND AS_APPLY_NO IS NOT NULL", new object[] { DateTime.Now.ToString("yyyy") });
            index += 1;
            List<string> list = new List<string>();
            list.Add("达市编控〔" + DateTime.Now.ToString("yyyy") + "〕" + index + "号");
            list.Add(DateTime.Now.ToString("yyyy"));
            list.Add(index.ToString());
            return list;
        }
        public List<string> GetApplyList(int i)
        {
            var index = GetCount(" TO_CHAR(CREATE_TIME,'YYYY')=? AND AS_APPLY_NO IS NOT NULL", new object[] { DateTime.Now.ToString("yyyy") });
            index = index + i + 1;
            List<string> list = new List<string>();
            list.Add("达市编控〔" + DateTime.Now.ToString("yyyy") + "〕" + index + "号");
            list.Add(DateTime.Now.ToString("yyyy"));
            list.Add(index.ToString());
            return list;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public IList<Entity> GetApplyByIDS(string ids)
        {
            return GetList<Entity>(" ID IN (" + ids + ")", new object[] { });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ids"></param>

        public int UpdateStatusForApprovel(string ids)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("CREATE_UID", SystemSession.UserID);
            dic.Add("CREATE_TIME", DateTime.Now);
            dic.Add("STATUS", AS_APPLY_STATUS.审议.ToString());
            return Update(dic, " ID in (" + ids + ")", new object[] { });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IList<Entity> GetApplyData()
        {
            string sql = string.Format(@"
              SELECT  * FROM AJTM_AS_APPLY WHERE STATUS = '{0}'
            ", AS_APPLY_STATUS.申报.ToString());
            using (BDBHelper dbHelper = new BDBHelper())
            {
                var dt = dbHelper.ExecuteDataTable(sql);
                List<Entity> list = new List<Entity>();
                foreach(DataRow dr in dt.Rows)
                {
                    Entity entity = new Entity()
                    {
                        ID = Convert.ToInt32(dr["ID"]),
                        UNIT_NAME = dr["UNIT_NAME"].ToString(),
                        UNIT_PARENT = dr["UNIT_PARENT"].ToString(),
                        APPLY_TIME = Convert.ToDateTime(dr["APPLY_TIME"]),
                        APPLY_NUM =Convert.ToInt32(dr["APPLY_NUM"]),
                        IS_YEAR = Convert.ToInt16(dr["IS_YEAR"]),
                        ACCOUNT_PEOPLE = dr["ACCOUNT_PEOPLE"].ToString(),
                        ACCOUNT_PHONE = dr["ACCOUNT_PHONE"].ToString()
                    };
                    list.Add(entity);
                }
                return list;
            }
        }

        public string PATH_BASE = "../File/APPLYNO/";

        private string File = "temp.doc";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="year"></param>
        /// <param name="no"></param>
        /// <param name="unitParent"></param>
        /// <param name="unitName"></param>
        /// <param name="applyFile"></param>
        /// <param name="asApplyNo"></param>
        /// <param name="ApproveNum"></param>
        /// <param name="AsDeal"></param>
        /// <returns></returns>
        public string SaveApplyNoFile(string path, string unitParent, string unitName, string applyFile, string asApplyNo, int ApproveNum, DateTime ApproveTime, string AsDeal)
        {
            var r = analysisApplyNo(asApplyNo);

            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("<YEAR>", r[0]);
            dic.Add("<NO>", r[1]);
            dic.Add("<UNIT_PARENT>", unitParent);
            dic.Add("<UNIT_NAME>", unitName);
            dic.Add("<APPLY_FILE>", applyFile);
            dic.Add("<AS_APPLY_NO>", asApplyNo);
            dic.Add("<APPROVE_NUM>", ApproveNum.ToString());
            dic.Add("<DATETIME>", ApproveTime.ToString("yyyy年MM月dd"));
            dic.Add("<AS_DEAL>", AsDeal);

            Extension.Export.WordFile word = new Extension.Export.WordFile(path, File);
            word.ReplaceKeyword(dic);
            string filename = "temp_" + DateTime.Now.Ticks + ".doc";
            word.Save(path + filename);
            return PATH_BASE + filename;
        }
        private string FileList = "templist.doc";
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="year"></param>
        /// <param name="no"></param>
        /// <param name="unitParent"></param>
        /// <param name="unitName"></param>
        /// <param name="applyFile"></param>
        /// <param name="asApplyNo"></param>
        /// <param name="ApproveNum"></param>
        /// <param name="AsDeal"></param>
        /// <returns></returns>
        public string SaveApplyNoFileList(string path, string unitParent, string unitName, string applyFile, string asApplyNo, int ApproveNum, DateTime ApproveTime, string AsDeal,List<string> ApplyNO)
        {
            var r = analysisApplyNo(asApplyNo);

            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("<YEAR>", r[0]);
            dic.Add("<NO>", r[1]);
            dic.Add("<UNIT_PARENT>", unitParent);
            dic.Add("<UNIT_NAME>", unitName);
            dic.Add("<APPLY_FILE>", applyFile);
            dic.Add("<AS_APPLY_NO>", asApplyNo);
            dic.Add("<APPROVE_NUM>", ApproveNum.ToString());
            dic.Add("<DATETIME>", ApproveTime.ToString("yyyy年MM月dd"));
            dic.Add("<AS_DEAL>", AsDeal);

            DataTable dt = new DataTable();
            dt.Columns.Add("序号");
            dt.Columns.Add("姓名");
            dt.Columns.Add("编号");

            for (var i = 0; i < ApplyNO.Count; i++)
            {
                DataRow dr = dt.NewRow();
                dr[0] = i + 1;
                dr[1] = " ";
                dr[2] = ApplyNO[i];
                dt.Rows.Add(dr);
            }

            Extension.Export.WordFile word = new Extension.Export.WordFile(path, FileList);
            word.ReplaceKeyword(dic);
            word.AddTableForTable(dt);
            string filename = "templist_" + DateTime.Now.Ticks + ".doc"; ;
            word.Save(path + filename);
            return PATH_BASE + filename;
        }
    }
    public enum AS_APPLY_STATUS
    {
        申报,
        审议,
        完成,
        撤销
    }
}
