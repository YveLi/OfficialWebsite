using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ast.Common;
using Ast.IBll;
using SqlSugar;
using Ast.Models;
using Ast.Models.ViewModels;
using System.Configuration;

namespace Ast.UI.Controllers
{
    public class MainController : UserBaseController
    {
        private readonly IList<OTAMenu> _cacheMenus = CacheHelper.GetCache("menus") as List<OTAMenu>;
        private IOTAPostService postservice { get; set; }
        private ISysNoticeListService noticeservice { get; set; }
        public ActionResult Index()
        {
            ViewBag.noticelist = noticeservice.GetList(o => o.OTAMSGUID == OTAMSGUID);
            ViewBag.UserName = CurrentRealName;
            ViewBag.Name = ConfigurationManager.AppSettings["webname"];
            ViewBag.Title = ConfigurationManager.AppSettings["webtitle"]; return View();
        }
        public ActionResult Home()
        {
            return View();
        }
        public ActionResult LoginOut()
        {
            Session.Remove("LoginUser");
            return Content(string.Empty);
        }
        public ActionResult GetMenu()
        {
            var list = _cacheMenus.Where(o => o.ParentId == 0 && o.OTAMSGUID == OTAMSGUID).OrderBy(o => o.Sort);
            List<Ast.UI.Models.OTAMenu> menu = new List<Ast.UI.Models.OTAMenu>();
            var postmd = postservice.GetSingle(o => o.Id == CurrentPostId);
            var arr = "," + postmd.MenuId + ",";
            foreach (var item in list)
            {
                if (arr.Contains("," + item.Id + ","))
                {
                    Ast.UI.Models.OTAMenu m = new Ast.UI.Models.OTAMenu
                    {
                        Id = item.Id,
                        ParentId = item.ParentId,
                        MenuName = item.MenuName,
                        Icon = item.Icon,
                        MenuUrl = item.MenuUrl,
                        Children = new List<Ast.UI.Models.OTAMenu>()
                    };
                    GetChildList(m, arr);
                    menu.Add(m);
                }
            }
            return Json(menu, JsonRequestBehavior.AllowGet);
        }
        public void GetChildList(Ast.UI.Models.OTAMenu md, string arr)
        {
            var list = _cacheMenus.Where(o => o.ParentId == md.Id && o.IsDel == 0 && o.OTAMSGUID == OTAMSGUID).OrderBy(o => o.Sort); ;
            foreach (var item in list)
            {
                if (arr.Contains("," + item.Id + ","))
                {
                    Ast.UI.Models.OTAMenu m = new Ast.UI.Models.OTAMenu
                    {
                        Id = item.Id,
                        ParentId = item.ParentId,
                        MenuName = item.MenuName,
                        Icon = item.Icon,
                        MenuUrl = item.MenuUrl,
                        Children = new List<Ast.UI.Models.OTAMenu>()
                    };
                    md.Children.Add(m);
                }
            }
        }
        public JsonResult GetTwoMenus(int pid)
        {
            var list = _cacheMenus.Where(o => o.ParentId == pid && o.OTAMSGUID == OTAMSGUID);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

    }
}