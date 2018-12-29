using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ast.Models.DBModels;
using SqlSugar;
using SyntacticSugar;
using Ast.Models;
using System.Web.Mvc.Filters;
using Ast.IBll;
using Ast.Bll;

namespace Ast.Web
{
    [Authorize]
    public class BaseController : Controller
    {
        private IMemberListService memberservice = new MemberListService();
        /// <summary>
        /// 是否已登录
        /// </summary>
        /// <returns></returns>
        protected bool IsLogin
        {
            get
            {
                return HttpContext.User.Identity.IsAuthenticated;
            }
        }
        /// <summary>
        /// 当前登录的帐户
        /// </summary>
        protected string CurrentName
        {
            get
            {
                if (!HttpContext.User.Identity.IsAuthenticated)
                    return "";
                var user = HttpContext.User.Identity.Name;
                return user.Split('$')[2];
            }
        }
        protected MemberList CurrentUser
        {
            get
            {
                if (!HttpContext.User.Identity.IsAuthenticated)
                    return null;
                var user = memberservice.GetById(CurrentUserId.ToString());
                return user;
            }
        }
        /// <summary>
        /// 当前登录的用户ID
        /// </summary>
        protected int CurrentUserId
        {
            get
            {
                if (!HttpContext.User.Identity.IsAuthenticated)
                    return -1;
                var user = HttpContext.User.Identity.Name;
                return Convert.ToInt32(user.Split('$')[0]);
            }
        }
    }
}
