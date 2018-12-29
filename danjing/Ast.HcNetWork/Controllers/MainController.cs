using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ast.Common;
using Ast.IBll;
using Ast.Models;
using Ast.Models.ViewModels;
using System.Configuration;

namespace Ast.HcNetWork.Controllers
{
    public class MainController : UserBaseController
    {

        public ActionResult Index()
        {
            ViewBag.UserName = CurrentRealName;
            ViewBag.WebSetting = CurrentWebSet;
            return View();
        }

        public ActionResult LoginOut()
        {
            Session.Remove("LoginUser");
            return Content(string.Empty);
        }

    }
}