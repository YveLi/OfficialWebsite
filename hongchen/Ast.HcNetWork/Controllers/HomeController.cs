using System;
using System.Configuration;
using System.Web.Mvc;
using Ast.Bll;
using Ast.Common;
using Ast.HcNetWork;
using Ast.IBll;
using Ast.Models;

namespace Ast.Web.Controllers
{
    public class HomeController : Controller
    {
        private IHc_WebSettingService webservice = new Hc_WebSettingService();
        private IHc_WebMenuService menuservice = new Hc_WebMenuService();
        private IHc_ArticleListService articleserview = new Hc_ArticleListService();
        private IHc_BannerListService bannerservice = new Hc_BannerListService();
        public ActionResult Index()
        {
            ViewBag.webset = webservice.GetById("1");
            ViewBag.menu = menuservice.GetList(o => true, o => o.Id, 0);
            ViewBag.mymenu = menuservice.GetList(o=>true);
            ViewBag.banner = bannerservice.GetList(o => true, o => o.Sort, 0);
            return View();
        }
        public ActionResult About()
        {
            ViewBag.webset = webservice.GetById("1");
            ViewBag.menu = menuservice.GetList(o => true, o => o.Id, 0);
            ViewBag.mymenu = menuservice.GetById("2");
            ViewBag.prolist = articleserview.GetList(o => o.MenuId == 1,o=>o.Sort,0);
            return View();
        }
        public ActionResult News()
        {
            ViewBag.webset = webservice.GetById("1");
            ViewBag.menu = menuservice.GetList(o => true, o => o.Id, 0);
            ViewBag.mymenu = menuservice.GetById("3");
            ViewBag.prolist = articleserview.GetList(o => o.MenuId == 2,o=>o.Sort,0);
            return View();
        }
        public ActionResult Contact()
        {
            ViewBag.webset = webservice.GetById("1");
            ViewBag.mymenu = menuservice.GetById("4");
            ViewBag.menu = menuservice.GetList(o => true, o => o.Id, 0);
            return View();
        }
        public ActionResult Login()
        {
            ViewBag.webset = webservice.GetById("1");
            ViewBag.menu = menuservice.GetList(o => true, o => o.Id, 0);
            ViewBag.mymenu = menuservice.GetById("5");
            return View();
        }
        public ActionResult ArticleDetail(int id = 0)
        {
            ViewBag.webset = webservice.GetById("1");
            ViewBag.menu = menuservice.GetList(o => true, o => o.Id, 0);
            ViewBag.detail = articleserview.GetById(id.ToString());
            var nav = articleserview.GetList(o => o.MenuId==2,o=>o.Sort,0);
            foreach(var item in nav)
            {
                if (item.Id == id)
                {
                    int index = nav.IndexOf(item);
                    ViewBag.Index = index;
                }
            }
            ViewBag.nav = nav;
            return View();
        }
    }
}