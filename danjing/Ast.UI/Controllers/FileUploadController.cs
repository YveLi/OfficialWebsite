using Ast.Common;
using Ast.Common.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ast.UI.Controllers
{
    public class FileUploadController : UserBaseController
    {
        // GET: FileUpload
        private const int Success = 0;
        private const int Defeat = 1;
        public JsonResult FileRecive()
        {
            //var onlinepath = ConfigurationManager.AppSettings["OnlineUrl"];
            FileInfo fileInfo = new FileInfo();
            HttpFileCollectionBase files = Request.Files;
            try
            {
                if (files.Count > 0)
                {
                    foreach (string str in files)
                    {
                        var file = files[str];
                        string msg = new FileHelper(ConfigHandle.GetUploadConfig()).fileSaveAs(file, false, false);
                        if (msg.Contains("success"))
                        {
                            fileInfo = new FileInfo
                            {
                                code = Success,
                                msg = "上传成功",
                                data = new Data
                                {
                                    src = msg.Replace("success", ""),
                                    title = file.FileName
                                }
                            };
                        }
                        else
                        {
                            fileInfo = new FileInfo
                            {
                                code = Defeat,
                                msg = "文件大小不可超过2MB"
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                fileInfo = new FileInfo
                {
                    code = Defeat,
                    msg = ex.Message
                };
            }

            return Json(fileInfo, JsonRequestBehavior.AllowGet);
        }
        public class FileInfo
        {
            public FileInfo()
            {
                code = 0;
                msg = "上传成功";
                data = new Data
                {
                    src = "",
                    title = ""
                };
            }

            public int code { get; set; }  // 0 -成功  1-失败
            public string msg { get; set; } // 消息
            public Data data { get; set; } // 信息
        }
        public struct Data
        {
            public string src { get; set; } //图片路径
            public string title { get; set; } //图片名称
        }

        public ActionResult Upload(HttpPostedFileBase f)
        {
            try
            {
                var onlinepath = ConfigurationManager.AppSettings["OnlineUrl"];
                var files = Request.Files;
                if (files.Count == 0)
                    return Json(new
                    {
                        status = 1,
                        msg = "请选择要上传的文件"
                    });
                var curFile = files[0];
                if ((curFile.ContentLength / 1024) > 10240)
                {
                    return Json(new
                    {
                        status = 1,
                        msg = "请选择小于10M的文件"
                    });
                }
                //获取保存路径

                var filesUrl = Server.MapPath("~/Upload/" + OTAMSGUID + "/" + CurrentUserName + "/");
                if (Directory.Exists(filesUrl) == false)//路径不存在则创建
                    Directory.CreateDirectory(filesUrl);
                var fileName = Path.GetFileName(curFile.FileName);
                //文件后缀名
                var filePostfixName = fileName.Substring(fileName.LastIndexOf('.'));
                //新文件名
                var newFileName = CommFun.GetRandomFileName(filePostfixName);
                var path = Path.Combine(filesUrl, newFileName);
                //保存文件
                curFile.SaveAs(path);
                var newPath = "/Upload/" + OTAMSGUID + "/" + CurrentUserName + "/" + newFileName;
                return Json(new
                {
                    status = 0,
                    msg = "上传成功",
                    url = newPath
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    status = 1,
                    msg = "上传失败、错误信息：" + ex.Message
                });
            }
        }
    }
}