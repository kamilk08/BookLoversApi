using BookLovers.Auth.Domain.Roles;

namespace BookLovers.Auth.Domain.Users.Services.Factories
{
    public class UserFactorySetup
    {
        private readonly UserFactory _userFactory;

        public UserFactorySetup(
            IUserNameUniquenessChecker userNameChecker,
            IEmailUniquenessChecker emailChecker,
            IHashingService hashingService,
            IRoleProvider roleProvider)
        {
            _userFactory = new UserFactory()
                .Set(userNameChecker)
                .Set(emailChecker)
                .Set(hashingService)
                .Set(roleProvider);
        }

        public UserFactory SetupFactory(UserFactoryData factoryData) =>
            _userFactory.Set(factoryData);
    }
}