using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Ast.IBll;
using Ast.Bll;
using Ast.Common;

namespace Ast.HcNetWork
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            IHc_WebSettingService menuservice = new Hc_WebSettingService();
            CacheHelper.SetCache("WebSetting", menuservice.GetSingle(o => true));
        }
    }
}
