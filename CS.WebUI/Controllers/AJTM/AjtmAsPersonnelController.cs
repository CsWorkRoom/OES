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
    public class AjtmAsPersonnelController : FW.ABaseController
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <returns></returns>
        public ActionResult Add()
        {
            ViewBag.Handlng = AJTM_AS_PERSONNEL.Instance.GetDropdownForHandlng();
            ViewBag.Action = AJTM_AS_PERSONNEL.Instance.GetDropdownForAction();
            ViewBag.Education = AJTM_AS_PERSONNEL.Instance.GetDropdownForEducation();
            ViewBag.PostType = AJTM_AS_PERSONNEL.Instance.GetDropdownForPostType();
            ViewBag.Unit = SerializeObject(AJTM_UNIT.Instance.GetDropTree());
            ViewBag.AsType = SerializeObject(AJTM_AS_TYPE.Instance.GetDropTree());
            ViewBag.ModeAccess = SerializeObject(AJTM_ACCESS_MODE.Instance.GetDropDownForDt());
            return View(new Model.AsPersonnel());
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Add(Model.AsPersonnel entity)
        {
            JsonResultData result = new JsonResultData();
            if (entity.ID == 0)
            {
                AJTM_AS_PERSONNEL.Instance.Add(entity);
                if (!string.IsNullOrEmpty(entity.AS_NO))
                {
                    //查询AS_NO代码
                    DataTable dt = AJTM_AS_DETAIL.Instance.GetTableFields("ID,AS_APPLY_NO,AS_APPLY_ID", " AS_NO=?", new object[] { entity.AS_NO });
                    if (dt.Rows.Count > 0)
                    {
                        DataRow dr = dt.Rows[0];
                        //用编序号清单装填
                        int AsDetailId = Convert.ToInt32(dr["ID"]);
                        string AsApplyNo = dr["AS_APPLY_NO"].ToString();
                        int AsApplyId = Convert.ToInt32(dr["AS_APPLY_ID"]);
                        //待上编明细
                        Dictionary<string, object> dicAsDetail = new Dictionary<string, object>();
                        if (entity.ACTION == "上编")
                        {
                            AJTM_AS_DETAIL.Instance.BeginUse(entity.AS_NO);
                            AJTM_AS_DETAIL_STATUS.Instance.Add(AsDetailId, AsApplyNo, AsApplyId, BLL.Model.ENUM_AS_DETAIL_STATUS.使用.ToString());
                        }
                        else
                        {
                            AJTM_AS_DETAIL.Instance.EndCancel(entity.AS_NO);
                            AJTM_AS_DETAIL_STATUS.Instance.Add(AsDetailId, AsApplyNo, AsApplyId, BLL.Model.ENUM_AS_DETAIL_STATUS.销号.ToString());
                        }
                    }
                    else
                    {
                        result.IsSuccess = true;
                        result.Message = "数据提交成功,但未找到待上编信息";
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }

            }
            result.IsSuccess = true;
            result.Message = "数据提交成功";
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: AjtmAsPersonnel
        public ActionResult Edit(int id = 0)
        {
            ViewBag.Handlng = AJTM_AS_PERSONNEL.Instance.GetDropdownForHandlng();
            ViewBag.Action = AJTM_AS_PERSONNEL.Instance.GetDropdownForAction();
            ViewBag.Education = AJTM_AS_PERSONNEL.Instance.GetDropdownForEducation();
            ViewBag.PostType = AJTM_AS_PERSONNEL.Instance.GetDropdownForPostType();
            ViewBag.Unit = SerializeObject(AJTM_UNIT.Instance.GetDropTree());
            ViewBag.AsType = SerializeObject(AJTM_AS_TYPE.Instance.GetDropTree());
            ViewBag.ModeAccess = SerializeObject(AJTM_ACCESS_MODE.Instance.GetDropDownForDt());
            if (id > 0)
            {
                var entity = AJTM_AS_PERSONNEL.Instance.GetEntityByKey<Model.AsPersonnel>(id);
                return View(entity);
            }
            return View(new Model.AsPersonnel());
        }
        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(Model.AsPersonnel entity)
        {
            JsonResultData result = new JsonResultData();
            if (entity.ID > 0)
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic.Add("HANDLNG", entity.HANDLNG);
                dic.Add("ACCOUNT_NAME", entity.ACCOUNT_NAME);
                dic.Add("ACCOUNT_AGE", entity.ACCOUNT_AGE);
                dic.Add("ACCOUNT_EDUCATION", entity.ACCOUNT_EDUCATION);
                dic.Add("POST_TYPE", entity.POST_TYPE);
                dic.Add("AS_TYPE_ID", entity.AS_TYPE_ID);
                string AS_TYPE = AJTM_AS_TYPE.Instance.GetStringValueByKey(entity.AS_TYPE_ID, "NAME");
                dic.Add("AS_TYPE", AS_TYPE);
                dic.Add("ACCESS_MODE_ID", entity.ACCESS_MODE_ID);
                string ACCESS_MODE = AJTM_ACCESS_MODE.Instance.GetStringValueByKey(entity.ACCESS_MODE_ID, "NAME");
                dic.Add("ACCESS_MODE", ACCESS_MODE);
                dic.Add("FILE_NAME", entity.FILE_NAME);
                if (entity.FILE_SEND > Convert.ToDateTime("1900-01-01"))
                    dic.Add("FILE_SEND", entity.FILE_SEND);
                dic.Add("ACCOUNT_SOURCE", entity.ACCOUNT_SOURCE);
                dic.Add("ACCOUNT_SITUATION", entity.ACCOUNT_SITUATION);
                if (entity.AGREE_TIME > Convert.ToDateTime("1900-01-01"))
                    dic.Add("AGREE_TIME", entity.AGREE_TIME);
                if (entity.CHECKIN_TIME > Convert.ToDateTime("1900-01-01"))
                    dic.Add("CHECKIN_TIME", entity.CHECKIN_TIME);
                dic.Add("ACCOUNT_REMARK", entity.ACCOUNT_REMARK);
                dic.Add("HANDLER", entity.HANDLER);
                dic.Add("HANDLER_PHONE", entity.HANDLER_PHONE);
                dic.Add("REMARKS", entity.REMARKS);
                dic.Add("UPDATE_UID", entity.UPDATE_UID);
                dic.Add("UPDATE_TIME", DateTime.Now);
                AJTM_AS_PERSONNEL.Instance.UpdateByKey(dic, entity.ID);

            }
            result.IsSuccess = true;
            result.Message = "数据提交成功";
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(int id)
        {
            JsonResultData result = new JsonResultData();
            result.IsSuccess = true;
            result.Message = "删除成功";
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 获取已上编人员信息
        /// </summary>
        /// <param name="AccountName">用户名称</param>
        /// <param name="UnitId">集团信息</param>
        /// <returns></returns>
        public string GetPersonnel(string AccountName, int UnitId = 0)
        {
            return SerializeObject(AJTM_AS_PERSONNEL.Instance.GetTableByAccountName(AccountName, UnitId));
        }
    }
}

namespace CS.WebUI.Controllers.Model
{
    public class AsPersonnel : BLL.Model.AJTM_AS_PERSONNEL.Entity
    {

    }
}