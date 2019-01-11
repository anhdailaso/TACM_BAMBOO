//using Autofac;
//using Autofac.Integration.Mvc;
//using Biz.TACM.Infrastructure.Modules;
using Microsoft.Owin;
using Owin;
//using System;
//using System.Web.Mvc;

[assembly: OwinStartupAttribute(typeof(Biz.TACM.Startup))]
namespace Biz.TACM
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //ConfigureAuth(app);

            //var builder = new ContainerBuilder();

            //// STANDARD MVC SETUP:
            //var executingAssemblies = AppDomain.CurrentDomain.GetAssemblies();
            //builder.RegisterAssemblyTypes(executingAssemblies)
            //       .Where(t => t.Name.ToLower().EndsWith("repository")
            //       || t.Name.ToLower().EndsWith("service"))
            //       .AsImplementedInterfaces()
            //       .InstancePerLifetimeScope();

            //builder.RegisterModule(new InfrastructureModule());

            //// Register your MVC controllers.
            //builder.RegisterControllers(typeof(MvcApplication).Assembly);

            //// Run other optional steps, like registering model binders,
            //// web abstractions, etc., then set the dependency resolver
            //// to be Autofac.
            //var container = builder.Build();
            //DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            //// OWIN MVC SETUP:

            //// Register the Autofac middleware FIRST, then the Autofac MVC middleware.
            //app.UseAutofacMiddleware(container);
            //app.UseAutofacMvc();
        }
    }
}
