using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CS.BLL.Extension.Export;
using CS.BLL.FW;

namespace CS.WebUI.Controllers.AJTM
{
    public class AjtmConsiderationController : Controller
    {
        // GET: AjtmConsideration
        public ActionResult Index(string IDS)
        {
            ViewBag.IDS = IDS;
            ViewBag.NAME = BLL.Model.AJTM_CONSIDERATION.Instance.GetConsiderationName();
            return View();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IDS"></param>
        /// <param name="NAME"></param>
        /// <param name="xTime"></param>
        /// <param name="xApprove"></param>
        /// <param name="xDeal"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Index(string IDS, string NAME, DateTime? xTime, string xApprove = "", string xDeal = "")
        {
            JsonResultData result = new JsonResultData();
            if (string.IsNullOrEmpty(IDS))
            {
                result.IsSuccess = false;
                result.Message = "未选中用申报,请选择中后再进行生成审议表";
                return Json(result, JsonRequestBehavior.AllowGet);

            }
            if (string.IsNullOrEmpty(NAME))
            {
                result.IsSuccess = false;
                result.Message = "请输入审议表名称,作为后期查看的重要依据";
                return Json(result, JsonRequestBehavior.AllowGet);

            }
            DataTable dt = BLL.Model.AJTM_CONSIDERATION.Instance.GetApplyConsideration(IDS);
            if (dt.Rows.Count === 0)
            {
                result.IsSuccess = false;
                result.Message = "未选中用申报,请选择中后再进行生成审议表";
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            Dictionary<string, string> filterDic = new Dictionary<string, string>();
            if (xTime != null)
                filterDic.Add("<X_TIME>", xTime.Value.ToString("yyyy年MM月dd日");
            else
                filterDic.Add("<X_TIME>", "   年  月  日");
            if (string.IsNullOrEmpty(xApprove))
                filterDic.Add("<X_APPROVE>", xApprove);
            if (string.IsNullOrEmpty(xDeal))
                filterDic.Add("<X_DEAL>", xDeal);
            //
            string path = Server.MapPath(BLL.Model.AJTM_CONSIDERATION.Instance.PATH_BASE);
            ExcelFile file = new ExcelFile(path);
            string filename = file.ToRepalceExcel(BLL.Model.AJTM_CONSIDERATION.Instance.PATH_FILENAME, dt, filterDic);
            //
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("NAME", NAME);
            dic.Add("IDS", IDS);
            dic.Add("PATH", BLL.Model.AJTM_CONSIDERATION.Instance.PATH_BASE + filename);
            dic.Add("CREATE_UID", SystemSession.UserID);
            dic.Add("UPDATE_UID", SystemSession.UserID);
            dic.Add("CREATE_TIME", DateTime.Now);
            dic.Add("UPDATE_TIME", DateTime.Now);
            //
            var id = BLL.Model.AJTM_CONSIDERATION.Instance.Add(dic, true);
            //
            var idArray = IDS.Split(',').Select(x => Convert.ToInt32(x)).ToList();
            foreach(var item in idArray)
            {
                Dictionary<string, object> dicConAs = new Dictionary<string, object>();
                dicConAs.Add("CONSIDERATION_ID", id);
                dicConAs.Add("AS_APPLY_ID", item);
                //
                BLL.Model.AJTM_CONSIDERATION_APPLY.Instance.Add(dicConAs);
            }
            result.IsSuccess = true;
            result.Message = "数据提交成功,请在《用编申报批准》模块进行下载审议表";
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}