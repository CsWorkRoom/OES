using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CS.BLL.FW;
using CS.BLL.Model;


namespace CS.WebUI.Controllers.AJTM
{
    public class AjtmAsPersonnelController : FW.ABaseController
    {
        // GET: AjtmAsPersonnel
        public ActionResult Edit(int id = 0)
        {
            ViewBag.Handlng = AJTM_AS_PERSONNEL.Instance.GetDropdownForHandlng();
            ViewBag.Action = AJTM_AS_PERSONNEL.Instance.GetDropdownForAction();
            ViewBag.Education = AJTM_AS_PERSONNEL.Instance.GetDropdownForEducation();
            ViewBag.PostType = AJTM_AS_PERSONNEL.Instance.GetDropdownForPostType();
            ViewBag.Unit = SerializeObject(AJTM_UNIT.Instance.GetDropTree());
            ViewBag.AsType = SerializeObject(AJTM_AS_TYPE.Instance.GetDropTree());
            return View(new Model.AsPersonnel());
        }
    }
}

namespace CS.WebUI.Controllers.Model
{
    public class AsPersonnel : BLL.Model.AJTM_AS_PERSONNEL.Entity
    {

    }
}