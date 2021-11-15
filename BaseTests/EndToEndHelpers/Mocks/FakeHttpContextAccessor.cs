using System;
using BookLovers.Base.Infrastructure.Services;

namespace BaseTests.EndToEndHelpers.Mocks
{
    public class FakeHttpContextAccessor : IHttpContextAccessor
    {
        private readonly bool _isLibrarian;
        private readonly bool _isSuperAdmin;

        public Guid UserGuid { get; }

        public bool IsAuthenticated { get; }

        public FakeHttpContextAccessor()
        {
        }

        public FakeHttpContextAccessor(Guid userGuid, bool isAuthenticated)
        {
            UserGuid = userGuid;
            IsAuthenticated = isAuthenticated;
        }

        public FakeHttpContextAccessor(
            Guid userGuid,
            bool isAuthenticated,
            bool isLibrarian,
            bool isSuperAdmin)
        {
            UserGuid = userGuid;
            IsAuthenticated = isAuthenticated;
            _isLibrarian = isLibrarian;
            _isSuperAdmin = isSuperAdmin;
        }

        public bool IsLibrarian() => _isLibrarian;

        public bool IsReader() => true;

        public bool IsSuperAdmin() => _isSuperAdmin;
    }
}