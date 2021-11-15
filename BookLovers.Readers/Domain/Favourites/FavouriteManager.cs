using System;
using System.Collections.Generic;
using System.Linq;
using BookLovers.Base.Domain.Rules;
using BookLovers.Base.Domain.Services;
using BookLovers.Readers.Events.Favourites;

namespace BookLovers.Readers.Domain.Favourites
{
    public class FavouriteManager : IAggregateManager<Favourite>
    {
        private List<Func<Favourite, IBusinessRule>> _rules = new List<Func<Favourite, IBusinessRule>>();

        public FavouriteManager()
        {
            _rules.Add(aggregate => new AggregateMustExist(aggregate.Guid));
            _rules.Add(aggregate => new AggregateMustBeActive(aggregate.AggregateStatus.Value));
        }

        public void Archive(Favourite aggregate)
        {
            foreach (var rule in _rules)
            {
                if (!rule(aggregate).IsFulfilled())
                    throw new BusinessRuleNotMetException(rule(aggregate).BrokenRuleMessage);
            }

            aggregate.ApplyChange(new FavouriteArchived(
                aggregate.FavouriteGuid,
                aggregate.FavouriteOwners.Select(s => s.OwnerGuid).ToList()));
        }
    }
}