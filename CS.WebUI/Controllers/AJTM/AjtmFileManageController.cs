using CS.BLL.FW;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using CS.BLL.Model;
using System.Data;

namespace CS.WebUI.Controllers.AJTM
{
    public class AjtmFileManageController : FW.ABaseController
    {
        // GET: AjtmFileManage
        public ActionResult Edit(int id = 0)
        {
            Model.FileManage entity = new Model.FileManage();
            entity.FileTable = new DataTable();
            if (id > 0)
            {
                entity = AJTM_FILE_MANAGE.Instance.GetEntityByKey<Model.FileManage>(id);
                entity.FileTable = AJTM_FILE.Instance.GetTable(id);
            }
            return View(entity);
        }
        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(string title, string content, string field,int ID = 0)
        {
            JsonResultData result = new JsonResultData();
            if (ID > 0)
            {
                AJTM_FILE_MANAGE.Instance.Update(title, content, ID);
            }
            else
            {
                ID = AJTM_FILE_MANAGE.Instance.Add(title, content);
            }

            AJTM_FILE_MANAGE_FILE.Instance.Add(field, ID);
            result.IsSuccess = true;
            result.Message = "提交成功！";
            return Json(result, JsonRequestBehavior.AllowGet);
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
            //
            AJTM_FILE.Entity fileEntity = new AJTM_FILE.Entity();
            string err = "";
            try
            {
                //
                string basePath = "../File/FILE_MANAGE/";
                string savePath = Server.MapPath(basePath);

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

                    fileEntity.LENGTH = Request.TotalBytes;
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
                    fileEntity.FILE_TYPE = extension;
                    fileEntity.NAME = localname;
                    saveName = string.Format("{0}-{1}", DateTime.Now.ToString("yyyyMMdd-HHmmss"), SystemSession.UserName);
                    fileEntity.PATH = saveName;
                    byte[] fileByte = Request.BinaryRead(Request.TotalBytes);
                    FileStream fs = new FileStream(savePath + "\\" + saveName, FileMode.Create, FileAccess.Write);
                    fileEntity.URL = basePath + "\\" + saveName;
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
                    fileEntity.LENGTH = postedfile.ContentLength;
                    localname = Path.GetFileName(postedfile.FileName).Replace('%', '_');

                    fileEntity.NAME = localname;
                    //后缀名
                    string extension = BLL.Model.AJTM_FILE.GetSuffixByPath(localname);
                    if (upext.IndexOf("," + extension + ",") < 0)
                    {
                        result.IsSuccess = false;
                        result.Message = "不允许的附件类型" + extension;
                        return Json(result, JsonRequestBehavior.AllowGet);
                        //throw new Exception("不允许的附件类型：" + extension);
                    }
                    fileEntity.FILE_TYPE = extension;
                    saveName = string.Format("{0}-{1}-{2}_{3}", DateTime.Now.ToString("yyyyMMdd-HHmmss"), SystemSession.UserName, postedfile.ContentLength, localname);
                    fileEntity.PATH = saveName;
                    postedfile.SaveAs(savePath + "\\" + saveName);
                    fileEntity.URL = basePath + "\\" + saveName;
                }

            }
            catch (Exception ex)
            {
                err = ex.Message;
                result.IsSuccess = false;
                result.Message = ex.Message.ToString();
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            if (fileEntity.NAME.Length > 10)
            {
                result.IsSuccess = false;
                result.Message = "文件名太长，请保存10个字";
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            fileEntity = AJTM_FILE.Instance.Add(fileEntity);
            if (fileEntity.ID == 0)
            {

                result.IsSuccess = false;
                result.Message = "未保存成功";
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            result.IsSuccess = true;
            result.Result = SerializeObject(fileEntity);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}


namespace CS.WebUI.Controllers.Model
{
    public class FileManage : BLL.Model.AJTM_FILE_MANAGE.Entity
    {
        /// <summary>
        /// 
        /// </summary>
        public DataTable FileTable { get; set; }
    }
}