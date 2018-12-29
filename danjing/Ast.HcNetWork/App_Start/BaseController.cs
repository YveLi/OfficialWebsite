using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ast.Models.DBModels;
using Ast.Models;
//using Ast.UI.Aop;
using System.Web.Mvc.Filters;
namespace Ast.HcNetWork
{
    //[AopActionFilter()]
    public class BaseController : Controller
    {
        private OTAUsers _user;
        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
            if (Session["LoginUser"] == null)
            {
                filterContext.HttpContext.Response.Redirect("/Admin/Login");
            }
            _user = Session["LoginUser"] as OTAUsers;
        }
    }
}
