using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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

        public ActionResult Edit()
        {
            return View();
        }
    }
}
