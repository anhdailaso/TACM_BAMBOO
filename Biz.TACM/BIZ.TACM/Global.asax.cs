using System;
//using System.Linq;
//using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
//using AutoMapper;
using WebGrease.Css.Extensions;
using Biz.Lib.Helpers;

namespace Biz.TACM
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError().GetBaseException();
            WriteLog.Error(ex + "\r\n", AppName.BizSecurity);
        }

        protected void Application_EndRequest()
        {
            var context = new HttpContextWrapper(Context);
            //need to login
            if (Context.Response.StatusCode == 308 && context.Request.IsAjaxRequest())
            {
                Context.Response.Clear();
                Context.Response.StatusCode = 308;
            }
            //un-authorized
            else if (Context.Response.StatusCode == 309 && context.Request.IsAjaxRequest())
            {
                Context.Response.Clear();
                Context.Response.StatusCode = 309;
            }
        }
    }
}
