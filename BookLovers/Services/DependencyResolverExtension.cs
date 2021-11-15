using System.Web.Http.Controllers;

namespace BookLovers.Services
{
    public static class DependencyResolverExtension
    {
        internal static object GetDependency<T>(this HttpActionContext ctx)
        {
            return ctx.ControllerContext.Configuration.DependencyResolver.GetService(typeof(T));
        }
    }
}