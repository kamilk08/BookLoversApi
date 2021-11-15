using System;
using System.Web;
using BookLovers.Base.Infrastructure.Services;
using BookLovers.Photos;
using BookLovers.Photos.Models;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using Ninject;
using Ninject.Web.Common;
using Ninject.Web.Common.WebHost;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(Startup), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(Startup), "Stop")]

namespace BookLovers.Photos
{
    public class Startup
    {
        private static readonly Bootstrapper Bootstrapper = new Bootstrapper();

        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            Startup.Bootstrapper.Initialize(new Func<IKernel>(Startup.CreateKernel));
        }

        public static void Stop()
        {
            Startup.Bootstrapper.ShutDown();
        }

        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();

            kernel.Bind<Func<IKernel>>().ToMethod(ctx => (() => new Bootstrapper().Kernel));
            kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();
            kernel.Bind<IAppManager>().To<AppManager>();

            var manager = kernel.Get<IAppManager>();

            var photosModule = new PhotosModule(manager);

            kernel.Load(photosModule);

            return kernel;
        }
    }
}