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
using Ast.UI;

namespace Ast.UI.Controllers
{
    public class PersonController : UserBaseController
    {
        private SqlSugarClient db = SqlSugarManage.GetInstance();
        private ISysNoticeListService noticeservice { get; set; }
        private ISysNoticeTypeService noticetypeservice { get; set; }
        // GET: Person
        public ActionResult NoticeIndex()
        {
            return View();
        }
        public ActionResult GetNoticeList(string query, int page = 1, int limit = 15)
        {
            PageModel pagemodel = new PageModel()
            {
                PageIndex = page,
                PageSize = limit
            };
            var list = noticeservice.GetPageList(pagemodel, (o => o.Title.Contains(query) && o.OTAMSGUID == OTAMSGUID));
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public ActionResult NoticeInfo(int id)
        {
            ViewBag.typelist = noticetypeservice.GetList(o => o.OTAMSGUID == OTAMSGUID);
            var model = noticeservice.GetById(id.ToString()) ?? new SysNoticeList();
            return View(model);
        }
        [ValidateInput(false)]
        public ActionResult NoticeUpdate(SysNoticeList md)
        {
            if (md.Id == 0)
            {
                md.AddTime = DateTime.Now;
                md.ModifyTime = DateTime.Now;
                md.OTAMSGUID = OTAMSGUID;
                md.PushUserId = CurrentUserId;
                md.PushUserName = CurrentUserName;
                noticeservice.Add(md);
            }
            else
            {
                var model = noticeservice.GetById(md.Id.ToString());
                model.Title = md.Title;
                model.PushTime = md.PushTime;
                model.PushUserId = CurrentUserId;
                model.PushUserName = CurrentUserName;
                model.NoticeType = md.NoticeType;
                model.NoticeTypeName = md.NoticeTypeName;
                model.Content = md.Content;
                model.ModifyTime = DateTime.Now;
                noticeservice.Update(model);
            }
            return Content("");
        }
        public ActionResult NoticeDel()
        {
            string Id = Request["Id"] ?? "";
            if (Id.Contains(","))
            {
                var sql = String.Format("DELETE FROM SysNoticeList Where Id In ({0})", Id);
                db.Ado.ExecuteCommand(sql);
            }
            else
            {
                noticeservice.DeleteById(Id);
            }
            return Content("return sth");
        }

        public ActionResult NoticeType()
        {
            return View();
        }
        public ActionResult NoticeTypeInfo(int id)
        {
            var model = noticetypeservice.GetById(id.ToString()) ?? new SysNoticeType();
            return View(model);
        }
        public ActionResult GetNoticeTypeList(string query, int page = 1, int limit = 15)
        {
            PageModel pagemodel = new PageModel()
            {
                PageIndex = page,
                PageSize = limit
            };
            var list = noticetypeservice.GetPageList(pagemodel, (o => o.Name.Contains(query) && o.OTAMSGUID == OTAMSGUID));
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public ActionResult NoticeTypeUpdate(SysNoticeType md)
        {
            if (md.Id == 0)
            {
                md.AddTime = DateTime.Now;
                md.ModifyTime = DateTime.Now;
                md.OTAMSGUID = OTAMSGUID;
                noticetypeservice.Add(md);
            }
            else
            {
                var model = noticetypeservice.GetById(md.Id.ToString());
                model.ModifyTime = DateTime.Now;
                model.Name = md.Name;
                noticetypeservice.Update(model);
            }
            return Content("");
        }
        public ActionResult NoticeTypeDel()
        {
            string Id = Request["Id"] ?? "";
            if (Id.Contains(","))
            {
                var sql = String.Format("DELETE FROM SysNoticeType Where Id In ({0})", Id);
                db.Ado.ExecuteCommand(sql);
            }
            else
            {
                noticetypeservice.DeleteById(Id);
            }
            return Content("return sth");
        }
    }
}