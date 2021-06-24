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
    public class AJTM_AS_PERSONNEL : BBaseQuery
    {
        /// <summary>
        /// 单例
        /// </summary>
        public static AJTM_AS_PERSONNEL Instance = new AJTM_AS_PERSONNEL();
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public AJTM_AS_PERSONNEL()
        {
            this.IsAddIntoCache = true;
            this.TableName = "AJTM_AS_PERSONNEL";
            this.ItemName = "上下编明细表";
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
            /// 办理状态
            /// </summary>
            [Field(IsNotNull = false, Length = 128, Comment = "办理状态")]
            public string HANDLNG { get; set; }
            /// <summary>
            /// 增加减少审批单号
            /// </summary>
            [Field(IsNotNull = false, Length = 128, Comment = "增加减少审批单号")]
            public string ACTION_NO { get; set; }
            /// <summary>
            /// 上编/下编
            /// </summary>
            [Field(IsNotNull = false, Length = 128, Comment = "上编/下编")]
            public string ACTION { get; set; }
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
            /// 人员姓名
            /// </summary>
            [Field(IsNotNull = false, Length = 128, Comment = "人员姓名")]
            public string ACCOUNT_NAME { get; set; }
            /// <summary>
            /// 人员年龄
            /// </summary>
            [Field(IsNotNull = false, Length = 128, Comment = "人员年龄")]
            public string ACCOUNT_AGE { get; set; }
            /// <summary>
            /// 人员学历
            /// </summary>
            [Field(IsNotNull = false, Length = 128, Comment = "人员学历")]
            public string ACCOUNT_EDUCATION { get; set; }
            /// <summary>
            /// 岗位类别
            /// </summary>
            [Field(IsNotNull = false, Length = 128, Comment = "岗位类别")]
            public string POST_TYPE { get; set; }
            /// <summary>
            /// 用编类型ID
            /// </summary>
            [Field(IsNotNull = true, DefaultValue = "0", Comment = "用编类型ID")]
            public int AS_TYPE_ID { get; set; }
            /// <summary>
            /// 编制类型
            /// </summary>
            [Field(IsNotNull = true, Length = 128, Comment = "编制类型")]
            public string AS_TYPE { get; set; }
            /// <summary>
            /// 编制使用通知单号
            /// </summary>
            [Field(IsNotNull = true, Length = 128, Comment = "编制使用通知单号")]
            public string AS_APPLY_NO { get; set; }
            /// <summary>
            /// 进出方式ID
            /// </summary>
            [Field(IsNotNull = true, DefaultValue = "0", Comment = "进出方式ID")]
            public int ACCESS_MODE_ID { get; set; }
            /// <summary>
            /// 进出方式
            /// </summary>
            [Field(IsNotNull = true, Length = 128, Comment = "进出方式")]
            public string ACCESS_MODE { get; set; }
            /// <summary>
            /// 用编序号
            /// </summary>
            [Field(IsNotNull = true, Length = 128, Comment = "用编序号")]
            public string AS_NO { get; set; }
            /// <summary>
            /// 人员增减文件依据
            /// </summary>
            [Field(IsNotNull = true, Length = 128, Comment = "人员增减文件依据")]
            public string FILE_NAME { get; set; }
            /// <summary>
            /// 发文时间
            /// </summary>
            [Field(IsNotNull = true, DefaultValue = "NOW", Comment = "发文时间")]
            public DateTime FILE_SEND { get; set; }
            /// <summary>
            /// 人员来源
            /// </summary>
            [Field(IsNotNull = false, Length = 512, Comment = "人员来源")]
            public string ACCOUNT_SOURCE { get; set; }
            /// <summary>
            /// 人员增减情况
            /// </summary>
            [Field(IsNotNull = false, Length = 512, Comment = "人员增减情况")]
            public string ACCOUNT_SITUATION { get; set; }
            /// <summary>
            /// 同意上下编时间
            /// </summary>
            [Field(IsNotNull = true, DefaultValue = "NOW", Comment = "同意上下编时间")]
            public DateTime AGREE_TIME { get; set; }
            /// <summary>
            /// 实名制信息入库登记时
            /// </summary>
            [Field(IsNotNull = true, DefaultValue = "NOW", Comment = "实名制信息入库登记时")]
            public DateTime CHECKIN_TIME { get; set; }
            /// <summary>
            /// 去(来）向
            /// </summary>
            [Field(IsNotNull = false, Length = 512, Comment = "去(来）向")]
            public string ACCOUNT_REMARK { get; set; }
            /// <summary>
            /// 单位经办人
            /// </summary>
            [Field(IsNotNull = false, Length = 512, Comment = "单位经办人")]
            public string HANDLER { get; set; }
            /// <summary>
            /// 联系电话
            /// </summary>
            [Field(IsNotNull = false, Length = 512, Comment = "联系电话")]
            public string HANDLER_PHONE { get; set; }
            /// <summary>
            /// 上下编未完成原因
            /// </summary>
            [Field(IsNotNull = false, Length = 512, Comment = "上下编未完成原因")]
            public string REMARKS { get; set; }
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
        /// 办理状态
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> GetDropdownForHandlng()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("1.已登记", "1.已登记");
            dic.Add("2.已领取", "2.已领取");
            dic.Add("3.已交回", "3.已交回");
            dic.Add("4.已更新", "4.已更新");
            return dic;
        }

        /// <summary>
        /// 上/下编
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> GetDropdownForAction()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("上编", "上编");
            dic.Add("下编", "下编");
            return dic;
        }

        /// <summary>
        /// 学历
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> GetDropdownForEducation()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("初中", "初中");
            dic.Add("高中", "高中");
            dic.Add("中专", "中专");
            dic.Add("大专", "大专");
            dic.Add("大学", "大学");
            dic.Add("研究生", "研究生");
            dic.Add("博士", "博士");
            return dic;
        }

        /// <summary>
        /// 岗位类别
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> GetDropdownForPostType()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("专业技术", "专业技术");
            dic.Add("行政管理", "行政管理");
            dic.Add("工勤技能", "工勤技能");
            return dic;
        }


        /// <summary>
        /// 获取上编的用户信息
        /// </summary>
        /// <param name="AccountName"></param>
        /// <returns></returns>
        public DataTable GetTableByAccountName(string AccountName, int UnitId = 0)
        {
            DataTable dt = new DataTable();
            if (UnitId == 0)
            {
                dt = GetTable(new Order("ID", "DESC"), "ACCOUNT_NAME like '%"+ AccountName + "%' AND ACTION='上编'", new object[] { });
            }
            else
            {
                dt = GetTable(new Order("ID", "DESC"), "ACCOUNT_NAME like '%" + AccountName + "%' AND ACTION='上编' AND UNIT_ID=?", new object[] { UnitId });
            }
            return dt;
        }
    }

}
