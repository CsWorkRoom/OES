using CS.BLL.FW;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace CS.WebUI.Controllers.AJTM
{
    public class AjtmFileManageController : Controller
    {
        // GET: AjtmFileManage
        public ActionResult Edit()
        {
            return View();
        }

        /// <summary>
        /// 上传
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file)
        {
            JsonResultData result = new JsonResultData();
            //本地文件名
            string localname = "";
            //保存文件名
            string saveName = "";
            //最大上传大小，单位是M
            int maxAttachSize = 5;
            //表单文件域name
            string inputname = "file";
            //上传扩展名
            string upext = ",txt,rar,zip,jpg,jpeg,gif,png,txt,pdf,ppt,pptx,doc,docx,xls,xlsx,avi,wma,mp3,mid,";
            //立即上传模式，仅为演示用
            string immediate = Request.QueryString["immediate"];

            string err = "";
            try
            {
                //
                string savePath = Server.MapPath("../File/FILE_MANAGE/");

                if (string.IsNullOrWhiteSpace(savePath))
                {
                    result.IsSuccess = false;
                    result.Message = "未配置附件存放路径，请联系管理员配置";
                    return Json(result, JsonRequestBehavior.AllowGet);
                    //throw new Exception("未配置附件存放路径，请联系管理员配置AttachmentPath。");
                }
                if (Directory.Exists(savePath) == false)
                {
                    Directory.CreateDirectory(savePath);
                }
                string disposition = Request.ServerVariables["HTTP_CONTENT_DISPOSITION"];
                if (disposition != null)
                {
                    //HTML5上传
                    if (Request.TotalBytes > maxAttachSize * 1024 * 1024)
                    {
                        result.IsSuccess = false;
                        result.Message = "文件大小超过" + maxAttachSize + "M";
                        return Json(result, JsonRequestBehavior.AllowGet);
                        //throw new Exception("文件大小超过" + maxAttachSize + "M");
                    }
                    //读取原始文件名
                    localname = Server.UrlDecode(Regex.Match(disposition, "filename=\"(.+?)\"").Groups[1].Value);
                    //后缀名
                    string extension = BLL.Model.AJTM_FILE.GetSuffixByPath(localname);
                    if (upext.IndexOf("," + extension + ",") < 0)
                    {
                        result.IsSuccess = false;
                        result.Message = "不允许的附件类型：" + extension;
                        return Json(result, JsonRequestBehavior.AllowGet);
                        //throw new Exception("不允许的附件类型：" + extension);
                    }
                    saveName = string.Format("{0}-{1}-{2}_{3}", DateTime.Now.ToString("yyyyMMdd-HHmmss"), SystemSession.UserName, Request.TotalBytes, localname);
                    byte[] fileByte = Request.BinaryRead(Request.TotalBytes);
                    FileStream fs = new FileStream(savePath + "\\" + saveName, FileMode.Create, FileAccess.Write);
                    fs.Write(fileByte, 0, fileByte.Length);
                    fs.Flush();
                    fs.Close();
                }
                else
                {
                    HttpPostedFileBase postedfile = Request.Files.Get(inputname);
                    if (postedfile == null)
                    {
                        postedfile = file;
                    }
                    if (postedfile == null)
                    {
                        result.IsSuccess = false;
                        result.Message = "未收到上传文件";
                        return Json(result, JsonRequestBehavior.AllowGet);
                        //throw new Exception("未收到上传文件");
                    }
                    if (postedfile.ContentLength > maxAttachSize * 1024 * 1024)
                    {
                        result.IsSuccess = false;
                        result.Message = "文件大小超过" + maxAttachSize + "M";
                        return Json(result, JsonRequestBehavior.AllowGet);
                        //throw new Exception("文件大小超过" + maxAttachSize + "M");
                    }
                    localname = Path.GetFileName(postedfile.FileName).Replace('%', '_');
                    //后缀名
                    string extension = BLL.Model.AJTM_FILE.GetSuffixByPath(localname);
                    if (upext.IndexOf("," + extension + ",") < 0)
                    {
                        result.IsSuccess = false;
                        result.Message = "不允许的附件类型" + extension;
                        return Json(result, JsonRequestBehavior.AllowGet);
                        //throw new Exception("不允许的附件类型：" + extension);
                    }

                    saveName = string.Format("{0}-{1}-{2}_{3}", DateTime.Now.ToString("yyyyMMdd-HHmmss"), SystemSession.UserName, postedfile.ContentLength, localname);

                    postedfile.SaveAs(savePath + "\\" + saveName);
                }

            }
            catch (Exception ex)
            {
                err = ex.Message;
                result.IsSuccess = false;
                result.Message = ex.Message.ToString();
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            result.IsSuccess = true;
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}