using System;

namespace BookLovers.Auth.Domain.Users.Services.Factories
{
    public class UserFactoryData
    {
        public BasicsFactoryData BasicData { get; private set; }

        public AccountFactoryData AccountData { get; private set; }

        private UserFactoryData()
        {
        }

        internal UserFactoryData(BasicsFactoryData basicData, AccountFactoryData accountData)
        {
            BasicData = basicData;
            AccountData = accountData;
        }

        public static UserFactoryData Initialize() => new UserFactoryData();

        public UserFactoryData WithBasics(Guid userGuid, string username)
        {
            BasicData = new BasicsFactoryData(userGuid, username);

            return this;
        }

        public UserFactoryData WithAccount(string email, string password)
        {
            AccountData = new AccountFactoryData(email, password);

            return this;
        }
    }
}