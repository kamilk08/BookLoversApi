using BookLovers.Auth.Domain.Roles;

namespace BookLovers.Auth.Domain.Users.Services.Factories
{
    public class SuperAdminFactorySetup
    {
        private readonly SuperAdminFactory _userFactory;

        public SuperAdminFactorySetup(
            IUserNameUniquenessChecker userNameChecker,
            IEmailUniquenessChecker emailChecker,
            IHashingService hashingService,
            IRoleProvider roleProvider)
        {
            _userFactory = new SuperAdminFactory()
                .Set(userNameChecker)
                .Set(emailChecker)
                .Set(hashingService)
                .Set(roleProvider);
        }

        public SuperAdminFactory SetupFactory(UserFactoryData factoryData) =>
            _userFactory.Set(factoryData);
    }
}