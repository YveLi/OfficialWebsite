using System;
using System.Configuration;
using System.Web;
using System.Web.Mvc;
using Ast.Bll;
using Ast.Common;
using Ast.IBll;
using Ast.Models;
using Ast.Web.Authentication;
using SqlSugar;

namespace Ast.Web.Controllers
{
    public class LoginController : BaseController
    {
        private IMemberListService memberservice = new MemberListService();
        // GET: Loginasdad
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Register(MemberList register)
        {
            return View();
        }
        public JsonResult MemberRegister(MemberList register)
        {
            var isSuccess = false;
            PageModel pagemodel = new PageModel()
            {
                PageIndex = 1,
                PageSize = 100,
            };
            //判断邮箱是否已被注册
            var list = memberservice.GetPageList(pagemodel, o => o.Email.Contains(register.Email));
            if (list.count > 0)
            {
                return Json(new { success = false, msg = "该邮箱已被注册！" }, JsonRequestBehavior.AllowGet);
            }
            var list2 = memberservice.GetPageList(pagemodel, o => o.Name.Contains(register.Name));
            if (list2.count > 0)
            {
                return Json(new { success = false, msg = "这个昵称已被注册！" }, JsonRequestBehavior.AllowGet);
            }
            register.Password = CommFun.Md5(register.Password);
            register.AddTime = DateTime.Now;
            register.ModifyTime = DateTime.Now;
            int id = memberservice.Add(register);
            isSuccess = id > 0;
            return Json(new { success = true, msg = "注册成功！快返回登陆", }, JsonRequestBehavior.AllowGet); ;
        }

        [AllowAnonymous]
        public JsonResult Go(LoginUser user)
        {
            user.PassWord = CommFun.Md5(user.PassWord);
            var userinfo = memberservice.GetSingle(u => u.LoginName == user.LoginName || u.Email == user.LoginName && u.Password == user.PassWord);
            if (userinfo == null)
            {
                return Json(new { success = false, msg = "帐号或密码错误！" }, JsonRequestBehavior.AllowGet);
            }
            new AspFormsAuthentication().SetAuthenticationToken(userinfo.Id + "$" + userinfo.Email + "$" + userinfo.Name);
            HttpCookie cookie = new HttpCookie("Email");
            cookie.Value = userinfo.Email;
            cookie.Expires = DateTime.Now.AddDays(7);
            Response.Cookies.Set(cookie);
            return Json(new
            {
                success = true,
                msg = "登录成功！",
                user = userinfo
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LogOut()
        {
            new AspFormsAuthentication().SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}