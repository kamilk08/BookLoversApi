using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookLovers.Base.Domain.Rules;
using BookLovers.Publication.Events.Authors;

namespace BookLovers.Publication.Domain.Authors.Services.Editors
{
    public class AuthorDetailsEditor : IAuthorEditor
    {
        private readonly List<Func<Author, IBusinessRule>> _rules;

        public AuthorDetailsEditor()
        {
            this._rules = new List<Func<Author, IBusinessRule>>();

            this._rules.Add(author => new AggregateMustBeActive(author.AggregateStatus.Value));
        }

        public Task Edit(Author author, AuthorData authorDto)
        {
            foreach (var rule in this._rules)
            {
                if (!rule(author).IsFulfilled())
                    throw new BusinessRuleNotMetException(rule(author).BrokenRuleMessage);
            }

            var authorDetails = new AuthorDetails(
                authorDto.Details.BirthPlace,
                authorDto.Details.LifeLength.BirthDate,
                authorDto.Details.LifeLength.DeathDate);

            if (authorDetails == author.Details)
                return Task.CompletedTask;

            var authorDetailsChanged = AuthorDetailsChanged.Initialize()
                .WithAggregate(author.Guid)
                .WithBirthPlace(authorDetails.BirthPlace)
                .WithDates(authorDetails.BirthDate, authorDetails.DeathDate);

            author.ApplyChange(authorDetailsChanged);

            return Task.CompletedTask;
        }
    }
}