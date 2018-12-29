using Ast.Bll;
using Ast.Common;
using Ast.IBll;
using Ast.Models;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ast.HcNetWork.Controllers
{
    public class AdminController : Controller
    {
        private readonly Hc_WebSetting _cacheWeb = CacheHelper.GetCache("WebSetting") as Hc_WebSetting;
        private IOTAUsersService userservice = new OTAUsersService();
        private IOTALoginLogService logservice = new OTALoginLogService();
        private IHc_WebSettingService websetservice = new Hc_WebSettingService();
        private IHc_WebMenuService webMenuService = new Hc_WebMenuService();
        private IHc_BannerListService bannerListService = new Hc_BannerListService();
        private IHc_ArticleListService articleListService = new Hc_ArticleListService();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        #region  登录
        public ActionResult Login()
        {
            ViewBag.WebSetting = _cacheWeb;
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
        #endregion

        #region 网站配置
        public ActionResult WebSetting()
        {
            var model = websetservice.GetById("1");
            return View(model);
        }
        [ValidateInput(false)]
        public ActionResult WebSetUpdate(Hc_WebSetting md)
        {
            if (md.Id == 0)
            {
                md.ModifyTime = DateTime.Now;
                md.AddTime = DateTime.Now;
                websetservice.Add(md);
            }
            else
            {
                var model = websetservice.GetById(md.Id.ToString());
                model.Url = md.Url;
                model.Name = md.Name;
                model.SEOKeywords = md.SEOKeywords;
                model.SEODescription = md.SEODescription;
                model.IndexLogo = md.IndexLogo;
                model.Logo = md.Logo;
                model.Wechat = md.Wechat;
                model.QQ = md.QQ;
                model.Tel = md.Tel;
                model.BeiAnNo = md.BeiAnNo;
                model.Adderss = md.Adderss;
                model.Email = md.Email;
                model.Memo = md.Memo;
                model.Map = md.Map;
                model.FullName = md.FullName;
                model.Weibo = md.Weibo;
                model.ModifyTime = DateTime.Now;
                websetservice.Update(model);
            }
            return Content("return sth");
        }

        #endregion

        #region 网站菜单
        public ActionResult WebMenu()
        {
            return View();
        }
        public ActionResult GetWebMenu(string query = "", int page = 1, int limit = 15)
        {
            PageModel pagemodel = new PageModel()
            {
                PageIndex = page,
                PageSize = limit,
            };
            var articleList = webMenuService.GetPageList(pagemodel, o => o.Name.Contains(query));
            return Json(articleList, JsonRequestBehavior.AllowGet);
        }
        public ActionResult InfoWebMenu(int id)
        {
            ViewBag.school = webMenuService.GetList(o => true);
            var model = webMenuService.GetById(id.ToString()) ?? new Hc_WebMenu();
            return View(model);
        }
        public ActionResult DelWebMenu()
        {
            string Id = Request["Id"] ?? "";
            if (Id.Contains(","))
            {
                webMenuService.DeleteByIds(Id.Split(','));
            }
            else
            {
                webMenuService.DeleteById(Id);
            }
            return Content("return sth");
        }
        public ActionResult UpdateWebMenu(Hc_WebMenu md)
        {
            if (md.Id == 0)
            {
                md.AddTime = DateTime.Now;
                md.ModifyTime = DateTime.Now;
                int id = webMenuService.Add(md);
                return Json(new { success = true, msg = "新增成功" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var model = webMenuService.GetById(md.Id.ToString());
                model.Name = md.Name;
                model.Url = md.Url;
                model.Img = md.Img;
                model.EnName = md.EnName;
                model.Memo = md.Memo;
                model.ModifyTime = DateTime.Now;
                model.ThemeName = md.ThemeName;
                model.EnThemeName = md.EnThemeName;
                webMenuService.Update(model);
            }
            return Json(new { success = true, msg = "修改成功" }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 横幅
        public ActionResult Banner()
        {
            return View();
        }
        public ActionResult GetBanner(string query = "", int page = 1,int limit = 15)
        {
            PageModel pagemodel = new PageModel()
            {
                PageIndex = page,
                PageSize = limit,
            };
            var articleList = bannerListService.GetPageList(pagemodel, o => o.Name.Contains(query));
            return Json(articleList, JsonRequestBehavior.AllowGet);
        }
        public ActionResult InfoBanner(int id)
        {
            var model = bannerListService.GetById(id.ToString()) ?? new Hc_BannerList();
            return View(model);
        }
        public ActionResult DelBanner()
        {
            string Id = Request["Id"] ?? "";
            if (Id.Contains(","))
            {
                bannerListService.DeleteByIds(Id.Split(','));
            }
            else
            {
                bannerListService.DeleteById(Id);
            }
            return Content("return sth");
        }
        public ActionResult UpdateBanner(Hc_BannerList md)
        {
            if (md.Id == 0)
            {
                md.AddTime = DateTime.Now;
                int id = bannerListService.Add(md);
                return Json(new { success = true, msg = "新增成功" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var model = bannerListService.GetById(md.Id.ToString());
                model.Name = md.Name;
                model.Url = md.Url;
                model.Img = md.Img;
                model.EnName = md.EnName;
                model.Memo = md.Memo;
                bannerListService.Update(model);
            }
            return Json(new { success = true, msg = "修改成功" }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 文章管理
        public ActionResult Article()
        {
            return View();
        }
        public ActionResult GetArticleList(string query = "", int page = 1,int limit=15)
        {
            PageModel pagemodel = new PageModel()
            {
                PageIndex = page,
                PageSize = limit,
            };
            var articleList = articleListService.GetPageList(pagemodel, o => o.HcContent.Contains(query) || o.Title.Contains(query),o=>o.Sort,0);
            return Json(articleList, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ArticleInfo(int id)
        {
            ViewBag.school = articleListService.GetList(o => true);
            var model = articleListService.GetById(id.ToString()) ?? new Hc_ArticleList();
            return View(model);
        }
        public ActionResult DelArticle()
        {
            string Id = Request["Id"] ?? "";
            if (Id.Contains(","))
            {
                articleListService.DeleteByIds(Id.Split(','));
            }
            else
            {
                articleListService.DeleteById(Id);
            }
            return Content("return sth");
        }
        [ValidateInput(false)]
        public ActionResult UpdateArticle(Hc_ArticleList md)
        {
            if (md.Id == 0)
            {
                md.AddTime = DateTime.Now;
                md.ModifyTime = DateTime.Now;
                int id = articleListService.Add(md);
                return Json(new { success = true, msg = "新增成功" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var model = articleListService.GetById(md.Id.ToString());
                model.TopImg = md.TopImg;
                model.HcContent = md.HcContent;
                model.Title = md.Title;
                model.Brief = md.Brief;
                model.Editor = md.Editor;
                model.Sort = md.Sort;
                model.MarkId = md.MarkId;
                model.IsRecommend = md.IsRecommend;
                model.ModifyTime = DateTime.Now;
                articleListService.Update(model);
            }
            return Json(new { success = true, msg = "修改成功" }, JsonRequestBehavior.AllowGet);
        }
        #endregion

    }
}