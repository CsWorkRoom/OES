using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CS.BLL.FW;
using CS.BLL.Model;

namespace CS.WebUI.Controllers.AJTM
{
    public class AjtmLeaderUnitController : FW.ABaseController
    {
        // GET: AjtmLeaderUnit
        public ActionResult Index()
        {
            ViewBag.LeaderType = AJTM_LEADER_TYPE.Instance.GetListEntity();
            ViewBag.Unit = SerializeObject(AJTM_UNIT.Instance.GetDropTree());
            ViewBag.SetupLevel = SerializeObject(AJTM_SETUP_LEVEL.Instance.GetDropDownForDt());
            return View();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Index(string Leader, string LeaderTypeUnit, int UNIT_ID = 0, string UNIT_NAME = "", int UNIT_PARENT_ID = 0, string UNIT_PARENT = "")
        {
            JsonResultData result = new JsonResultData();
            if (UNIT_ID > 0) {

                var luEntity = DeserializeObject<List<BLL.Model.AJTM_LEADER_UNIT.Entity>>(LeaderTypeUnit);
                //删除所有配置信息
                BLL.Model.AJTM_LEADER_UNIT.Instance.Delete(" UNIT_ID=?", UNIT_ID);
                foreach (var entity in luEntity)
                {
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    dic.Add("LEADER_TYPE_ID", entity.LEADER_TYPE_ID);
                    dic.Add("LEADER_TYPE", entity.LEADER_TYPE);
                    dic.Add("UNIT_NAME", UNIT_NAME);
                    dic.Add("UNIT_ID", UNIT_ID);
                    dic.Add("UNIT_PARENT_ID", UNIT_PARENT_ID);
                    dic.Add("UNIT_PARENT", UNIT_PARENT);
                    dic.Add("NUM", entity.NUM);
                    dic.Add("CREATE_UID", SystemSession.UserID);
                    dic.Add("UPDATE_UID", SystemSession.UserID);
                    dic.Add("CREATE_TIME", DateTime.Now);
                    dic.Add("UPDATE_TIME", DateTime.Now);
                    BLL.Model.AJTM_LEADER_UNIT.Instance.Add(dic);
                }
                //新增所有配置信息
                BLL.Model.AJTM_LEADER.Instance.Delete(" UNIT_ID=?", UNIT_ID);
                var lEntity = DeserializeObject<List<BLL.Model.AJTM_LEADER.Entity>>(Leader);
                foreach (var entity in lEntity)
                {
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
                    dic.Add("LEADER_NAME", entity.LEADER_NAME);
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
        public string GetLeaderInfoByUnit(int UnitID) {

            var r = AJTM_LEADER_UNIT.Instance.GetTable(" UNIT_ID=?", new object[] { UnitID }); ;
            var r2 = AJTM_LEADER.Instance.GetTable(" UNIT_ID=?", new object[] { UnitID });

            return SerializeObject(new
            {
                LeaderUnit = r,
                Leader = r2
            });
        }


        public ActionResult Edit()
        {
            return View();
        }
    }
}
