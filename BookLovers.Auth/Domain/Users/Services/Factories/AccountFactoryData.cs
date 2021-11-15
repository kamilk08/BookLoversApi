using System;

namespace BookLovers.Auth.Domain.Users.Services.Factories
{
    public class AccountFactoryData
    {
        public string Email { get; }

        public string Password { get; }

        public DateTime AccountCreateDate { get; }

        public AccountFactoryData(string email, string password)
        {
            Email = email;
            Password = password;
            AccountCreateDate = DateTime.UtcNow;
        }
    }
}