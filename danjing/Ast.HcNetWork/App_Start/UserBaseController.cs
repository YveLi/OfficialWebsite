using Ast.Models;
using Ast.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ast.HcNetWork
{
    public class UserBaseController : BaseController
    {
        private readonly Hc_WebSetting _cacheWeb = CacheHelper.GetCache("WebSetting") as Hc_WebSetting;
        public UserBaseController()
        {
            OTAUsers user = CommFun.GetSession("LoginUser") as OTAUsers;
            if (user == null)
                return;
            CurrentUserId = user.Id;
            CurrentUserName = user.LoginName;
            CurrentRealName = user.RealName;
            OTAMSGUID = user.OTAMSGUID;
            IsAdmin = user.IsAdmin;
            IsLeader = user.IsLeader;
            CurrentUserType = user.UserType;
            CurrentDepartmentId = user.DepartmentId;
            CurrentPostId = user.PostId;
            var hcwebset = _cacheWeb;
            if (hcwebset == null)
                return;
            CurrentWebSet = hcwebset;
        }
        protected Hc_WebSetting CurrentWebSet { get; set; }
        protected string OTAMSGUID { get; set; }
        protected int CurrentUserId { get; set; }
        protected string CurrentUserName { get; set; }
        protected int CurrentUserType { get; set; }
        protected int CurrentDepartmentId { get; set; }
        protected int IsLeader { get; set; }
        protected int IsAdmin { get; set; }
        protected string CurrentRealName { get; set; }
        protected int CurrentPostId { get; set; }
    }
}