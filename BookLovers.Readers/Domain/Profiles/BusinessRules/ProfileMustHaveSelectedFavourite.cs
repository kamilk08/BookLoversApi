using System.Linq;
using BookLovers.Base.Domain.Rules;

namespace BookLovers.Readers.Domain.Profiles.BusinessRules
{
    internal class ProfileMustHaveSelectedFavourite : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "Profile must have selected favourite.";

        private readonly Profile _profile;
        private readonly IFavourite _favourite;

        public ProfileMustHaveSelectedFavourite(Profile profile, IFavourite favourite)
        {
            _profile = profile;
            _favourite = favourite;
        }

        public bool IsFulfilled()
        {
            return _profile.Favourites.Contains(_favourite);
        }

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}