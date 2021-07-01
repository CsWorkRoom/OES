using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CS.BLL.FW;
using CS.BLL.Model;

namespace CS.WebUI.Controllers.AJTM
{
    public class AjtmAsApplyController : FW.ABaseController
    {
        // GET: AjtmAsApply
        public ActionResult Edit(int id = 0)
        {
            ViewBag.AsPurpose = SerializeObject(AJTM_AS_PURPOSE.Instance.GetDropDown());
            ViewBag.AsType = SerializeObject(AJTM_AS_TYPE.Instance.GetDropTree());
            ViewBag.Unit = SerializeObject(AJTM_UNIT.Instance.GetDropTree());
            if (id > 0)
            {
                var r = AJTM_AS_APPLY.Instance.GetEntityByKey<BLL.Model.AJTM_AS_APPLY.Entity>(id);
                var model = Common.Fun.ClassToCopy<BLL.Model.AJTM_AS_APPLY.Entity, Model.AsApply>(r);
                model.AsApplyDetailJson = SerializeObject(AJTM_AS_APPLY_DETAIL.Instance.GetTableByApplyId(model.ID));
                return View(model);
            }
            var m = new Model.AsApply() { APPLY_TIME = DateTime.Now };
            return View(m);
        }
        /// <summary>
        /// 编辑提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(Controllers.Model.AsApply entity)
        {
            JsonResultData result = new JsonResultData();
            if (entity.UNIT_ID == 0)
            {
                result.IsSuccess = false;
                result.Message = "提交失败,请选择用编单位";
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(entity.AsApplyDetailJson))
            {
                result.IsSuccess = false;
                result.Message = "提交失败,用编明细缺失";
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            var AsApplyDetail = DeserializeObject<List<AJTM_AS_APPLY_DETAIL.Entity>>(entity.AsApplyDetailJson);
            if(AsApplyDetail.Count == 0)
            {
                result.IsSuccess = false;
                result.Message = "提交失败,用编明细缺失";
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("UNIT_ID", entity.UNIT_ID);
            dic.Add("UNIT_NAME", entity.UNIT_NAME);
            dic.Add("UNIT_PARENT_ID", entity.UNIT_PARENT_ID);
            dic.Add("UNIT_PARENT", entity.UNIT_PARENT);
            dic.Add("APPLY_FILE", entity.APPLY_FILE);
            dic.Add("APPLY_TIME", entity.APPLY_TIME);
            dic.Add("ACCOUNT_PEOPLE", entity.ACCOUNT_PEOPLE);
            dic.Add("ACCOUNT_PHONE", entity.ACCOUNT_PHONE);
            dic.Add("IS_YEAR", entity.IS_YEAR);
            dic.Add("APPLY_NUM", AsApplyDetail.Sum(x => x.APPLY_NUM));
            dic.Add("APPROVAL_NUM", 0);
            dic.Add("STATUS", BLL.Model.AS_APPLY_STATUS.申报.ToString());
            if (entity.ID > 0)
            {
                dic.Add("UPDATE_UID", SystemSession.UserID);
                dic.Add("UPDATE_TIME", DateTime.Now);
                AJTM_AS_APPLY.Instance.Update(dic, " ID=?", new object[] { entity.ID });
                AJTM_AS_APPLY_DETAIL.Instance.Delete(" AS_APPLY_ID=?", new object[] { entity.ID });
            }
            else
            {
                dic.Add("CREATE_UID", SystemSession.UserID);
                dic.Add("UPDATE_UID", SystemSession.UserID);
                dic.Add("CREATE_TIME", DateTime.Now);
                dic.Add("UPDATE_TIME", DateTime.Now);
                try
                {
                    entity.ID = AJTM_AS_APPLY.Instance.Add(dic, true);
                }
                catch (Exception ex)
                {
                    result.IsSuccess = false;
                    result.Message = "提交失败,信息异常:" + ex.ToString();
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (entity.ID == 0)
                {
                    result.IsSuccess = false;
                    result.Message = "提交失败,信息异常";
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }

            foreach (var item in AsApplyDetail)
            {
                Dictionary<string, object> AsApplyDetailItem = new Dictionary<string, object>();
                AsApplyDetailItem.Add("AS_APPLY_ID", entity.ID);
                AsApplyDetailItem.Add("AS_TYPE_ID", item.AS_TYPE_ID);
                AsApplyDetailItem.Add("AS_PURPOSE_ID", item.AS_PURPOSE_ID);
                AsApplyDetailItem.Add("AS_PURPOSE_REMARK", item.AS_PURPOSE_REMARK);
                AsApplyDetailItem.Add("APPLY_NUM", item.APPLY_NUM);
                AsApplyDetailItem.Add("APPROVAL_NUM", 0);
                AJTM_AS_APPLY_DETAIL.Instance.Add(AsApplyDetailItem);
            }

            result.IsSuccess = true;
            result.Message = "数据提交成功";
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 批准
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Approval(int id = 0)
        {
            ViewBag.AsPurpose = SerializeObject(AJTM_AS_PURPOSE.Instance.GetDropDown());
            ViewBag.AsType = SerializeObject(AJTM_AS_TYPE.Instance.GetDropTree());
            if (id > 0)
            {
                var r = AJTM_AS_APPLY.Instance.GetEntityByKey<BLL.Model.AJTM_AS_APPLY.Entity>(id);
                var model = Common.Fun.ClassToCopy<BLL.Model.AJTM_AS_APPLY.Entity, Model.AsApply>(r);
                model.AS_APPLY_NO = AJTM_AS_APPLY.Instance.GetApplyNo();
                model.AsApplyDetailJson = SerializeObject(AJTM_AS_APPLY_DETAIL.Instance.GetTableByApplyId(model.ID));
                return View(model);
            }
            return View(new Model.AsApply());
        }
        /// <summary>
        /// 批准提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Approval(Controllers.Model.AsApply entity)
        {
            JsonResultData result = new JsonResultData();
            var AsApplyDetail = DeserializeObject<List<AJTM_AS_APPLY_DETAIL.Entity>>(entity.AsApplyDetailJson);
            if (AsApplyDetail.Count == 0)
            {
                result.IsSuccess = false;
                result.Message = "提交失败,用编明细缺失";
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            if(entity.ID == 0)
            {
                result.IsSuccess = false;
                result.Message = "用编申报不存在";
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            var Apply = AJTM_AS_APPLY.Instance.GetEntityByKey<AJTM_AS_APPLY.Entity>(entity.ID);
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
            //
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("AS_APPLY_NO", entity.AS_APPLY_NO);
            dic.Add("APPROVAL_NUM", APPROVAL_NUM);
            dic.Add("UPDATE_UID", SystemSession.UserID);
            dic.Add("UPDATE_TIME", DateTime.Now);
            //
            AJTM_AS_APPLY.Instance.Update(dic, " ID=?", new object[] { entity.ID });
            var CNo = AJTM_AS_DETAIL.Instance.GetCurrentNo();
            var j = 1;
            //
            foreach (var item in AsApplyDetail)
            {
                Dictionary<string, object> AsApplyDetailItem = new Dictionary<string, object>();
                AsApplyDetailItem.Add("AS_TYPE_ID", item.AS_TYPE_ID);
                AsApplyDetailItem.Add("AS_PURPOSE_ID", item.AS_PURPOSE_ID);
                AsApplyDetailItem.Add("AS_PURPOSE_REMARK", item.AS_PURPOSE_REMARK);
                AsApplyDetailItem.Add("APPROVAL_NUM", item.APPROVAL_NUM);
                if (item.ID == 0)
                {
                    AsApplyDetailItem.Add("AS_APPLY_ID", entity.ID);
                    AsApplyDetailItem.Add("APPLY_NUM", 0);
                    AJTM_AS_APPLY_DETAIL.Instance.Add(AsApplyDetailItem);
                }
                else
                {
                    AJTM_AS_APPLY_DETAIL.Instance.Update(AsApplyDetailItem, " ID=?", item.ID);
                }

                for(int i = 0; i < item.APPROVAL_NUM; i++)
                {
                    Dictionary<string, object> AsD = new Dictionary<string, object>();
                    AsD.Add("APPROVAL_TIME", entity.AS_APPROVAL_TIME);
                    AsD.Add("MEETING", entity.MEETING);
                    AsD.Add("UNIT_ID", Apply.UNIT_ID);
                    AsD.Add("UNIT_NAME", Apply.UNIT_NAME);
                    AsD.Add("UNIT_PARENT_ID", Apply.UNIT_PARENT_ID);
                    AsD.Add("UNIT_PARENT", Apply.UNIT_PARENT);
                    AsD.Add("AS_APPLY_ID", Apply.ID);
                    AsD.Add("AS_APPLY_NO", entity.AS_APPLY_NO);
                    AsD.Add("AS_PURPOSE_ID", item.AS_PURPOSE_ID);
                    AsD.Add("AS_TYPE_ID", item.AS_TYPE_ID);
                    AsD.Add("AS_PURPOSE_REMARK", item.AS_PURPOSE_REMARK);
                    AsD.Add("AS_NO", AJTM_AS_DETAIL.Instance.GetAsNo(i * j + CNo));
                    AsD.Add("APPROVAL_NUM", 1);
                    AsD.Add("CREATE_TIME", DateTime.Now);

                    int AsDetailId = AJTM_AS_DETAIL.Instance.Add(AsD, true);

                    Dictionary<string, object> AsDS = new Dictionary<string, object>();
                    AsDS.Add("AS_DETAIL_ID", AsDetailId);
                    AsDS.Add("AS_APPLY_ID", Apply.ID);
                    AsDS.Add("AS_APPLY_NO", entity.AS_APPLY_NO);
                    AsDS.Add("STATUS", ENUM_AS_DETAIL_STATUS.创建.ToString());
                    AsDS.Add("STATUS_TIME", DateTime.Now);
                    AsDS.Add("CREATE_UID", SystemSession.UserID);
                    AsDS.Add("UPDATE_UID", SystemSession.UserID);
                    AsDS.Add("CREATE_TIME", DateTime.Now);
                    AsDS.Add("UPDATE_TIME", DateTime.Now);

                    AJTM_AS_DETAIL_STATUS.Instance.Add(AsDS);
                }
                
            }
            result.IsSuccess = true;
            result.Message = "数据提交成功";
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 编制使用通知单
        /// </summary>
        /// <param name="UnitId"></param>
        /// <returns></returns>
        public string GetApplyNo(int UnitId=0)
        {
            if (UnitId > 0)
            {
                var r = AJTM_AS_APPLY.Instance.GetTableFields("AS_APPLY_NO,ID", "UNIT_ID=? AND APPROVAL_NUM > 0", new object[] { UnitId });
                return SerializeObject(r);
            }
            return "{}";
        }
    }
}


namespace CS.WebUI.Controllers.Model
{
    public class AsApply : BLL.Model.AJTM_AS_APPLY.Entity
    {
        public string AsApplyDetailJson { get; set; }
        /// <summary>
        /// 会议名称
        /// </summary>
        public string MEETING { get; set; }
        /// <summary>
        /// 批复时间
        /// </summary>
        public DateTime AS_APPROVAL_TIME { get; set; }
    }
}