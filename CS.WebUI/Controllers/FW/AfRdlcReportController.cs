using CS.Base.Log;
using CS.Library.Export;
using CS.BLL.FW;
using CS.Common.FW;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Collections;
using CS.Base.DBHelper;

namespace CS.WebUI.Controllers.FW
{
    public class AfRdlcReportController : ABaseController
    {
        
        public ActionResult Index()
        {
            return View();
        }

        #region 增删改

        #region 新增或编辑rdlc报表
        /// <summary>
        /// 新增或编辑图表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(int id = 0)
        {
            ViewBag.Result = false;
            ViewBag.Message = string.Empty;
            ViewBag.DIC_DBS = BF_DATABASE.Instance.GetDictionary();

            BF_RDLC_REPORT.Entity entity = new BF_RDLC_REPORT.Entity();
            entity.IS_SHOW_EXPORT = 1;
            entity.IS_SHOW_DEBUG = 1;

            if (id > 0)
            {
                entity = BF_RDLC_REPORT.Instance.GetEntityByKey<BF_RDLC_REPORT.Entity>(id);
                if (entity == null)
                {
                    ViewBag.Message = "报表不存在";
                    entity = new BF_RDLC_REPORT.Entity();
                    entity.ID = -1;
                }
            }

            ModelState.Clear();
            return View(entity);
        }
        #endregion

        #region 编辑提交
        /// <summary>
        /// 编辑提交
        /// </summary>
        /// <param name="id"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(BF_RDLC_REPORT.Entity entity)
        {
            JsonResultData result = new JsonResultData();
            result.IsSuccess = false;
            int i = 0;
            try
            {

                CheckInput(entity);

                if (entity.ID < 0)
                {
                    result.Message = "rdlc报表不存在，不可编辑";
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                if (entity.ID == 0)
                {
                    entity.IS_ENABLE = 1;
                    entity.CREATE_UID = SystemSession.UserID;
                    entity.CREATE_TIME = DateTime.Now;
                    entity.UPDATE_UID = SystemSession.UserID;
                    entity.UPDATE_TIME = DateTime.Now;
                    i = BF_RDLC_REPORT.Instance.Add(entity);
                }
                else
                {
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    dic.Add("NAME", entity.NAME.Trim());
                    dic.Add("DB_ID", entity.DB_ID);
                    dic.Add("IS_SHOW_EXPORT", entity.IS_SHOW_EXPORT);
                    dic.Add("IS_SHOW_DEBUG", entity.IS_SHOW_DEBUG);
                    dic.Add("SQL_CODE", entity.SQL_CODE);
                    dic.Add("REMARK", entity.REMARK);
                    dic.Add("TOP_CODE", entity.TOP_CODE);
                    dic.Add("BOTTOM_CODE", entity.BOTTOM_CODE);
                    dic.Add("UPDATE_UID", SystemSession.UserID);
                    dic.Add("UPDATE_TIME", DateTime.Now);

                    dic.Add("RDLC_CODE", entity.RDLC_CODE);//RDLC代码体
                    i = BF_RDLC_REPORT.Instance.UpdateByKey(dic, entity.ID);
                }

                if (i < 1)
                {
                    result.Message = "提交失败！出现了未知错误";
                }
                else
                {
                    result.IsSuccess = true;
                    result.Message = "提交成功！";
                }


            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = ex.Message;
                BLog.Write(BLog.LogLevel.WARN, "编辑RDLC报表出错：" + ex.ToString());
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 删除
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(int id)
        {
            JsonResultData result = new JsonResultData();
            result.IsSuccess = false;
            result.Message = "参数有误！请联系管理员";
            if (id > 0)
            {
                result.IsSuccess = BLL.FW.BF_RDLC_REPORT.Instance.DeleteByKey(id) > 0;
                if (result.IsSuccess)
                    result.Message = "删除完成。";
                else
                    result.Message = "程序异常。删除失败！";
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        /// <summary>
        /// 解析sql返回解析字段信息
        /// </summary>
        /// <param name="dbid"></param>
        /// <param name="reportName"></param>
        /// <param name="sql"></param>
        /// <param name="inputjson"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AnalysisSql(int dbid = -1, string reportName = "暂无", string sql = "", string inputjson = "")
        {
            JsonResultData result = new JsonResultData();
            //SQL分析
            if (dbid >= 0 && string.IsNullOrWhiteSpace(sql) == false)
            {
                try
                {
                    sql = string.Format("select * from ({0}) where 1<>1", sql);
                    DataTable dt = BF_TB_REPORT.CheckSqlReturnDt(dbid, sql, inputjson);
                    var dic = GetDictionaryByTb(dt);//获得字段信息
                    if (dic != null && dic.Count > 0)
                    {
                        result.IsSuccess = true;
                        result.Message = "SQL分析成功，点击“配置”可进行高级配置";
                        result.Result = SerializeObject(dic);
                        //var xml= MakeRdlc(dic, reportName);
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.Message = "未获取到查询结构表信息";
                    }
                }
                catch (Exception e)
                {
                    result.IsSuccess = false;
                    result.Message = "分析出错：" + e.Message;
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 下载rdlc
        /// </summary>
        /// <param name="fields"></param>
        /// <param name="reportName"></param>
        /// <param name="rdlcId">ID编号(值为-1为新建报表)</param>
        /// <returns></returns>
        public ActionResult DownRDLC(string fields, string reportName, int rdlcId = -1)
        {
            string rdlcXml = "";//声明 
            if (rdlcId > 0)
            {
                var rdlc = BF_RDLC_REPORT.Instance.GetEntityByKey<BF_RDLC_REPORT.Entity>(rdlcId);
                rdlcXml = rdlc.RDLC_CODE;
            }

            if (!string.IsNullOrEmpty(fields) && fields != "[]")
            {
                var dic = DeserializeObject<Dictionary<string, string>>(fields);
                if (dic != null && dic.Count > 0)
                {
                    rdlcXml = MakeRdlc(dic, reportName);// 获取rdlc的xml
                }
                else
                {
                    return new EmptyResult();
                }
            }
            var tmp = Encoding.UTF8.GetBytes(rdlcXml);
            var fileName = (string.IsNullOrEmpty(reportName) ? "新建rdlc报表" : reportName) + DateTime.Now.ToString("yyyyMMdd") + ".rdlc";

            Response.Charset = "UTF-8";
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
            Response.ContentType = "application/octet-stream";
            //Response.AddHeader("Content-Disposition", "attachment; filename=" + Server.UrlEncode(fileName));
            Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);
            Response.BinaryWrite(tmp);
            Response.Flush();
            Response.End();

            return new EmptyResult();
        }
        
        #region 私有方法
        /// <summary>
        /// 验证输入
        /// </summary>
        /// <param name="entity"></param>
        private void CheckInput(BF_RDLC_REPORT.Entity entity)
        {
            if (entity == null)
            {
                throw new Exception("对象为空");
            }
            if (string.IsNullOrWhiteSpace(entity.NAME))
            {
                throw new Exception("名称不可为空");
            }
            if (string.IsNullOrWhiteSpace(entity.SQL_CODE))
            {
                throw new Exception("SQL语句不可为空");
            }
            if (string.IsNullOrWhiteSpace(entity.RDLC_CODE))
            {
                throw new Exception("RDLC代码不可为空");
            }

            if (BF_RDLC_REPORT.Instance.IsDuplicate(entity.ID, "NAME", entity.NAME))
            {
                throw new Exception("RDLC名称 " + entity.NAME + " 已经存在");
            }
        }
        /// <summary>
        /// 拼凑rdlc的xml内容体
        /// </summary>
        /// <param name="dic"></param>
        /// <param name="reportName"></param>
        /// <returns></returns>
        private string MakeRdlc(Dictionary<string, string> dic, string reportName)
        {
            string reStr = @"<?xml version=""1.0"" encoding=""utf-8""?>
<Report xmlns=""http://schemas.microsoft.com/sqlserver/reporting/2008/01/reportdefinition"" xmlns:rd=""http://schemas.microsoft.com/SQLServer/reporting/reportdesigner"">
  <Width>6.5in</Width>
  <Body>
    <Height>2in</Height>
  </Body>
  <rd:ReportTemplate>true</rd:ReportTemplate>
  <Page>
  </Page>
{0}
</Report>
";
            string dataStr = @"
<DataSources>
    <DataSource Name=""{1}"">
      <ConnectionProperties>
        <DataProvider>System.Data.DataSet</DataProvider>
        <ConnectString>/* Local Connection */</ConnectString>
      </ConnectionProperties>
      <rd:DataSourceID>{2}</rd:DataSourceID>
    </DataSource>
  </DataSources>
  <DataSets>
    <DataSet Name=""DataSet1"">
      <Query>
        <DataSourceName>{1}</DataSourceName>
        <CommandText>/* Local Query */</CommandText>
      </Query>
      <Fields>
{0}
      </Fields>
      <rd:DataSetInfo>
        <rd:DataSetName>{1}</rd:DataSetName>
        <rd:SchemaPath></rd:SchemaPath>
        <rd:TableName>DataTable1</rd:TableName>
        <rd:TableAdapterFillMethod />
        <rd:TableAdapterGetDataMethod />
        <rd:TableAdapterName />
      </rd:DataSetInfo>
    </DataSet>
  </DataSets>
";
            var fieldStr = @"
        <Field Name=""{0}"">
          <DataField>{1}</DataField>
          <rd:TypeName>{2}</rd:TypeName>
        </Field>
";
            var allFieldStr = "";
            foreach (var d in dic)
            {
                allFieldStr += string.Format(fieldStr, d.Key, d.Key, d.Value);
            }
            dataStr = string.Format(dataStr, allFieldStr, reportName + DateTime.Now.Ticks, new Guid().ToString());
            reStr = string.Format(reStr, dataStr);
            return reStr;
        }

        /// <summary>
        /// 返回dataTable的字段及类型信息
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private Dictionary<string, string> GetDictionaryByTb(DataTable dt)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            if (dt != null && dt.Columns.Count > 0)
            {
                //解析出字段名和字段类型
                foreach (DataColumn col in dt.Columns)
                {
                    dic.Add(col.ColumnName, col.DataType.Name);
                }
            }
            return dic;
        }
        #endregion
        #endregion

        #region 报表展示
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Show(int id)
        {
            if (id < 1)
            {
                return ShowAlert("请选择正确的报表");
            }
            string queryString = System.Web.HttpUtility.UrlDecode(Request.QueryString.ToString());
            if (string.IsNullOrWhiteSpace(queryString))
            {
                queryString = "[]";
            }
            else
            {
                var paraArr = queryString.Split(new char[] { '&', '?' }, StringSplitOptions.RemoveEmptyEntries);
                if (paraArr != null && paraArr.Count() > 0)
                {
                    queryString = SerializeObject(paraArr.Select(p =>
                    {
                        return new
                        {
                            Name = p.Substring(0, p.IndexOf("=")).Trim(),
                            Value = p.Substring((p.IndexOf("=") + 1)).Trim()
                        };
                    }));
                }
            }
            try
            {
                BF_RDLC_REPORT.Entity entity = BF_RDLC_REPORT.Instance.GetEntityByKey<BF_RDLC_REPORT.Entity>(id);
                if (entity == null)
                {
                    return ShowAlert("报表不存在");
                }
                if (entity.IS_ENABLE != 1)
                {
                    return ShowAlert("报表已被禁用");
                }
                ViewBag.QUERY_STRING = queryString;
                ViewBag.SQL_CODE = Base.Encrypt.BAES.Encrypt(entity.SQL_CODE);
                ViewBag.TOP_CODE = SystemSession.TransParams(Functions.Instance.ExecuteFunction(entity.TOP_CODE));
                ViewBag.BOTTOM_CODE = SystemSession.TransParams(Functions.Instance.ExecuteFunction(entity.BOTTOM_CODE));
                ViewBag.ID = entity.ID;
                return View();
            }
            catch (Exception ex)
            {
                BLog.Write(BLog.LogLevel.WARN, "查询报表[" + id + "]出现未知错误：" + ex.Message);
                return ShowAlert("查询报表出现未知错误：" + ex.Message);
            }
        }
        #endregion

        #region 以json串返回sql查询结果集(含内置参数的替换)
        /// <summary>
        /// 以json串返回sql查询结果集(含内置参数的替换)
        /// </summary>
        /// <param name="sqlE"></param>
        /// <returns></returns>
        public ActionResult GetDataBySql(Models.FW.SqlExcute sqlE)
        {
            BF_CHART_REPORT.Entity entity = new BF_CHART_REPORT.Entity();
            entity.ID = 0;
            entity.DB_ID = sqlE.DB_ID;
            entity.SQL_CODE = sqlE.SQL_CODE;

            List<object> paraList = new List<object>();

            string sql = BF_CHART_REPORT.GenerateSQL(entity, null, out paraList);
            try
            {
                DataTable dt = BF_CHART_REPORT.QueryTable(entity, sql, null, 0, 1);
                return Content(JSON.DecodeToStr(dt));
            }
            catch (Exception ex)
            {
                BLog.Write(BLog.LogLevel.ERROR, string.Format("sql查询结果集(含内置参数的替换)SQL为：{0}，数据异常为：{1}", sqlE.SQL_CODE, ex.Message));
                return null;
            }
        }
        #endregion

       

        #region 获取列表
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="id"></param>
        /// <returns>JSON数据</returns>
        public string GetList(int page, int limit, string name = "", string orderByField = "CR.ID", string orderByType = "DESC")
        {
            int count = 0;
            DataTable data = BF_RDLC_REPORT.Instance.GetDataTable(limit, page, ref count, name, orderByField, orderByType);
            return ToJsonString(data, count);
        }
        #endregion

        #region 导出文件附件         
        /// <summary>
        /// 导出文件附件
        /// </summary>
        /// <param name="wherestr">搜索条件</param>
        /// <returns>附件信息</returns>
        public ActionResult ExportFile(string name = "")
        {
            try
            {
                string filename = HttpUtility.UrlEncode(string.Format("报表{0}_{1}.xlsx",name, DateTime.Now.ToString("yyyyMMddHHmmss")), Encoding.UTF8);
                string path = System.Web.HttpContext.Current.Server.MapPath("~/tmp/");

                #region 添加参数
                string orderByField = "CR.ID";
                string orderByType = "DESC";
                List<object> param = new List<object>();
                string where = "1=1";
                if (string.IsNullOrWhiteSpace(name) == false)
                    where += " and CR.NAME LIKE '%" + name.Replace('\'', ' ') + "%'";
                #endregion
                int count = 0;
                DataTable dt = BF_RDLC_REPORT.Instance.GetDataTable(0, 0, ref count, name, orderByField, orderByType);
                if (dt == null)
                {
                    return ShowAlert("导出数据到Excel出现错误：未查询出数据");
                }
                dt.Columns.Remove("ID");
                dt.Columns["NAME"].Caption = "名称";
                dt.Columns["DBNAME"].Caption = "数据库名称";
                dt.Columns["CHART_TYPE"].Caption = "类别";
                dt.Columns["SHOWEXPORT"].Caption = "导出";
                dt.Columns["SHOWDEBUG"].Caption = "调试";
                dt.Columns["SQL_CODE"].Caption = "SQL";
                dt.Columns["IS_ENABLE"].Caption = "启用";
                dt.Columns["UPDATE_TIME"].Caption = "更新时间";

                Library.Export.ExcelFile export = new Library.Export.ExcelFile(path);
                string fullName = export.ToExcel(dt);
                if (string.IsNullOrWhiteSpace(fullName) == true)
                {
                    return ShowAlert("未生成Excel文件");
                }
                System.Web.HttpContext.Current.Response.Buffer = true;
                System.Web.HttpContext.Current.Response.Clear();//清除缓冲区所有内容
                System.Web.HttpContext.Current.Response.ContentType = "application/octet-stream";
                System.Web.HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + filename);
                System.Web.HttpContext.Current.Response.WriteFile(fullName);
                System.Web.HttpContext.Current.Response.Flush();
                System.Web.HttpContext.Current.Response.End();
                //删除文件
                export.Delete(fullName);
            }
            catch (Exception ex)
            {
                BLog.Write(BLog.LogLevel.WARN, "导出默认报表[DB]到Excel出错:" + ex.ToString());
                return ShowAlert("导出数据到Excel出现未知错误：" + ex.Message);
            }
            return ShowAlert("导出成功");
        }
        #endregion

    }
}