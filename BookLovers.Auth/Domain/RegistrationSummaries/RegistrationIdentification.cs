using System;
using BookLovers.Base.Domain.ValueObject;

namespace BookLovers.Auth.Domain.RegistrationSummaries
{
    public class RegistrationIdentification : ValueObject<RegistrationIdentification>
    {
        public Guid UserGuid { get; private set; }

        public string Email { get; private set; }

        private RegistrationIdentification()
        {
        }

        public RegistrationIdentification(Guid userGuid, string email)
        {
            UserGuid = userGuid;
            Email = email;
        }

        protected override int GetHashCodeCore()
        {
            var hash = 17;

            hash = (hash * 23) + this.Email.GetHashCode();
            hash = (hash * 23) + this.UserGuid.GetHashCode();

            return hash;
        }

        protected override bool EqualsCore(RegistrationIdentification obj)
        {
            return Email == obj.Email && UserGuid == obj.UserGuid;
        }
    }
}