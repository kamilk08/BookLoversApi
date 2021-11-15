using System;
using System.Collections.Generic;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Base.Domain.Rules;
using BookLovers.Readers.Domain.Profiles.BusinessRules;
using BookLovers.Readers.Events.Profile;

namespace BookLovers.Readers.Domain.Profiles.Services
{
    public class FavouriteBookRemover : IFavouriteRemover
    {
        private readonly List<Func<Profile, FavouriteBook, IBusinessRule>>
            _rules = new List<Func<Profile, FavouriteBook, IBusinessRule>>();

        public FavouriteType FavouriteType => FavouriteType.FavouriteBook;

        public FavouriteBookRemover()
        {
            _rules.Add((profile, favourite) => new AggregateMustBeActive(profile.AggregateStatus != null
                ? profile.AggregateStatus.Value
                : AggregateStatus.Archived.Value));

            _rules.Add((profile, favourite) => new ProfileMustHaveSelectedFavourite(profile, favourite));
        }

        public void RemoveFavourite(Profile profile, IFavourite favourite)
        {
            foreach (var rule in _rules)
            {
                if (!rule(profile, favourite as FavouriteBook).IsFulfilled())
                    throw new BusinessRuleNotMetException(rule(profile, favourite as FavouriteBook).BrokenRuleMessage);
            }

            profile.ApplyChange(new FavouriteBookRemoved(profile.Guid, profile.ReaderGuid, favourite.FavouriteGuid));
        }
    }
}