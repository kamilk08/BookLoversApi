using System;
using BookLovers.Base.Domain.ValueObject;

namespace BookLovers.Auth.Domain.Users
{
    public class AccountConfirmation : ValueObject<AccountConfirmation>
    {
        public DateTime? ConfirmationDate { get; private set; }

        public bool IsConfirmed { get; private set; }

        private AccountConfirmation()
        {
            ConfirmationDate = null;

            IsConfirmed = false;
        }

        private AccountConfirmation(DateTime confirmationDate)
        {
            ConfirmationDate = confirmationDate;

            IsConfirmed = true;
        }

        public AccountConfirmation(bool isConfirmed, DateTime? confirmationDate)
        {
            ConfirmationDate = confirmationDate;

            IsConfirmed = isConfirmed;
        }

        public static AccountConfirmation NotConfirmed() => new AccountConfirmation();

        public static AccountConfirmation Confirmed(DateTime confirmationDate) =>
            new AccountConfirmation(confirmationDate);

        protected override int GetHashCodeCore()
        {
            var hash = 17;

            hash = (hash * 23) + this.IsConfirmed.GetHashCode();
            hash = (hash * 23) + this.ConfirmationDate.GetHashCode();

            return hash;
        }

        protected override bool EqualsCore(AccountConfirmation obj)
        {
            return this.IsConfirmed == obj.IsConfirmed && this.ConfirmationDate == obj.ConfirmationDate;
        }
    }
}