using CS.BLL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CS.WebUI.Controllers.AJTM
{
    public class AjtmLeaderController : FW.ABaseController
    {
        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: AjtmLeader
        public ActionResult Edit(int id = 0)
        {
            ViewBag.Unit = SerializeObject(AJTM_UNIT.Instance.GetDropTree());
            ViewBag.LeaderType = AJTM_LEADER_TYPE.Instance.GetListEntity();
            ViewBag.SetupLevel = AJTM_SETUP_LEVEL.Instance.GetDropDown();
            if (id > 0)
            {
                var model = AJTM_LEADER.Instance.GetEntityByKey<Model.Leader>(id);
                return View(model);
            }
            return View(new Model.Leader());
        }
        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(Model.Leader entity)
        {
            JsonResultData result = new JsonResultData();
            entity.LEADER_TYPE = AJTM_LEADER_TYPE.Instance.GetStringValueByKey(entity.LEADER_TYPE_ID, "NAME");
            entity.LEADER_LEVEL = AJTM_LEADER_TYPE.Instance.GetStringValueByKey(entity.LAEDER_LEVEL_ID, "NAME");
            if (entity.ID > 0)
            {
                var model = AJTM_LEADER.Instance.GetEntityByKey<Model.Leader>(entity.ID);
                if (model == null)
                {
                    result.IsSuccess = false;
                    result.Message = "数据提交失败：未找到对应领导信息";
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (model.IS_INIT == 1)
                {
                    AJTM_LEADER.Instance.UpdateInit(entity);
                }
                else
                {
                    AJTM_LEADER.Instance.UpdateNotInit(entity);
                }
            }
            else
            {
                AJTM_LEADER.Instance.Add(entity);
            }
            //更新领导预留信息
            string leaderremark = AJTM_LEADER.Instance.GetLeaderRemark(entity.UNIT_ID);
            Dictionary<string, object> dicUnit = new Dictionary<string, object>();
            dicUnit.Add("LEADER_REAMRK", leaderremark);
            AJTM_UNIT.Instance.UpdateByKey(dicUnit, entity.UNIT_ID);
            //
            result.IsSuccess = true;
            result.Message = "数据提交成功";
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(int id = 0)
        {
            JsonResultData result = new JsonResultData();
            result.IsSuccess = true;
            result.Message = "删除成功";
            try
            {
                var model = AJTM_LEADER.Instance.GetEntityByKey<Model.Leader>(id);
                if (model == null)
                {
                    result.IsSuccess = false;
                    result.Message = "删除失败：未找到对应领导信息";
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                if (model.IS_INIT == 1)
                {
                    AJTM_LEADER_UNIT.Instance.SplusNum(model.LEADER_TYPE_ID, model.UNIT_ID);
                }
                var i = AJTM_LEADER.Instance.DeleteByKey(model.ID);
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = "启用失败：" + ex.Message;
            }

            return Json(result, JsonRequestBehavior.AllowGet); ;
        }
    }

}

namespace CS.WebUI.Controllers.Model
{
    public class Leader : BLL.Model.AJTM_LEADER.Entity
    {

    }
}