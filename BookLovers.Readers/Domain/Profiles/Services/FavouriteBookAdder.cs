using System;
using System.Collections.Generic;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Base.Domain.Rules;
using BookLovers.Readers.Domain.Profiles.BusinessRules;
using BookLovers.Readers.Events.Profile;

namespace BookLovers.Readers.Domain.Profiles.Services
{
    public class FavouriteBookAdder : IFavouriteAdder
    {
        private readonly List<Func<Profile, FavouriteBook, IBusinessRule>> _rules =
            new List<Func<Profile, FavouriteBook, IBusinessRule>>();

        public FavouriteType FavouriteType => FavouriteType.FavouriteBook;

        public FavouriteBookAdder()
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
                if (!rule(profile, favourite as FavouriteBook).IsFulfilled())
                    throw new BusinessRuleNotMetException(rule(profile, favourite as FavouriteBook).BrokenRuleMessage);
            }

            var favouriteBookAdded = FavouriteBookAdded
                .Initialize()
                .WithAggregate(profile.Guid)
                .WithReader(profile.ReaderGuid)
                .WithBook(favourite.FavouriteGuid)
                .WithFavouriteType(favourite.FavouriteType.Value);

            profile.ApplyChange(favouriteBookAdded);
        }
    }
}