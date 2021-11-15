using System;
using BookLovers.Base.Domain.ValueObject;

namespace BookLovers.Auth.Domain.Users
{
    public class AccountDetails : ValueObject<AccountDetails>
    {
        public DateTime AccountCreateDate { get; private set; }

        public bool HasBeenBlockedPreviously { get; private set; }

        private AccountDetails()
        {
        }

        public AccountDetails(DateTime accountCreateDate, bool hasBeenBlockedPreviously)
        {
            AccountCreateDate = accountCreateDate;
            HasBeenBlockedPreviously = hasBeenBlockedPreviously;
        }

        protected override int GetHashCodeCore()
        {
            var hash = 17;

            hash = (hash * 23) + this.AccountCreateDate.GetHashCode();
            hash = (hash * 23) + this.HasBeenBlockedPreviously.GetHashCode();

            return hash;
        }

        protected override bool EqualsCore(AccountDetails obj) =>
            this.AccountCreateDate == obj.AccountCreateDate
            && this.HasBeenBlockedPreviously == obj.HasBeenBlockedPreviously;
    }
}