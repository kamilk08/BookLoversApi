using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using BookLovers.Base.Infrastructure.Extensions;

namespace BookLovers.Auth.Infrastructure.Services.Authentication
{
    public class ClaimsBuilder
    {
        private readonly List<Claim> _claims;

        public ClaimsBuilder() => _claims = new List<Claim>();

        public ClaimsBuilder AddSubject<T>(T subject)
        {
            _claims.Add(new Claim("sub", subject.ToString()));
            return this;
        }

        public ClaimsBuilder AddTokenIdentifier()
        {
            _claims.Add(new Claim("jti", Guid.NewGuid().ToString()));
            return this;
        }

        public ClaimsBuilder AddIssuedDate()
        {
            _claims.Add(new Claim("iat", TimeStamper.ToTimeStamp().ToString()));
            return this;
        }

        public ClaimsBuilder AddCustomClaim<T>(string type, T value)
        {
            _claims.Add(new Claim(type, value.ToString()));
            return this;
        }

        public ClaimsBuilder AddEmail(string email)
        {
            _claims.Add(new Claim(nameof(email), email));
            return this;
        }

        public ClaimsBuilder AddRoles(IEnumerable<string> roles)
        {
            foreach (var role in roles)
            {
                if (!role.IsEmpty())
                    _claims.Add(new Claim(nameof(roles), role));
            }

            return this;
        }

        public ClaimsIdentity GetClaimsIdentity() =>
            new ClaimsIdentity(_claims.AsEnumerable(), "Bearer");
    }
}