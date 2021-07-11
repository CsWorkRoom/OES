﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CS.BLL.FW;
using CS.BLL.Model;

namespace CS.WebUI.Controllers.AJTM
{
    public class AjtmUnitController : FW.ABaseController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Detail(int id = 0)
        {
            ViewBag.SetupLevel = AJTM_SETUP_LEVEL.Instance.GetDropDown();
            ViewBag.SetupNature = AJTM_SETUP_NATRUE.Instance.GetDropDown();
            ViewBag.SetupType = AJTM_SETUP_TYPE.Instance.GetDropDown();
            ViewBag.SetupRange = AJTM_SETUP_RANGE.Instance.GetDropDown();
            ViewBag.OutLayMode = AJTM_OUTLAY_MODE.Instance.GetDropDown();
            ViewBag.AsType = SerializeObject(AJTM_AS_TYPE.Instance.GetDropTree());
            ViewBag.Unit = SerializeObject(AJTM_UNIT.Instance.GetDropTree());
            if (id > 0)
            {
                var r = BLL.Model.AJTM_UNIT.Instance.GetEntityByKey<BLL.Model.AJTM_UNIT.Entity>(id);
                var model = Common.Fun.ClassToCopy<BLL.Model.AJTM_UNIT.Entity,Model.Unit>(r);
                model.AsUnitJson = SerializeObject(AJTM_UNIT_AS.Instance.GetTableByUnitId(model.ID));
                return View(model);
            }
            return View(new Model.Unit());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Detail(Model.Unit entity)
        {
            JsonResultData result = new JsonResultData();
            if (string.IsNullOrEmpty(entity.NAME))
            {
                result.IsSuccess = false;
                result.Message = "提交失败,单位名称不能为空";
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("NAME", entity.NAME);
            dic.Add("PARENT_ID", entity.PARENT_ID);
            dic.Add("SETUP_TYPE_ID", entity.SETUP_TYPE_ID);
            dic.Add("SETUP_NATRUE_ID", entity.SETUP_NATRUE_ID);
            dic.Add("SETUP_LEVEL_ID", entity.SETUP_LEVEL_ID);
            dic.Add("OUTLAY_MODE_ID", entity.OUTLAY_MODE_ID);
            dic.Add("SETUP_RANGE_ID", entity.SETUP_RANGE_ID);
            dic.Add("IS_PUBLIC", entity.IS_PUBLIC);
            dic.Add("DEP_NUM", entity.DEP_NUM);
            dic.Add("WITHIN_MAIN_NUM", entity.WITHIN_MAIN_NUM);
            dic.Add("WITHIN_VICE_NUM", entity.WITHIN_VICE_NUM);
            dic.Add("OTHER_NUM", entity.OTHER_NUM);
            dic.Add("OFFICE_MIAN_NUM", entity.OFFICE_MIAN_NUM);
            dic.Add("OFFICE_VICE_NUM", entity.OFFICE_VICE_NUM);
            dic.Add("COUNTY_MIAN_L", entity.COUNTY_MIAN_L);
            dic.Add("COUNTY_VICE_L", entity.COUNTY_VICE_L);
            dic.Add("VILLAGE_MIAN_L", entity.VILLAGE_MIAN_L);
            dic.Add("VILLAGE_VICE_L", entity.VILLAGE_VICE_L);
            dic.Add("COUNTY_MIAN_MR", entity.COUNTY_MIAN_MR);
            dic.Add("COUNTY_VICE_MR", entity.COUNTY_VICE_MR);
            dic.Add("VILLAGE_MIAN_MR", entity.VILLAGE_MIAN_MR);
            dic.Add("VILLAGE_VICE_MR", entity.VILLAGE_VICE_MR);
            //
            if (entity.ID > 0)
            {

                dic.Add("UPDATE_UID", SystemSession.UserID);
                dic.Add("UPDATE_TIME", DateTime.Now);
                AJTM_UNIT.Instance.Update(dic, " ID=?", new object[] { entity.ID });
                AJTM_UNIT_AS.Instance.Delete(" UNIT_ID=?", new object[] { entity.ID });
            }
            else
            {
                dic.Add("IS_USE", 1);
                dic.Add("CREATE_UID", SystemSession.UserID);
                dic.Add("UPDATE_UID", SystemSession.UserID);
                dic.Add("CREATE_TIME", DateTime.Now);
                dic.Add("UPDATE_TIME", DateTime.Now);
                entity.ID = AJTM_UNIT.Instance.Add(dic, true);
            }
            //
            var unitAs = DeserializeObject<List<AJTM_UNIT_AS.Entity>>(entity.AsUnitJson);
            foreach (var item in unitAs)
            {
                Dictionary<string, object> unitAsDic = new Dictionary<string, object>();
                unitAsDic.Add("AS_TYPE_ID", item.AS_TYPE_ID);
                unitAsDic.Add("UNIT_ID", entity.ID);
                unitAsDic.Add("VERIFICATION_NUM", item.VERIFICATION_NUM);
                unitAsDic.Add("BEGIN_NUM", item.BEGIN_NUM);
                AJTM_UNIT_AS.Instance.Add(unitAsDic);
            }

            result.IsSuccess = true;
            result.Message = "数据提交成功";
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 启用
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SetEnable(int id)
        {
            JsonResultData result = new JsonResultData();
            result.IsSuccess = true;
            result.Message = "启用成功";
            try
            {
                int i = AJTM_UNIT.Instance.SetEnable(id);
                if (i < 1)
                {
                    throw new Exception("未知原因");
                }
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = "启用失败：" + ex.Message;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 禁用
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SetUnable(int id)
        {
            JsonResultData result = new JsonResultData();
            result.IsSuccess = true;
            result.Message = "禁用成功";
            try
            {
                int i = AJTM_UNIT.Instance.SetUnable(id);
                if (i < 1)
                {
                    throw new Exception("未知原因");
                }
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = "禁用失败：" + ex.Message;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewBag.Unit = SerializeObject(CS.BLL.Model.AJTM_UNIT.Instance.GetDropTree());
            return View();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Show(int unitid = 0)
        {
            //CS.BLL.Extension.UnitLeaderExcel excel = new BLL.Extension.UnitLeaderExcel("I:\\OES\\DOC", "text");
            //excel.Save();
            ViewBag.UnitInfo = AJTM_UNIT.Instance.GetUnitByIdForShow(unitid);
            ViewBag.UnitAs = AJTM_UNIT_AS.Instance.GetListByUnitId(unitid);
            ViewBag.Leader = AJTM_LEADER.Instance.GetListEntityByUnitId(unitid);
            ViewBag.leaderUnit = AJTM_LEADER_UNIT.Instance.GetListEntityByUnitId(unitid);
            return View();
        }
    }
}

namespace CS.WebUI.Controllers.Model
{
    public class Unit : BLL.Model.AJTM_UNIT.Entity
    {
        public string AsUnitJson { get; set; }
    }
}