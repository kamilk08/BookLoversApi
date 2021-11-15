namespace BookLovers.Auth.Domain.Users.Services
{
    public class UserAuthenticationService
    {
        private readonly IHashingService _hashingService;

        public UserAuthenticationService(IHashingService hashingService)
        {
            _hashingService = hashingService;
        }

        public bool Authenticate(User user, string password)
        {
            return user != null && user.Account.IsAccountActive() &&
                   this.AreHashesEqual(_hashingService.GetHash(password, user.Account.AccountSecurity.Salt), user);
        }

        private bool AreHashesEqual(string generateHash, User user)
        {
            return generateHash == user.Account.AccountSecurity.Hash;
        }
    }
}