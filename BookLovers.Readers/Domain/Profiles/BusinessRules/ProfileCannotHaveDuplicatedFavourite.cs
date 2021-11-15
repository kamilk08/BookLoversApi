using System.Linq;
using BookLovers.Base.Domain.Rules;

namespace BookLovers.Readers.Domain.Profiles.BusinessRules
{
    internal class ProfileCannotHaveDuplicatedFavourite : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "Profile cannot have duplicated favourite";

        private readonly Profile _profile;
        private readonly IFavourite _favourite;

        public ProfileCannotHaveDuplicatedFavourite(Profile profile, IFavourite favourite)
        {
            _profile = profile;
            _favourite = favourite;
        }

        public bool IsFulfilled()
        {
            return !_profile.Favourites.Contains(_favourite);
        }

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}