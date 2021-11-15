using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using BookLovers.Auth.Domain.PasswordResets;
using BookLovers.Auth.Domain.Users.BusinessRules;
using BookLovers.Auth.Domain.Users.Services;
using BookLovers.Auth.Events;
using BookLovers.Base.Domain.Aggregates;

namespace BookLovers.Auth.Domain.Users
{
    public class User : AggregateRoot
    {
        public Account Account { get; private set; }

        public UserName UserName { get; private set; }

        internal ICollection<UserRole> UserRoles { get; private set; } =
            new HashSet<UserRole>();

        public IReadOnlyCollection<UserRole> Roles => UserRoles.ToList();

        private User()
        {
        }

        internal User(Guid readerGuid, UserName userName, Account account)
        {
            this.Guid = readerGuid;
            this.UserName = userName;
            this.Account = account;
            this.Status = AggregateStatus.Active.Value;
            this.AddEvent(new UserCreated(Guid, Account.Email.Value));
        }

        public void ChangePassword(string password, IHashingService hasher)
        {
            CheckBusinessRules(new ChangePasswordRules(this));

            var hashWithSalt = hasher.CreateHashWithSalt(password);

            Account.AccountSecurity =
                new AccountSecurity(hashWithSalt.Item1, hashWithSalt.Item2, Account.AccountSecurity.IsBlocked);
        }

        public void ResetPassword(PasswordResetToken token, string password, IHashingService hasher)
        {
            this.CheckBusinessRules(new ResetPasswordRules(this, token));

            var hashWithSalt = hasher.CreateHashWithSalt(password);

            Account.AccountSecurity =
                new AccountSecurity(hashWithSalt.Item1, hashWithSalt.Item2, Account.AccountSecurity.IsBlocked);
        }

        public void ChangeEmail(string email, IEmailUniquenessChecker uniquenessChecker)
        {
            this.CheckBusinessRules(new ChangeEmailRules(this, uniquenessChecker, email));

            Account.Email = new Email(email);
        }

        public void ConfirmAccount(DateTime confirmationDate)
        {
            this.CheckBusinessRules(new AccountConfirmationRules(Account));

            Account.AccountConfirmation = AccountConfirmation.Confirmed(confirmationDate);
        }

        public void BlockAccount()
        {
            CheckBusinessRules(new AccountCannotBeBlockedRule(Account));

            Account.BlockAccount();
        }

        public bool IsPasswordCorrect(string hash) => Account.AccountSecurity.Hash == hash;

        public UserRole GetRole(string role)
        {
            return this.UserRoles.SingleOrDefault(p => p.Role.Name == role);
        }

        public bool IsInRole(string role) => GetRole(role) != null;

        public class Relations
        {
            public const string RolesCollectionName = "UserRoles";

            public static Expression<Func<User, ICollection<UserRole>>> Roles => c => c.UserRoles;
        }
    }
}