using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ast.Models;
using Ast.IBll;
using Ast.Common;
using System.Configuration;

namespace Ast.UI.Controllers
{
    public class LoginController : Controller
    {
        private IOTAUsersService userservice { get; set; }
        private IOTALoginLogService logservice { get; set; }
        // GET: Loginasdad
        public ActionResult Index()
        {
            ViewBag.Title = ConfigurationManager.AppSettings["webtitle"];
            ViewBag.Name = ConfigurationManager.AppSettings["webname"];
            ViewBag.Version = ConfigurationManager.AppSettings["webversion"];
            ViewBag.Company = ConfigurationManager.AppSettings["company"];
            ViewBag.Url = ConfigurationManager.AppSettings["weburl"];
            return View();
        }

        public JsonResult Go(LoginUser user)
        {
            user.PassWord = CommFun.Md5(user.PassWord);
            var userinfo = userservice.GetSingle(u => u.LoginName == user.LoginName && u.PassWord == user.PassWord && u.IsDel == 0);
            var log = new OTALoginLog()
            {
                IP = CommFun.GetIPAddress(),
                LoginName = user.LoginName,
                Memo = "帐号或密码错误",
                UserId = 0,
                AddTime = DateTime.Now,
                ModifyTime = DateTime.Now
            };
            if (userinfo == null)
            {
                logservice.Add(log);
                return Json(new { success = false, msg = "帐号或密码错误！" }, JsonRequestBehavior.AllowGet);
            }
            Session["LoginUser"] = userinfo;
            Session.Timeout = 60;
            log.LoginName = userinfo.LoginName;
            log.OTAMSGUID = userinfo.OTAMSGUID;
            log.UserId = userinfo.Id;
            log.Memo = "登录成功";
            logservice.Add(log);
            return Json(new
            {
                success = true,
                msg = "登录成功！",
                uid = userinfo.Id
            }, JsonRequestBehavior.AllowGet);
        }
    }
}