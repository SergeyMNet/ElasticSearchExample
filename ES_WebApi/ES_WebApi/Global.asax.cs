using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using ESService.Interfaces;
using ES_WebApi.App_Start;
using ES_WebApi.Services;
using Ninject;
using Ninject.Web.Common;

namespace ES_WebApi
{
    public class WebApiApplication : NinjectHttpApplication
     {

        protected override IKernel CreateKernel()
        {
            var kernel = new StandardKernel(new NinjectSettings { InjectNonPublic = false, InjectParentPrivateProperties = false, });
            
            kernel.Bind<ISearchService>().To<ESearchService>();

            GlobalConfiguration.Configuration.DependencyResolver = new App_Start.LocalNinjectDependencyResolver(kernel);
            
            return kernel;
        }
        

        //protected override IKernel CreateKernel()
        // {
        //     var kernel = new StandardKernel();
        //     kernel.Load(Assembly.GetExecutingAssembly());

        //    // TODO add Bindings
        //    kernel.Bind<ISearchService>().To<ESearchService>();

        //     return kernel;
        // }
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
