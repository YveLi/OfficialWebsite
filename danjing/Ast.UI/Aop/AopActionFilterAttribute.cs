using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;

namespace Ast.UI.Aop
{
    public class AopActionFilterAttribute : ActionFilterAttribute
    {
        private bool _IsExecute { get; set; }

        public AopActionFilterAttribute(bool IsExecute = true)
        {
            this._IsExecute = IsExecute;
        }

        /// <summary>
        /// 在行为方法执行后执行
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);
        }

        /// <summary>
        /// 每次请求Action之前发生，，在行为方法执行前执行
        /// </summary>
        /// <param name="context"></param>
        //public override void OnActionExecuting(ActionExecutingContext context)
        //{

        //    if (this._IsExecute)
        //    {
        //        //登陆超时验证
        //        this.CheckedLoginAccount(context);
        //    }

        //    base.OnActionExecuting(context);
        //}

        //public override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        //{
        //    return base.OnActionExecutionAsync(context, next);
        //}

        /// <summary>
        /// 在行为方法返回后执行
        /// </summary>
        /// <param name="context"></param>
        public override void OnResultExecuted(ResultExecutedContext context)
        {
            base.OnResultExecuted(context);
        }

        /// <summary>
        /// 在行为方法返回前执行，判断session是否为空,重写这个方法即可实现
        /// </summary>
        /// <param name="context"></param>
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            base.OnResultExecuting(context);
        }

        //public override Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        //{
        //    return base.OnResultExecutionAsync(context, next);
        //}




        ///// <summary>
        ///// 检查登录帐户
        ///// </summary>
        //private void CheckedLoginAccount(ActionExecutingContext context)
        //{
        //    //判断是否为 ajax 请求
        //    var IsAjaxRequest = context.HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest";

        //    //var _RouteValues = context.ActionDescriptor.RouteValues;
        //    //var _Area = _RouteValues["area"];
        //    //var _Controller = _RouteValues["controller"];
        //    //var _Action = _RouteValues["action"];

        //    var accountM = Tools.GetSession<Sys_AccountM>("Account");

        //    if (accountM == null || accountM.UserID.GetGuid() == Guid.Empty)
        //    {
        //        if (IsAjaxRequest)
        //        {
        //            context.Result = new JsonResult(new ErrorModel(AppConfig.LoginPageUrl, EMsgStatus.登录超时20));
        //        }
        //        else
        //        {
        //            context.Result = new ContentResult()
        //            {
        //                Content = @"<script type='text/javascript'>
        //                                alert('登录超时！系统将退出重新登录！');
        //                                top.window.location='" + AppConfig.LoginPageUrl + @"';
        //                            </script>",
        //                ContentType = "text/html;charset=utf-8;"
        //            };
        //        }
        //        return;
        //    }
        //}








    }
}