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
using Ast.UI.Models;

namespace Ast.UI.Controllers
{
    public class SysController : UserBaseController
    {
        IOTAFuncMenusService funcservice { get; set; }
        IOTAMenuService menuservice { get; set; }
        IOTAPostService postservice { get; set; }
        IOTAUsersService userservice { get; set; }
        // GET: Sys
        public ActionResult Index()
        {
            return View();
        }
        #region 判断按钮权限
        public ActionResult GetPower(string menuid)
        {
            var _power_list = new Dictionary<string, object>();
            var postmd = postservice.GetSingle(o => o.Id == CurrentPostId && o.OTAMSGUID == OTAMSGUID);
            var powerlist = funcservice.GetList(o => o.OTAMSGUID == OTAMSGUID);
            var arr = postmd.MenuFunc.Split(',');
            var arrs = "";
            foreach (var item in arr)
            {
                var menuarr = item.Split('|');
                if (menuarr[0] == menuid)
                {
                    arrs += menuarr[1] + ",";
                }
            }
            arrs = "," + arrs;
            foreach (var item in powerlist)
            {
                if (arrs.Contains("," + item.Id + ","))
                {
                    _power_list.Add(item.FuncEName, true);
                }
                else
                {
                    _power_list.Add(item.FuncEName, false);
                }
            }
            return Json(_power_list, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region 按钮权限
        public ActionResult FuncMenuIndex()
        {
            return View();
        }
        public ActionResult FuncMenuInfo(int id)
        {
            var model = funcservice.GetById(id.ToString()) ?? new OTAFuncMenus();
            return View(model);
        }
        public ActionResult GetFuncList(string query, int page = 1, int limit = 15)
        {
            PageModel pagemodel = new PageModel()
            {
                PageIndex = page,
                PageSize = limit
            };
            var list = funcservice.GetPageList(pagemodel, (o => (o.FuncCode.Contains(query) || o.FuncEName.Contains(query) || o.FuncName.Contains(query)) && o.OTAMSGUID == OTAMSGUID), o => o.Id, 0);
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FuncUpdate(OTAFuncMenus md)
        {
            if (md.Id == 0)
            {
                md.AddTime = DateTime.Now;
                md.ModifyTime = DateTime.Now;
                md.OTAMSGUID = OTAMSGUID;
                md.CreateUser = CurrentUserId;
                funcservice.Add(md);
            }
            else
            {
                var model = funcservice.GetById(md.Id.ToString());
                model.FuncCode = md.FuncCode;
                model.FuncEName = md.FuncEName;
                model.ModifyTime = DateTime.Now;
                model.FuncName = md.FuncName;
                funcservice.Update(model);
            }
            return Content("");
        }
        #endregion


        #region 菜单权限

        public ActionResult MenuIndex()
        {
            return View();
        }
        public ActionResult MenuInfo(int id)
        {
            var model = menuservice.GetById(id.ToString()) ?? new Ast.Models.OTAMenu();
            ViewBag.Func = funcservice.GetList(o => o.OTAMSGUID == OTAMSGUID).OrderBy(o => o.Id).ToList();
            ViewBag.list = menuservice.GetList(o => o.OTAMSGUID == OTAMSGUID && o.IsDel == 0 && o.ParentId == 0);
            return View(model);
        }
        public ActionResult GetPostList()
        {
            var list = postservice.GetList(o => o.OTAMSGUID == OTAMSGUID).OrderBy(o => o.Id);
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SaveNodes(int id, string menuids, string menufunc)
        {
            var model = postservice.GetById(id.ToString());
            model.MenuId = menuids;
            model.MenuFunc = menufunc;
            postservice.Update(model);
            return Content("");
        }
        public ActionResult MenuUpdate(Ast.Models.OTAMenu md)
        {
            if (md.Id == 0)
            {
                md.AddTime = DateTime.Now;
                md.ModifyTime = DateTime.Now;
                md.OTAMSGUID = OTAMSGUID;
                menuservice.Add(md);
            }
            else
            {
                var model = menuservice.GetById(md.Id.ToString());
                model.Code = md.Code;
                model.Icon = md.Icon;
                model.MenuFunction = md.MenuFunction;
                model.MenuName = md.MenuName;
                model.MenuUrl = md.MenuUrl;
                model.ModifyTime = md.ModifyTime;
                model.ParentId = md.ParentId;
                model.PathCode = md.PathCode;
                model.Sort = md.Sort;
                model.Type = md.Type;
                model.ModifyTime = DateTime.Now;
                menuservice.Update(model);
            }
            return Content("");
        }
        public ActionResult GetMenuList()
        {
            var list = menuservice.GetList(o => o.ParentId == 0 && o.OTAMSGUID == OTAMSGUID).OrderBy(o => o.Sort);
            List<Ast.UI.Models.OTAMenu> menu = new List<Ast.UI.Models.OTAMenu>();
            foreach (var item in list)
            {
                Ast.UI.Models.OTAMenu m = new Ast.UI.Models.OTAMenu
                {
                    Id = item.Id,
                    ParentId = item.ParentId,
                    MenuName = item.MenuName,
                    Icon = item.Icon,
                    MenuUrl = item.MenuUrl,
                    Checked = false,
                    MenuFunction = item.MenuFunction,
                    Children = new List<Ast.UI.Models.OTAMenu>()
                };
                GetChildList(m);
                menu.Add(m);
            }
            return Json(menu, JsonRequestBehavior.AllowGet);
        }
        public void GetChildList(Ast.UI.Models.OTAMenu md)
        {
            var list = menuservice.GetList(o => o.ParentId == md.Id && o.IsDel == 0 && o.OTAMSGUID == OTAMSGUID).OrderBy(o => o.Sort);
            foreach (var item in list)
            {
                Ast.UI.Models.OTAMenu m = new Ast.UI.Models.OTAMenu
                {
                    Id = item.Id,
                    ParentId = item.ParentId,
                    MenuName = item.MenuName,
                    Icon = item.Icon,
                    Checked = false,
                    MenuFunction = item.MenuFunction,
                    MenuUrl = item.MenuUrl,
                    Children = new List<Ast.UI.Models.OTAMenu>()
                };
                if (m.MenuFunction != null)
                {
                    GetThreeMenu(m);// 通过递归实现无限级加载
                }
                md.Children.Add(m);
            }
        }

        public void GetThreeMenu(Ast.UI.Models.OTAMenu md)
        {
            int[] arr = DataConversion.StrArrToIntArr(md.MenuFunction.Split(','));
            var list = funcservice.GetList(o => arr.Contains(o.Id) && o.OTAMSGUID == OTAMSGUID).OrderBy(o => o.Id);
            foreach (var item in list)
            {
                Ast.UI.Models.OTAMenu m = new Ast.UI.Models.OTAMenu
                {
                    Id = item.Id,
                    ParentId = md.Id,
                    MenuName = item.FuncName,
                    Checked = false,
                };
                md.Children.Add(m);
            }
        }
        #endregion
    }
}