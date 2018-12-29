using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ast.Models.DBModels;
using SqlSugar;
using SyntacticSugar;
using Ast.Models;
//using Ast.UI.Aop;
using System.Web.Mvc.Filters;
namespace Ast.UI
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
                filterContext.HttpContext.Response.Redirect("/Login/Index");
            }
            _user = Session["LoginUser"] as OTAUsers;
        }
        /// <summary>
        /// Action 执行之后 发生
        /// </summary>
        /// <param name="filterContext"></param>
        //public override void OnActionExecuted(ActionExecutedContext filterContext)
        //{
        //    if (IsExecutePowerLogic && Account != null)
        //    {
        //        this.PowerLogic(filterContext);
        //    }
        //    base.OnActionExecuted(filterContext);
        //}
        /// <summary>
        /// 设置在线信息
        /// </summary>
        /// <param name="_userInfo"></param>
        //private void SetOnlineVisitors(Users _users)
        //{
        //    var timeOut = DateTime.Now.AddMinutes(-ConstPubConst.SiteOnlineTimeOut);
        //    List<OnlineVisitorsResult> onlineList = null;
        //    var key = ConstPubConst.SessionOnline;
        //    var cm = CacheManager<List<OnlineVisitorsResult>>.GetInstance();
        //    if (cm.ContainsKey(key))
        //    {
        //        onlineList = cm[key];
        //        if (onlineList.IsValuable())
        //        {
        //            //过滤超时用户
        //            onlineList = onlineList.Where(it => it.LastActionTime > timeOut).ToList();
        //        }
        //    }
        //    if (onlineList == null || onlineList.Count == 0)
        //    {
        //        onlineList = new List<OnlineVisitorsResult>();
        //    }
        //    var ip = RequestInfo.UserAddress;
        //    OnlineVisitorsResult o = new OnlineVisitorsResult();
        //    o.User = new Users();
        //    if (_users != null && _users.Id > 0)
        //    {
        //        var oItem = onlineList.FirstOrDefault(it => it.User.Id == _users.Id);
        //        if (oItem != null)
        //        {
        //            oItem.LastActionTime = DateTime.Now;
        //            oItem.LastActionIp = ip;
        //        }
        //        else
        //        {
        //            o.User = _users;
        //            o.LastActionIp = ip;
        //            o.LastActionTime = DateTime.Now;
        //            onlineList.Add(o);
        //        }
        //    }
        //    else
        //    {
        //        var oItem = onlineList.FirstOrDefault(it => it.LastActionIp == ip);
        //        if (oItem != null)
        //        {
        //            oItem.LastActionTime = DateTime.Now;
        //            oItem.LastActionIp = ip;
        //        }
        //        else
        //        {
        //            o.User = new Users() { RoleId = ConstPubEnum.RoleType.Tourist.TryToInt() };
        //            o.LastActionIp = ip;
        //            o.LastActionTime = DateTime.Now;
        //            onlineList.Add(o);
        //        }
        //    }
        //    cm.Add(key, onlineList, cm.Day);
        //    ViewBag.OnlineVisitorsResult = onlineList;
        //}

        //public bool IsLogin
        //{
        //    get
        //    {
        //        return (_users != null && _users.Id > 0);
        //    }
        //}
        //public void RemoveNewUserListCache()
        //{
        //    var key = ConstPubConst.SessionNewUserList;
        //    var cm = CacheManager<List<Users>>.GetInstance();
        //    if (cm.ContainsKey(key))
        //    {
        //        cm.Remove(key);
        //    }
        //}
        [AllowAnonymous]
        public PartialViewResult LoadMenus()
        {
            return PartialView("_MenusLayout", "");
        }

        [AllowAnonymous]
        public PartialViewResult LoadMsgCenter()
        {
            return PartialView("_MsgCenterLayout", "");
        }

        [AllowAnonymous]
        public PartialViewResult LoadDesktop()
        {
            return PartialView("_DesktopLayout", "");
        }
    }
    public class BaseOutsourcing
    {
    }
}
