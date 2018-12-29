using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ast.Bll;
using Ast.IBll;
using Ast.Models;
using Ast.Common;

namespace Ast.UI.Controllers
{
    public class UserController : UserBaseController
    {
        private IUsersService userservice = new UsersService();
        private ISchoolListService schoolservice = new SchoolListService();
        // GET: User
        #region 学生模块

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Info(int id)
        {
            ViewBag.school = schoolservice.GetList(o => true);
            var model = userservice.GetById(id.ToString()) ?? new Users();
            return View(model);
        }
        public ActionResult GetUsersList(string query="", int page = 1, int limit = 15)
        {
            PageModel pagemodel = new PageModel()
            {
                PageIndex = page,
                PageSize = limit
            };
            var list = userservice.GetPageList(pagemodel, o => o.UserName.Contains(query));
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Update(Users md)
        {
            var isSuccess = false;
            if (md.Id == 0)
            {
                md.AddTime = DateTime.Now;
                md.ModifyTime = DateTime.Now;
                int id = userservice.Add(md);
                isSuccess = id > 0;
            }
            else
            {
                var model = userservice.GetById(md.Id.ToString());
                model.UserName = md.UserName;
                model.Age = md.Age;
                model.School = md.School;
                model.Memo = md.Memo;
                model.ModifyTime = DateTime.Now;
                isSuccess = userservice.Update(model);
            }
            return Content(isSuccess ? "1" : "0");
        }
        public ActionResult Del()
        {
            string Id = Request["Id"] ?? "";
            if (Id.Contains(","))
            {
                userservice.DeleteByIds(Id.Split(','));
            }
            else
            {
                userservice.DeleteById(Id);
            }
            return Content("return sth");
        }
        #endregion

        #region 学校模块

        public ActionResult School()
        {
            return View();
        }
        public ActionResult InfoSchool(int id)
        {
            ViewBag.school = schoolservice.GetList(o => true);
            var model = schoolservice.GetById(id.ToString()) ?? new SchoolList();
            return View(model);
        }
        public ActionResult GetUsersListSchool(string query, int page = 1, int limit = 15)
        {
            PageModel pagemodel = new PageModel()
            {
                PageIndex = page,
                PageSize = limit
            };
            var list = schoolservice.GetPageList(pagemodel, o => o.Name.Contains(query) || o.Address.Contains(query));
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public ActionResult UpdateSchool(SchoolList md)
        {
            var isSuccess = false;
            if (md.Id == 0)
            {
                md.AddTime = DateTime.Now;
                md.ModifyTime = DateTime.Now;
                int id = schoolservice.Add(md);
                isSuccess = id > 0;
            }
            else
            {
                var model = schoolservice.GetById(md.Id.ToString());
                model.Name = md.Name;
                model.Address = md.Address;
                model.ModifyTime = DateTime.Now;
                isSuccess = schoolservice.Update(model);
            }
            return Content(isSuccess ? "1" : "0");
        }
        public ActionResult DelSchool()
        {
            string Id = Request["Id"] ?? "";
            if (Id.Contains(","))
            {
                schoolservice.DeleteByIds(Id.Split(','));
            }
            else
            {
                schoolservice.DeleteById(Id);
            }
            return Content("return sth");
        }
        #endregion
    }
}