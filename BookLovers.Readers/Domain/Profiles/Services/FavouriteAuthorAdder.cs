using System;
using System.Collections.Generic;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Base.Domain.Rules;
using BookLovers.Readers.Domain.Profiles.BusinessRules;
using BookLovers.Readers.Events.Profile;

namespace BookLovers.Readers.Domain.Profiles.Services
{
    public class FavouriteAuthorAdder : IFavouriteAdder
    {
        private List<Func<Profile, FavouriteAuthor, IBusinessRule>> _rules =
            new List<Func<Profile, FavouriteAuthor, IBusinessRule>>();

        public FavouriteType FavouriteType => FavouriteType.FavouriteAuthor;

        public FavouriteAuthorAdder()
        {
            _rules.Add((profile, favourite) => new AggregateMustBeActive(profile.AggregateStatus != null
                ? profile.AggregateStatus.Value
                : AggregateStatus.Archived.Value));

            _rules.Add((profile, favourite) => new ProfileFavouriteMustBeValid(favourite));
            _rules.Add((profile, favourite) => new ProfileCannotHaveDuplicatedFavourite(profile, favourite));
        }

        public void AddFavourite(Profile profile, IFavourite favourite)
        {
            foreach (var rule in _rules)
            {
                if (!rule(profile, favourite as FavouriteAuthor).IsFulfilled())
                    throw new BusinessRuleNotMetException(rule(profile, favourite as FavouriteAuthor)
                        .BrokenRuleMessage);
            }

            var favouriteAuthorAdded = FavouriteAuthorAdded
                .Initialize().WithAggregate(profile.Guid)
                .WithReader(profile.ReaderGuid).WithAuthor(favourite.FavouriteGuid)
                .WithFavouriteType(FavouriteType.Value);

            profile.ApplyChange(favouriteAuthorAdded);
        }
    }
}