using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CS.BLL.FW;
using CS.BLL.Model;

namespace CS.WebUI.Controllers.AJTM
{
    public class AjtmAsDetailController : FW.ABaseController
    {
        // GET: AjtmAsDetail
        public ActionResult Edit(int id=0)
        {
            ViewBag.AsType = SerializeObject(AJTM_AS_TYPE.Instance.GetDropTree());
            ViewBag.Unit = SerializeObject(AJTM_UNIT.Instance.GetDropTree());
            ViewBag.AsPurpose = AJTM_AS_PURPOSE.Instance.GetDicDropDown();
            if (id > 0)
            {
                var r = AJTM_AS_DETAIL.Instance.GetEntityByKey<Model.AsDetail>(id);
                return View(r);
            }
            return View(new Model.AsDetail());
        }
        [HttpPost]
        public ActionResult Edit(Model.AsDetail entity)
        {
            JsonResultData result = new JsonResultData();

            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("APPROVAL_TIME", entity.APPROVAL_TIME);
            dic.Add("MEETING", entity.MEETING);
            dic.Add("UNIT_ID", entity.UNIT_ID);
            dic.Add("UNIT_NAME", entity.UNIT_NAME);
            dic.Add("UNIT_PARENT_ID", entity.UNIT_PARENT_ID);
            dic.Add("UNIT_PARENT", entity.UNIT_PARENT);
            dic.Add("AS_APPLY_ID", entity.AS_APPLY_ID);
            dic.Add("AS_APPLY_NO", entity.AS_APPLY_NO);
            dic.Add("AS_PURPOSE_ID", entity.AS_PURPOSE_ID);
            dic.Add("AS_PURPOSE", entity.AS_PURPOSE);
            dic.Add("AS_PURPOSE_REMARK", entity.AS_PURPOSE_REMARK);
            dic.Add("AS_TYPE_ID", entity.AS_TYPE_ID);
            dic.Add("AS_TYPE", entity.AS_TYPE);
            dic.Add("APPROVAL_NUM", entity.APPROVAL_NUM);
            if (entity.ID > 0)
            {
                AJTM_AS_DETAIL.Instance.Update(dic, " ID=?", new object[] { entity.ID });
            }
            else
            {
                dic.Add("CREATE_TIME", DateTime.Now);
                entity.ID = AJTM_AS_DETAIL.Instance.Add(dic, true);

                Dictionary<string, object> AsDS = new Dictionary<string, object>();
                AsDS.Add("AS_DETAIL_ID", entity.ID);
                AsDS.Add("AS_APPLY_NO", entity.AS_APPLY_NO);
                AsDS.Add("STATUS", ENUM_AS_DETAIL_STATUS.创建.ToString());
                AsDS.Add("STATUS_TIME", DateTime.Now);
                AsDS.Add("CREATE_UID", SystemSession.UserID);
                AsDS.Add("UPDATE_UID", SystemSession.UserID);
                AsDS.Add("CREATE_TIME", DateTime.Now);
                AsDS.Add("UPDATE_TIME", DateTime.Now);
                AJTM_AS_DETAIL_STATUS.Instance.Add(AsDS);
            }


            result.IsSuccess = true;
            result.Message = "数据提交成功";
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}


namespace CS.WebUI.Controllers.Model
{
    public class AsDetail : BLL.Model.AJTM_AS_DETAIL.Entity
    {

    }
}