using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Ast.IBll;
using Ast.Bll;
using Ast.Common;

namespace Ast.UI
{
    public class MvcApplication : Spring.Web.Mvc.SpringMvcApplication //System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            log4net.Config.XmlConfigurator.Configure();
            IOTAMenuService menuservice= new OTAMenuService();
            CacheHelper.SetCache("menus", menuservice.GetList(o=>o.IsDel==0));
        }
    }
}
