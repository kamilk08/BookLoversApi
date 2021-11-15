using System;
using System.Collections.Generic;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Base.Domain.Rules;
using BookLovers.Readers.Domain.Profiles.BusinessRules;
using BookLovers.Readers.Events.Profile;

namespace BookLovers.Readers.Domain.Profiles.Services
{
    public class FavouriteAuthorRemover : IFavouriteRemover
    {
        private readonly List<Func<Profile, FavouriteAuthor, IBusinessRule>> _rules =
            new List<Func<Profile, FavouriteAuthor, IBusinessRule>>();

        public FavouriteType FavouriteType => FavouriteType.FavouriteAuthor;

        public FavouriteAuthorRemover()
        {
            _rules.Add((profile, author) => new AggregateMustBeActive(profile.AggregateStatus != null
                ? profile.AggregateStatus.Value
                : AggregateStatus.Archived.Value));

            _rules.Add((profile, author) => new ProfileMustHaveSelectedFavourite(profile, author));
        }

        public void RemoveFavourite(Profile profile, IFavourite favourite)
        {
            foreach (var rule in _rules)
            {
                if (!rule(profile, favourite as FavouriteAuthor).IsFulfilled())
                    throw new BusinessRuleNotMetException(rule(profile, favourite as FavouriteAuthor)
                        .BrokenRuleMessage);
            }

            profile.ApplyChange(new FavouriteAuthorRemoved(profile.Guid, favourite.FavouriteGuid, profile.ReaderGuid));
        }
    }
}