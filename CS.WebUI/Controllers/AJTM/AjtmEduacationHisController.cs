using CS.BLL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CS.WebUI.Controllers.AJTM
{
    public class AjtmEduacationHisController : Controller
    {
        // GET: AjtmEduacationHis
        public ActionResult Look(int id = 0)
        {
            Model.EduacationHis entity = new Model.EduacationHis();
            if (id > 0)
            {
                entity = AJTM_EDUACATION_HIS.Instance.GetEntityByKey<Model.EduacationHis>(id);
            }
            return View(entity);
        }
    }
}


namespace CS.WebUI.Controllers.Model
{
    public class EduacationHis : BLL.Model.AJTM_EDUACATION_HIS.Entity
    {

    }
}