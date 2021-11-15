using System.Threading.Tasks;
using System.Web.Http.Controllers;

namespace BookLovers.Filters
{
    public interface IPermissionHandler
    {
        string PermissionName { get; }

        Task<bool> HasPermission(HttpActionContext ctx);
    }
}