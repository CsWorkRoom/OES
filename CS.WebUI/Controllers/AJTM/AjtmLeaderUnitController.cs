using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using CS.BLL.Extension;
using CS.BLL.FW;
using CS.BLL.Model;

namespace CS.WebUI.Controllers.AJTM
{
    public class AjtmLeaderUnitController : FW.ABaseController
    {
        // GET: AjtmLeaderUnit
        public ActionResult Index(int unitId = 0)
        {
            ViewBag.LeaderType = AJTM_LEADER_TYPE.Instance.GetListEntity();
            ViewBag.Unit = SerializeObject(AJTM_UNIT.Instance.GetDropTree());
            ViewBag.SetupLevel = SerializeObject(AJTM_SETUP_LEVEL.Instance.GetDropDownForDt());
            ViewBag.UnitId = unitId;
            return View();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Index(string Leader, string LeaderTypeUnit, int UNIT_ID = 0)
        {
            JsonResultData result = new JsonResultData();
            if (UNIT_ID > 0)
            {
                var unit = AJTM_UNIT.Instance.GetUnitAndParent(UNIT_ID);
                if (unit.Rows.Count == 0)
                {
                    result.IsSuccess = false;
                    result.Message = "未找到该单位";
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                var UNIT_NAME = unit.Rows[0]["NAME"].ToString();
                var UNIT_PARENT = unit.Rows[0]["PANRET"].ToString();
                var UNIT_PARENT_ID = Convert.ToInt32(unit.Rows[0]["PARENT_ID"]);

                ////删除所有配置信息
                //var luEntity = DeserializeObject<List<BLL.Model.AJTM_LEADER_UNIT.Entity>>(LeaderTypeUnit);
                //BLL.Model.AJTM_LEADER_UNIT.Instance.Delete(" UNIT_ID=?", UNIT_ID);
                //foreach (var entity in luEntity)
                //{
                //    Dictionary<string, object> dic = new Dictionary<string, object>();
                //    dic.Add("LEADER_TYPE_ID", entity.LEADER_TYPE_ID);
                //    dic.Add("LEADER_TYPE", entity.LEADER_TYPE);
                //    dic.Add("UNIT_NAME", UNIT_NAME);
                //    dic.Add("UNIT_ID", UNIT_ID);
                //    dic.Add("UNIT_PARENT_ID", UNIT_PARENT_ID);
                //    dic.Add("UNIT_PARENT", UNIT_PARENT);
                //    dic.Add("NUM", entity.NUM);
                //    dic.Add("CREATE_UID", SystemSession.UserID);
                //    dic.Add("UPDATE_UID", SystemSession.UserID);
                //    dic.Add("CREATE_TIME", DateTime.Now);
                //    dic.Add("UPDATE_TIME", DateTime.Now);
                //    BLL.Model.AJTM_LEADER_UNIT.Instance.Add(dic);
                //}
                //新增所有配置信息
                BLL.Model.AJTM_LEADER.Instance.Delete(" UNIT_ID=?", UNIT_ID);
                var lEntity = DeserializeObject<List<BLL.Model.AJTM_LEADER.Entity>>(Leader);
                foreach (var entity in lEntity)
                {
                    var LEADER_NAME = entity.LEADER_NAME.Trim();
                    if (string.IsNullOrEmpty(LEADER_NAME)) entity.IS_USE = 0;
                    else entity.IS_USE = 1;
                    entity.IS_RESERVE = AJTM_LEADER.Instance.JudgeReserve(entity.IS_USE, entity.IS_ORG, entity.IS_AS, entity.IS_CONCURREENT_POST);

                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    dic.Add("LEADER_TYPE_ID", entity.LEADER_TYPE_ID);
                    dic.Add("LEADER_TYPE", entity.LEADER_TYPE);
                    dic.Add("UNIT_NAME", UNIT_NAME);
                    dic.Add("UNIT_ID", UNIT_ID);
                    dic.Add("UNIT_PARENT_ID", UNIT_PARENT_ID);
                    dic.Add("UNIT_PARENT", UNIT_PARENT);
                    dic.Add("LAEDER_LEVEL_ID", entity.LAEDER_LEVEL_ID);
                    dic.Add("LEADER_LEVEL", entity.LEADER_LEVEL);
                    dic.Add("LEADER_JOB", entity.LEADER_JOB);
                    dic.Add("IS_AS", entity.IS_AS);
                    dic.Add("IS_USE", entity.IS_USE);
                    dic.Add("IS_ORG", entity.IS_ORG);
                    dic.Add("IS_CONCURREENT_POST", entity.IS_CONCURREENT_POST);
                    dic.Add("IS_RESERVE", entity.IS_RESERVE);
                    dic.Add("LEADER_NAME", LEADER_NAME);
                    dic.Add("CREATE_UID", SystemSession.UserID);
                    dic.Add("UPDATE_UID", SystemSession.UserID);
                    dic.Add("CREATE_TIME", DateTime.Now);
                    dic.Add("UPDATE_TIME", DateTime.Now);
                    BLL.Model.AJTM_LEADER.Instance.Add(dic);
                }

                result.IsSuccess = true;
                result.Message = "数据提交成功";
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                result.IsSuccess = false;
                result.Message = "未选择单位";
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="UnitID"></param>
        /// <returns></returns>
        public string GetLeaderInfoByUnit(int UnitID)
        {
            var r = AJTM_LEADER_UNIT.Instance.GetTable(" UNIT_ID=?", new object[] { UnitID }); ;
            var r2 = AJTM_LEADER.Instance.GetTable(" UNIT_ID=?", new object[] { UnitID });

            return SerializeObject(new
            {
                LeaderUnit = r,
                Leader = r2
            });
        }
        /// <summary>
        /// 报表
        /// </summary>
        /// <returns></returns>
        public ActionResult Report()
        {

            CS.BLL.ModelExtension.EX_LEADER_UNIT model = new BLL.ModelExtension.EX_LEADER_UNIT();
            ViewBag.ARR = model.GetLeaderUnitReportInfo();
            return View();
        }

        public ActionResult Edit()
        {
            return View();
        }

        /// <summary>
        /// 下载
        /// </summary>
        /// <returns></returns>
        public ActionResult Export()
        {
            string path = Server.MapPath(AJTM_LEADER_UNIT.Instance.PATH);
            ExcelLeaderUnit excel = new ExcelLeaderUnit(path, "市本级县处级及以上单位核定领导职数情况表");
            string fullName = excel.Save();
            try
            {
                string filename = HttpUtility.UrlEncode(string.Format("{1}_{0}.xlxs", DateTime.Now.ToString("yyyyMMddHHmmss"), "单位核定领导职数情况表"), Encoding.UTF8);
                System.Web.HttpContext.Current.Response.Buffer = true;
                System.Web.HttpContext.Current.Response.Clear();//清除缓冲区所有内容
                System.Web.HttpContext.Current.Response.ContentType = "application/octet-stream";
                System.Web.HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + filename);
                System.Web.HttpContext.Current.Response.WriteFile(fullName);
                System.Web.HttpContext.Current.Response.Flush();
                System.Web.HttpContext.Current.Response.End();
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
