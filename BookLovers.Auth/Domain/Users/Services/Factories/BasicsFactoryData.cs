using System;

namespace BookLovers.Auth.Domain.Users.Services.Factories
{
    public class BasicsFactoryData
    {
        public Guid UserGuid { get; }

        public string Username { get; }

        public BasicsFactoryData(Guid userGuid, string username)
        {
            UserGuid = userGuid;
            Username = username;
        }
    }
}