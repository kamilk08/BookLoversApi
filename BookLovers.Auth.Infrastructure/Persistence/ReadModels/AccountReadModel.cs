using System;

namespace BookLovers.Auth.Infrastructure.Persistence.ReadModels
{
    public class AccountReadModel
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public bool IsAccountConfirmed { get; set; }

        public DateTime? ConfirmationDate { get; set; }

        public DateTime AccountCreateDate { get; set; }

        public bool HasBeenBlockedPreviously { get; set; }

        public string Hash { get; set; }

        public string Salt { get; set; }

        public bool IsBlocked { get; set; }
    }
}