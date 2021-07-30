using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CS.BLL.Model;

namespace CS.WebUI.Controllers.AJTM
{
    public class AjtmEduacationController : FW.ABaseController
    {
        // GET: AjtmEduacation
        public ActionResult Edit(int id = 0)
        {
            Model.Eduacation entity = new Model.Eduacation();
            if (id > 0)
            {
                entity = AJTM_EDUACATION.Instance.GetEntityByKey<Model.Eduacation>(id);
            }
            return View(entity);
        }
        /// <summary>
        /// 编辑
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(HttpPostedFileBase file, string excelJson = "", string excelName = "", int id = 0)
        {
            string basePath = AJTM_EDUACATION.PATH;
            string rootPath = Server.MapPath(basePath);
            JsonResultData result = new JsonResultData();
            string excelPath = "";
            string excelDown = "";
            if (file != null)
            {
                try
                {
                    excelPath = UploadExcel(file, "../File/EDUACATION/", ",xlsx,");
                }
                catch (Exception ex)
                {
                    result.IsSuccess = false;
                    result.Message = ex.ToString();
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            try
            {
                CS.BLL.Extension.LuckSheetByExcel exc = new BLL.Extension.LuckSheetByExcel(rootPath, excelJson);
                excelDown = exc.Save();
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = ex.ToString();
                return Json(result, JsonRequestBehavior.AllowGet);
            }

            if (id > 0)
            {
                var entity = AJTM_EDUACATION.Instance.GetEntityByKey<Model.Eduacation>(id);
                if (entity.ID > 0)
                {
                    if (file == null) excelPath = entity.EXCEL_PATH;
                    AJTM_EDUACATION.Instance.Update(id, excelName, excelJson, excelPath, excelDown);
                    AJTM_EDUACATION_HIS.Instance.Add(id, excelName, excelJson, excelPath, excelDown);
                }
            }
            else
            {
                id = AJTM_EDUACATION.Instance.Add(excelName, excelJson, excelPath, excelDown);
                AJTM_EDUACATION_HIS.Instance.Add(id, excelName, excelJson, excelPath, excelDown);
            }
            result.IsSuccess = true;
            result.Message = "提交成功!";
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 查看
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        public ActionResult Look(int id = 0)
        {
            Model.Eduacation entity = new Model.Eduacation();
            if (id > 0)
            {
                entity = AJTM_EDUACATION.Instance.GetEntityByKey<Model.Eduacation>(id);
            }
            return View(entity);
        }
        /// <summary>
        /// 下载
        /// </summary>
        /// <param name="excelJson"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Export(string excelJson = "", int id = 0, string type = "",int isSource=0)
        {
            string rootPath = AJTM_EDUACATION.PATH; 
            string title = "";
            string path = "";
            if (id == 0) return ShowAlert("未找到文件！"); ;
            if (string.IsNullOrEmpty(excelJson))
            {
                if (string.IsNullOrEmpty(type))
                {
                    title = AJTM_EDUACATION.Instance.GetStringValueByKey(id, "TITLE");
                    if (isSource == 0)
                        path = AJTM_EDUACATION.Instance.GetStringValueByKey(id, "EXCEL_DOWN");
                    else
                        path = AJTM_EDUACATION.Instance.GetStringValueByKey(id, "EXCEL_PATH");
                }
                else
                {
                    title = AJTM_EDUACATION_HIS.Instance.GetStringValueByKey(id, "TITLE");
                    if (isSource == 0)
                        path = AJTM_EDUACATION_HIS.Instance.GetStringValueByKey(id, "EXCEL_DOWN");
                    else
                        path = AJTM_EDUACATION_HIS.Instance.GetStringValueByKey(id, "EXCEL_PATH");
                }

            }
            else
            {
                CS.BLL.Extension.LuckSheetByExcel exc = new BLL.Extension.LuckSheetByExcel(rootPath, excelJson);
                path = exc.Save();
                title = DateTime.Now.ToString("yyyy年MM月dd日") + "基础教育表";
            }
            try
            {
                Export(path, title);
            }
            catch (Exception ex)
            {
                return ShowAlert("导出数据到Excel出现未知错误：" + ex.Message);
            }
            return ShowAlert("导出成功！");
        }
    }
}


namespace CS.WebUI.Controllers.Model
{
    public class Eduacation : BLL.Model.AJTM_EDUACATION.Entity
    {

    }
}