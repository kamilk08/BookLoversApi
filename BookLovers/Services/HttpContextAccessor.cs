using System;
using System.Security.Claims;
using System.Web;
using BookLovers.Auth.Domain.Users;
using BookLovers.Base.Infrastructure.Services;

namespace BookLovers.Services
{
    public class HttpContextAccessor : IHttpContextAccessor
    {
        public Guid UserGuid => GetUserGuid();

        public bool IsAuthenticated => UserGuid != Guid.Empty;

        public bool IsLibrarian()
        {
            return ClaimsPrincipal.Current.IsInRole(Role.Librarian.Name);
        }

        public bool IsReader()
        {
            return ClaimsPrincipal.Current.IsInRole(Role.Reader.Name);
        }

        public bool IsSuperAdmin()
        {
            return ClaimsPrincipal.Current.IsInRole(Role.SuperAdmin.Name);
        }

        private Guid GetUserGuid()
        {
            if (!(HttpContext.Current.User is ClaimsPrincipal user))
                return Guid.Empty;

            Guid.TryParse(
                user?.FindFirst(p => p.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")
                    ?.Value, out var result);

            return result;
        }
    }
}