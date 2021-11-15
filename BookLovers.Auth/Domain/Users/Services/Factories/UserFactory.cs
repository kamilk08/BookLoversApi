using System;
using System.Collections.Generic;
using BookLovers.Auth.Domain.Roles;
using BookLovers.Auth.Domain.Users.BusinessRules;
using BookLovers.Base.Domain.Rules;

namespace BookLovers.Auth.Domain.Users.Services.Factories
{
    public class UserFactory : IUserFactory
    {
        private readonly List<Func<User, IBusinessRule>> _businessRules =
            new List<Func<User, IBusinessRule>>();

        private IHashingService _hashingService;
        private IRoleProvider _roleProvider;
        private IEmailUniquenessChecker _emailChecker;
        private IUserNameUniquenessChecker _userNameChecker;
        private UserFactoryData _factoryData;

        internal UserFactory(UserFactoryData factoryData)
        {
            _factoryData = factoryData;
        }

        public UserFactory()
            : this(UserFactoryData.Initialize())
        {
            _businessRules.Add(user => new AggregateMustBeActive(user.Status));
            _businessRules.Add(user => new AccountCannotBeBlockedRule(user.Account));
            _businessRules.Add(user => new EmailMustBeUnique(_emailChecker, user.Account.Email.Value));
            _businessRules.Add(user => new UserNameMustBeUnique(_userNameChecker, user.UserName.Value));
        }

        public User CreateUser()
        {
            var user = new User(
                _factoryData.BasicData.UserGuid,
                new UserName(_factoryData.BasicData.Username),
                CreateAccount());

            user.UserRoles.Add(_roleProvider.GetRole(Role.Reader));

            foreach (var businessRule in _businessRules)
            {
                if (!businessRule(user).IsFulfilled())
                    throw new BusinessRuleNotMetException(businessRule(user).BrokenRuleMessage);
            }

            return user;
        }

        internal UserFactory Set(IRoleProvider roleProvider)
        {
            _roleProvider = roleProvider;

            return this;
        }

        internal UserFactory Set(IUserNameUniquenessChecker userNameChecker)
        {
            _userNameChecker = userNameChecker;

            return this;
        }

        internal UserFactory Set(IEmailUniquenessChecker emailUniquenessChecker)
        {
            _emailChecker = emailUniquenessChecker;

            return this;
        }

        internal UserFactory Set(IHashingService hashingService)
        {
            _hashingService = hashingService;

            return this;
        }

        internal UserFactory Set(UserFactoryData factoryData)
        {
            _factoryData = factoryData;

            return this;
        }

        private Account CreateAccount()
        {
            var hashWithSalt = _hashingService.CreateHashWithSalt(_factoryData.AccountData.Password);

            return new Account(
                new Email(_factoryData.AccountData.Email),
                new AccountSecurity(hashWithSalt.Item1, hashWithSalt.Item2, false),
                new AccountDetails(_factoryData.AccountData.AccountCreateDate, false));
        }
    }
}