using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using ES_WebApi.App_Start;
using Ninject;
using Ninject.Web.Common;

namespace ES_WebApi
{
    // Origin
    //public class WebApiApplication : System.Web.HttpApplication
    //{
    //    protected void Application_Start()
    //    {
    //        //httpConfiguration.DependencyResolver = new NinjectResolver(NinjectConfig.CreateKernel());

    //        //GlobalConfiguration.Configuration.ServiceResolver
    //        //    .SetResolver(DependencyResolver.Current.ToServiceResolver());

    //        AreaRegistration.RegisterAllAreas();
    //        GlobalConfiguration.Configure(WebApiConfig.Register);
    //        FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
    //        RouteConfig.RegisterRoutes(RouteTable.Routes);
    //        BundleConfig.RegisterBundles(BundleTable.Bundles);
    //    }
    //}


    public class WebApiApplication : NinjectHttpApplication
     {
         protected override IKernel CreateKernel()
         {
             var kernel = new StandardKernel();
             kernel.Load(Assembly.GetExecutingAssembly());

            // TODO add Bind
            //kernel.Bind<IMessageService>().To<MessageService>();

             return kernel;
         }
         protected override void OnApplicationStarted()
         {
             base.OnApplicationStarted();

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
         }
     }
}
