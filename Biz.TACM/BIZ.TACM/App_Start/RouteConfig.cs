using System.Web.Mvc;
using System.Web.Routing;

namespace Biz.TACM
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapMvcAttributeRoutes();

            routes.MapRoute(
                "Default",
                "{controller}/{action}/{id}",
                new { controller = "Landing", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
