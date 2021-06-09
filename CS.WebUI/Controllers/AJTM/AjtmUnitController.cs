using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
            var unitkeyValue = BLL.Model.AJTM_UNIT.Instance.GetList<BLL.Model.AJTM_UNIT.Entity>();
            var listKeyValue = new List<object>();
            foreach (var item in unitkeyValue)
            {
                listKeyValue.Add(new
                {
                    id = item.ID,
                    pId = item.PARENT_ID,
                    name = item.NAME,
                    value = item.ID
                });
            }
            ViewBag.ParentList = SerializeObject(listKeyValue);

            if (id > 0)
            {
                var r = BLL.Model.AJTM_UNIT.Instance.GetEntityByKey<BLL.Model.AJTM_UNIT.Entity>(id);
                var model = Common.Fun.ClassToCopy<BLL.Model.AJTM_UNIT.Entity,Model.Unit>(r);
                return View(model);
            }
            return View();
        }
    }
}

namespace CS.WebUI.Controllers.Model
{
    public class Unit : BLL.Model.AJTM_UNIT.Entity
    {
        public string JsonStr { get; set; }
    }
}