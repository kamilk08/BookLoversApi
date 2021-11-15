using System;
using BookLovers.Base.Domain.Rules;

namespace BookLovers.Readers.Domain.Profiles.BusinessRules
{
    internal class ProfileFavouriteMustBeValid : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "Profile favourite is not valid.";

        private readonly IFavourite _favourite;

        public ProfileFavouriteMustBeValid(IFavourite favourite)
        {
            _favourite = favourite;
        }

        public bool IsFulfilled()
        {
            return _favourite.FavouriteGuid != Guid.Empty;
        }

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}