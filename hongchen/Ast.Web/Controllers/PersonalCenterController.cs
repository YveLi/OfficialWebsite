using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ast.Bll;
using Ast.IBll;
using Ast.Models;
using Ast.Web.Controllers;

namespace Ast.Web.Controllers
{
    public class PersonalCenterController : BaseController
    {
        private IMemberListService memberservice = new MemberListService();
        private IClubPostListService postService = new ClubPostListService();

        #region 个人主页首页
        [AllowAnonymous]
        public ActionResult Index()
        {
            ViewBag.member = CurrentUser;
            return View();
        }
        #endregion

        #region 个人资料
        public ActionResult profile()
        {
            ViewBag.member = CurrentUser;
            return View();
        }
        public JsonResult updateProfile(MemberList member)
        {
            var model = memberservice.GetById(CurrentUserId.ToString());
            model.Email = member.Email;
            model.Name = member.Name;
            model.BirthDay = member.BirthDay;
            model.Sex = member.Sex;
            model.Address = member.Address;
            model.ModifyTime = DateTime.Now;
            memberservice.Update(model);
            return Json(new { success = true, msg = "修改成功", }, JsonRequestBehavior.AllowGet); ;
        }

        public ActionResult myPost(int page = 1, int limit = 10)
        {
            PageModel pagemodel = new PageModel()
            {
                PageIndex = page,
                PageSize = limit
            };
            ViewBag.member = CurrentUser;
            var myPost = postService.GetPageList(pagemodel, o => o.MemberId == CurrentUserId);
            return View(myPost);
        }
        #endregion
    }
}