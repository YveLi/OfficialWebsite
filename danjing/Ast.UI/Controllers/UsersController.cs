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
    public class UsersController : UserBaseController
    {
        private SqlSugarClient db = SqlSugarManage.GetInstance();
        private IOTAUsersService userservice { get; set; }
        private IOTADepartmentService departmentservice { get; set; }
        private IOTAPostService postservice { get; set; }
        // GET: Users
        #region 用户列表
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetUsersList(string query, int pid = 0, int page = 1, int limit = 15)
        {
            PageModel pagemodel = new PageModel()
            {
                PageIndex = page,
                PageSize = limit
            };
            if (pid == 0)
            {
                var list = userservice.GetPageList(pagemodel, (o => (o.LoginName.Contains(query) || o.RealName.Contains(query)) && o.IsDel != 1 && o.OTAMSGUID == OTAMSGUID));
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var list = userservice.GetPageList(pagemodel, (o => (o.LoginName.Contains(query) || o.RealName.Contains(query)) && o.IsDel != 1 && o.OTAMSGUID == OTAMSGUID && o.DepartmentId == pid));
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Info(int id)
        {
            var model = userservice.GetById(id.ToString()) ?? new OTAUsers();
            ViewBag.departlist = departmentservice.GetList(o => o.ParentId != 0 && o.OTAMSGUID == OTAMSGUID && o.IsDel == 0);
            ViewBag.postlist = postservice.GetList(o => o.OTAMSGUID == OTAMSGUID && o.IsDel == 0);
            return View(model);
        }
        public ActionResult UsersUpdate(OTAUsers md)
        {
            var msg = "";
            if (md.Id == 0)
            {
                var model = userservice.GetSingle(o => o.LoginName == md.LoginName);
                if (model == null)
                {
                    md.PassWord = CommFun.Md5(md.PassWord);
                    md.AddTime = DateTime.Now;
                    md.ModifyTime = DateTime.Now;
                    md.OTAMSGUID = OTAMSGUID;
                    userservice.Add(md);
                }
                else
                {
                    msg = "账号已存在！";
                }
            }
            else
            {
                var model = userservice.GetById(md.Id.ToString());
                if (model.PassWord != md.PassWord)
                {
                    model.PassWord = CommFun.Md5(md.PassWord);
                }
                model.DepartName = md.DepartName;
                model.PostName = md.PostName;
                model.RealName = md.RealName;
                model.IsLeader = md.IsLeader;
                model.Mobile = md.Mobile;
                model.Img = md.Img;
                model.Gender = md.Gender;
                model.Email = md.Email;
                model.PostId = md.PostId;
                model.DepartmentId = md.DepartmentId;
                model.Address = md.Address;
                model.ModifyTime = DateTime.Now;
                userservice.Update(model);
            }
            return Content(msg);
        }
        public ActionResult UsersDel()
        {
            string Id = Request["Id"] ?? "";
            if (Id.Contains(","))
            {
                var sql = String.Format("Update DailyTripList Set IsDel=1 Where Id In ({0})", Id);
                db.Ado.ExecuteCommand(sql);
            }
            else
            {
                var md = userservice.GetById(Id);
                md.IsDel = 1;
                userservice.Update(md);
            }
            return Content("return sth");
        }
        #endregion

        #region  部门
        public ActionResult GetDepartData(string keyword)
        {
            var list = departmentservice.GetList(o => o.PostName.Contains(keyword) && (o.ParentId == 0 || o.ParentId == 1 || o.ParentId == 2));
            var lists = new List<SelectData>();
            foreach (var item in list)
            {
                var selcet = new SelectData()
                {
                    name = item.PostName,
                    value = item.Id,
                    selected = "",
                    disabled = "",
                };
                lists.Add(selcet);
            }
            return Json(new DataList<SelectData>(lists), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetDepartList()
        {
            var list = departmentservice.GetList(e => e.ParentId == 0 && e.OTAMSGUID == OTAMSGUID && e.IsDel == 0);
            List<PointEntity> enterprise = new List<PointEntity>();
            foreach (var item in list)
            {
                PointEntity et = new PointEntity
                {
                    id = item.Id,
                    pid = item.ParentId,
                    name = item.PostName,
                    isParent = true,
                    children = new List<PointEntity>()
                };
                GetChildList(et);
                enterprise.Add(et);
            }
            return Json(enterprise, JsonRequestBehavior.AllowGet);
        }
        public void GetChildList(PointEntity et)
        {
            var list = departmentservice.GetList(e => e.ParentId == et.id && e.IsDel == 0);
            foreach (var item in list)
            {
                PointEntity entity = new PointEntity
                {
                    id = item.Id,
                    pid = item.ParentId,
                    name = item.PostName,
                    isParent = true,
                    children = new List<PointEntity>()
                };
                GetChildList(entity);     // 通过递归实现无限级加载
                et.children.Add(entity);
            }
        }
        public ActionResult DepartInfo(int id)
        {
            var model = departmentservice.GetById(id.ToString()) ?? new OTADepartment();
            return View(model);
        }
        public ActionResult DepartUpdate(OTADepartment md)
        {
            if (md.Id == 0)
            {
                md.AddTime = DateTime.Now;
                md.ModifyTime = DateTime.Now;
                md.OTAMSGUID = OTAMSGUID;
                departmentservice.Add(md);
            }
            else
            {
                var model = departmentservice.GetById(md.Id.ToString());
                model.PostName = md.PostName;
                model.ParentId = md.ParentId;
                model.PathCode = md.PathCode;
                model.Memo = md.Memo;
                model.FullName = md.FullName;
                departmentservice.Update(model);
            }
            return Content("");
        }
        public ActionResult DepartDel()
        {
            var msg = "0";
            string Id = Request["Id"] ?? "";
            if (Id.Contains(","))
            {
                var sql = String.Format("Update OTADepartMentList Set IsDel=1 Where Id In ({0})", Id);
                db.Ado.ExecuteCommand(sql);
            }
            else
            {
                var md = departmentservice.GetById(Id);
                var model = departmentservice.GetList(o => o.ParentId == md.Id && o.IsDel == 0);
                if (model.Count == 0)
                {
                    md.IsDel = 1;
                    departmentservice.Update(md);
                    msg = "";
                }
            }
            return Content(msg);
        }
        #endregion

        #region 职位
        public ActionResult Post()
        {
            return View();
        }
        public ActionResult GetPostList(string query, int page = 1, int limit = 15)
        {
            PageModel pagemodel = new PageModel()
            {
                PageIndex = page,
                PageSize = limit
            };
            var list = postservice.GetPageList(pagemodel, (o => (o.PostName.Contains(query) || o.PostNo.Contains(query)) && o.IsDel == 0 && o.OTAMSGUID == OTAMSGUID));
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public ActionResult PostInfo(int id)
        {
            var model = postservice.GetById(id.ToString()) ?? new OTAPost();
            return View(model);
        }
        public ActionResult PostUpdate(OTAPost md)
        {
            if (md.Id == 0)
            {
                md.AddTime = DateTime.Now;
                md.ModifyTime = DateTime.Now;
                md.OTAMSGUID = OTAMSGUID;
                postservice.Add(md);
            }
            else
            {
                var model = postservice.GetById(md.Id.ToString());
                model.PostName = md.PostName;
                model.PostNo = md.PostNo;
                model.PostDesc = md.PostDesc;
                postservice.Update(model);
            }
            return Content("");
        }
        public ActionResult PostDel()
        {
            string Id = Request["Id"] ?? "";
            if (Id.Contains(","))
            {
                var sql = String.Format("Update OTAPost Set IsDel=1 Where Id In ({0})", Id);
                db.Ado.ExecuteCommand(sql);
            }
            else
            {
                var md = postservice.GetById(Id);
                md.IsDel = 1;
                postservice.Update(md);
            }
            return Content("return sth");
        }
        #endregion
    }
}