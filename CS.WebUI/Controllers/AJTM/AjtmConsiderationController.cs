using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using CS.BLL.Extension.Export;
using CS.BLL.FW;
using CS.BLL.Model;

namespace CS.WebUI.Controllers.AJTM
{
    public class AjtmConsiderationController : FW.ABaseController
    {
        // GET: AjtmConsideration
        public ActionResult Index()
        {
            ViewBag.NAME = BLL.Model.AJTM_CONSIDERATION.Instance.GetConsiderationName();
            ViewBag.AsApply = BLL.Model.AJTM_AS_APPLY.Instance.GetApplyData();
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
            if (dt.Rows.Count == 0)
            {
                result.IsSuccess = false;
                result.Message = "未选中用申报,请选择中后再进行生成审议表";
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            Dictionary<string, string> filterDic = new Dictionary<string, string>();
            if (xTime != null)
                filterDic.Add("<X_TIME>", xTime.Value.ToString("yyyy年MM月dd日"));
            else
                filterDic.Add("<X_TIME>", "   年  月  日");
      
            filterDic.Add("<X_APPROVE>", xApprove);
            filterDic.Add("<X_DEAL>", xDeal);
            //新增审议表
            string path = Server.MapPath(BLL.Model.AJTM_CONSIDERATION.Instance.PATH_BASE);
            ExcelFile file = new ExcelFile(path);
            string filename = file.ToRepalceExcel(BLL.Model.AJTM_CONSIDERATION.Instance.PATH_FILENAME, dt, filterDic);
            //
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("NAME", NAME);
            dic.Add("IDS", IDS);
            dic.Add("PATH", BLL.Model.AJTM_CONSIDERATION.Instance.PATH_BASE + filename);
            dic.Add("STATUS", BLL.Model.CONSIDERATION_STATUS.创建.ToString());
            dic.Add("CREATE_UID", SystemSession.UserID);
            dic.Add("UPDATE_UID", SystemSession.UserID);
            dic.Add("CREATE_TIME", DateTime.Now);
            dic.Add("UPDATE_TIME", DateTime.Now);
            //
            var id = BLL.Model.AJTM_CONSIDERATION.Instance.Add(dic, true);
            //新增审议表详细
            var idArray = IDS.Split(',').Select(x => Convert.ToInt32(x)).ToList();
            foreach(var item in idArray)
            {
                Dictionary<string, object> dicConAs = new Dictionary<string, object>();
                dicConAs.Add("CONSIDERATION_ID", id);
                dicConAs.Add("AS_APPLY_ID", item);
                //
                BLL.Model.AJTM_CONSIDERATION_APPLY.Instance.Add(dicConAs);
            }
            //修改状态
            BLL.Model.AJTM_AS_APPLY.Instance.UpdateStatusForApprovel(IDS);
            result.IsSuccess = true;
            result.Message = "数据提交成功,请在《用编申报批准》模块进行下载审议表";
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public ActionResult Approvel(int id = 0)
        {
            ViewBag.AsPurpose = SerializeObject(AJTM_AS_PURPOSE.Instance.GetDropDown());
            ViewBag.AsType = SerializeObject(AJTM_AS_TYPE.Instance.GetDropTree());
            ViewBag.AsApplyList = "";
            ViewBag.ID = id;
            if (id > 0)
            {
                string ids = AJTM_CONSIDERATION.Instance.GetIdsById(id);
                if (!string.IsNullOrEmpty(ids))
                {
                    var Apply = AJTM_AS_APPLY.Instance.GetApplyByIDS(ids);
                    var ApplyDetail = AJTM_AS_APPLY_DETAIL.Instance.GetApplyDetailByIDS(ids);
                    if (ApplyDetail != null)
                    {
                        List<Model.AsApply> AsApplyList = new List<Model.AsApply>();
                        int i = 0;
                        foreach (var item in Apply)
                        {
                            var AsApply = CS.Common.Fun.ClassToCopy<AJTM_AS_APPLY.Entity, Model.AsApply>(item);
                            AsApply.AS_APPLY_NO = BLL.Model.AJTM_AS_APPLY.Instance.GetApplyNo(i);
                            AsApply.AsApplyDetailJson = SerializeObject(ApplyDetail.Where(x => x.AS_APPLY_ID == AsApply.ID).ToList());
                            AsApplyList.Add(AsApply);
                            i++;
                        }
                        ViewBag.AsApplyList = SerializeObject(AsApplyList);
                    }

                }
            }
            return View();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="AS_APPROVAL_TIME"></param>
        /// <param name="MEETING"></param>
        /// <param name="AsApply"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Approvel(int ID,DateTime AS_APPROVAL_TIME,string MEETING,string AsApply)
        {
            JsonResultData result = new JsonResultData();
            if (ID == 0)
            {
                result.IsSuccess = false;
                result.Message = "提交失败,该审议表不存在";
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            //校验
            List<Model.AsApply> AsApplyArr = DeserializeObject<List<Model.AsApply>>(AsApply);
            foreach(var AsA in AsApplyArr)
            {
                var AsApplyDetail = DeserializeObject<List<AJTM_AS_APPLY_DETAIL.Entity>>(AsA.AsApplyDetailJson);
                if (AsApplyDetail.Count == 0)
                {
                    result.IsSuccess = false;
                    result.Message = "提交失败,用编明细缺失";
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (AsA.ID == 0)
                {
                    result.IsSuccess = false;
                    result.Message = "用编申报不存在";
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                var Apply = AJTM_AS_APPLY.Instance.GetEntityByKey<AJTM_AS_APPLY.Entity>(AsA.ID);
                if (Apply.ID == 0)
                {
                    result.IsSuccess = false;
                    result.Message = "用编申报不存在";
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                int APPROVAL_NUM = AsApplyDetail.Sum(x => x.APPROVAL_NUM);
                if (Apply.APPLY_NUM < APPROVAL_NUM)
                {
                    result.IsSuccess = false;
                    result.Message = "批准数不能大于申请数";
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            //获取编制用途列表,编制类型
            var AsPurse = BLL.Model.AJTM_AS_PURPOSE.Instance.GetDicDropDown();
            var AsType = BLL.Model.AJTM_AS_TYPE.Instance.GetDropDown();
           
            foreach (var AsA in AsApplyArr)
            {

                //解析申报明细,用编申报信息
                var AsApplyDetail = DeserializeObject<List<AJTM_AS_APPLY_DETAIL.Entity>>(AsA.AsApplyDetailJson);
                var Apply = AJTM_AS_APPLY.Instance.GetEntityByKey<AJTM_AS_APPLY.Entity>(AsA.ID);
                //获取当年序列号
                var CNo = AJTM_AS_DETAIL.Instance.GetCurrentNo();
                //编制号码
                List<string> AsNoList = new List<string>();
                List<string> AsPurseList = new List<string>();
                //申报明细序列号，用于计算用编需要清单
                var j = 1;
                //
                foreach (var item in AsApplyDetail)
                {
                    var Purpose = AsPurse[item.AS_PURPOSE_ID];
                    AsPurseList.Add(Purpose);
                    //
                    Dictionary<string, object> AsApplyDetailItem = new Dictionary<string, object>();
                    AsApplyDetailItem.Add("AS_TYPE_ID", item.AS_TYPE_ID);
                    AsApplyDetailItem.Add("AS_PURPOSE_ID", item.AS_PURPOSE_ID);
                    AsApplyDetailItem.Add("AS_PURPOSE_REMARK", item.AS_PURPOSE_REMARK);
                    AsApplyDetailItem.Add("APPROVAL_NUM", item.APPROVAL_NUM);
                    if (item.ID == 0)
                    {
                        AsApplyDetailItem.Add("AS_APPLY_ID", AsA.ID);
                        AsApplyDetailItem.Add("APPLY_NUM", 0);
                        AJTM_AS_APPLY_DETAIL.Instance.Add(AsApplyDetailItem);
                    }
                    else
                    {
                        AJTM_AS_APPLY_DETAIL.Instance.Update(AsApplyDetailItem, " ID=?", item.ID);
                    }
                   
                    for (int i = 0; i < item.APPROVAL_NUM; i++)
                    {
                        //string ASNO = AJTM_AS_DETAIL.Instance.GetAsNo(i * j + CNo);

                        string ASNO = AJTM_AS_DETAIL.Instance.GetAsNo();
                        Dictionary<string, object> AsD = new Dictionary<string, object>();

                        AsD.Add("APPROVAL_TIME", AS_APPROVAL_TIME);
                        AsD.Add("MEETING", MEETING);
                        AsD.Add("UNIT_ID", Apply.UNIT_ID);
                        AsD.Add("UNIT_NAME", Apply.UNIT_NAME);
                        AsD.Add("UNIT_PARENT_ID", Apply.UNIT_PARENT_ID);
                        AsD.Add("UNIT_PARENT", Apply.UNIT_PARENT);
                        AsD.Add("AS_APPLY_ID", Apply.ID);
                        AsD.Add("AS_APPLY_NO", AsA.AS_APPLY_NO);
                        AsD.Add("AS_PURPOSE_ID", item.AS_PURPOSE_ID);
                        AsD.Add("AS_PURPOSE", Purpose);
                        AsD.Add("AS_TYPE_ID", item.AS_TYPE_ID);
                        AsD.Add("AS_TYPE", AsType[item.AS_TYPE_ID]);
                        AsD.Add("AS_PURPOSE_REMARK", item.AS_PURPOSE_REMARK);
                        AsD.Add("AS_NO", ASNO);
                        AsD.Add("APPROVAL_NUM", 1);
                        AsD.Add("CREATE_TIME", DateTime.Now);

                        int AsDetailId = AJTM_AS_DETAIL.Instance.Add(AsD, true);

                        Dictionary<string, object> AsDS = new Dictionary<string, object>();
                        AsDS.Add("AS_DETAIL_ID", AsDetailId);
                        AsDS.Add("AS_APPLY_ID", Apply.ID);
                        AsDS.Add("AS_APPLY_NO", AsA.AS_APPLY_NO);
                        AsDS.Add("STATUS", ENUM_AS_DETAIL_STATUS.创建.ToString());
                        AsDS.Add("STATUS_TIME", DateTime.Now);
                        AsDS.Add("CREATE_UID", SystemSession.UserID);
                        AsDS.Add("UPDATE_UID", SystemSession.UserID);
                        AsDS.Add("CREATE_TIME", DateTime.Now);
                        AsDS.Add("UPDATE_TIME", DateTime.Now);

                        AJTM_AS_DETAIL_STATUS.Instance.Add(AsDS);
                        //
                        AsNoList.Add(ASNO);
                    }
                    
                    j++;
                }
                //参数-批准数,编制用途，根目录
                int APPROVAL_NUM = AsApplyDetail.Sum(x => x.APPROVAL_NUM);
                string AsPurseString = string.Join("/", AsPurseList);
                string basePath = Server.MapPath(AJTM_AS_APPLY.Instance.PATH_BASE);
                //生成通知表
                string file1 = AJTM_AS_APPLY.Instance.SaveApplyNoFile(basePath, AsA.UNIT_PARENT, AsA.UNIT_NAME, AsA.APPLY_FILE, AsA.AS_APPLY_NO, APPROVAL_NUM, AS_APPROVAL_TIME, AsPurseString);
                string file2 = AJTM_AS_APPLY.Instance.SaveApplyNoFileList(basePath, AsA.UNIT_PARENT, AsA.UNIT_NAME, AsA.APPLY_FILE, AsA.AS_APPLY_NO, APPROVAL_NUM, AS_APPROVAL_TIME, AsPurseString, AsNoList);

                //修改申请表
                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic.Add("AS_APPLY_NO", AsA.AS_APPLY_NO);
                dic.Add("APPROVAL_NUM", APPROVAL_NUM);
                dic.Add("AS_APPLY_PATH", file1);
                dic.Add("AS_APPLY_PATH2", file2);
                dic.Add("STATUS", AS_APPLY_STATUS.完成.ToString());
                dic.Add("UPDATE_UID", SystemSession.UserID);
                dic.Add("UPDATE_TIME", DateTime.Now);
                AJTM_AS_APPLY.Instance.UpdateByKey(dic, AsA.ID);
            }

            //修改审议
            Dictionary<string, object> dicc = new Dictionary<string, object>();
            dicc.Add("STATUS", BLL.Model.CONSIDERATION_STATUS.审议完成.ToString());
            BLL.Model.AJTM_CONSIDERATION.Instance.UpdateByKey(dicc, ID);

            result.IsSuccess = true;
            result.Message = "数据提交成功";
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 导出审议表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ExportFile(int id)
        {

            try
            {
                var model = BLL.Model.AJTM_CONSIDERATION.Instance.GetEntityByKey<AJTM_CONSIDERATION.Entity>(id);
                if (model.ID > 0)
                {
                    string filename = HttpUtility.UrlEncode(string.Format("{1}_{0}.xlsx", DateTime.Now.ToString("yyyyMMddHHmmss"), model.NAME), Encoding.UTF8);
                    string fullName = Server.MapPath(model.PATH);
                    System.Web.HttpContext.Current.Response.Buffer = true;
                    System.Web.HttpContext.Current.Response.Clear();//清除缓冲区所有内容
                    System.Web.HttpContext.Current.Response.ContentType = "application/octet-stream";
                    System.Web.HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + filename);
                    System.Web.HttpContext.Current.Response.WriteFile(fullName);
                    System.Web.HttpContext.Current.Response.Flush();
                    System.Web.HttpContext.Current.Response.End();
                }
                else
                {
                    return ShowAlert("导出失败！未找到该工单");
                }
            }
            catch (Exception ex)
            {
                return ShowAlert("导出数据到Excel出现未知错误：" + ex.Message);
            }
            //
            return ShowAlert("导出成功！");
        }
    }
}