using System;
using System.Collections.Generic;
using System.Data;
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
            dic.Add("AS_NO", entity.AS_NO);
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
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetAsDetail(int unitId = 0, int op = 0)
        {
            DataTable dt;
            if(op == 0)
            {
                dt = AJTM_AS_DETAIL.Instance.GetCrateAsDetail(unitId);
            }
            else
            {
                dt = AJTM_AS_DETAIL.Instance.GetCancelAsDetail(unitId);
            }
            if(dt != null)
            {
                List<string> dstr = new List<string>();
                List<object> AsApplyD = new List<object>();
                foreach (DataRow dr in dt.Rows)
                {
                    var id = dr["AS_APPLY_ID"].ToString() + dr["AS_APPLY_NO"].ToString();
                    if (!dstr.Contains(id))
                    {
                        long AS_APPLY_ID = Convert.ToInt64(dr["AS_APPLY_ID"]);
                        string AS_APPLY_NO = dr["AS_APPLY_NO"].ToString();
                        var AsDetail = dt.AsEnumerable().Where(
                            x => x.Field<long>("AS_APPLY_ID") == AS_APPLY_ID
                        && x.Field<string>("AS_APPLY_NO") == AS_APPLY_NO).CopyToDataTable();
        
                        AsApplyD.Add(new
                        {
                            AS_APPLY_ID = dr["AS_APPLY_ID"].ToString(),
                            AS_APPLY_NO = dr["AS_APPLY_NO"].ToString(),
                            AS_DETAIL = AsDetail
                        });
                        dstr.Add(id);
                    }
                   
                }
                return SerializeObject(new
                {
                    AsApply = AsApplyD,
                    AsDetail = dt
                });
            }
            return "{AsApply:[],AsDetail:[]}";
        }
    }
}


namespace CS.WebUI.Controllers.Model
{
    public class AsDetail : BLL.Model.AJTM_AS_DETAIL.Entity
    {

    }
}