using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookLovers.Base.Domain.Rules;
using BookLovers.Publication.Events.Authors;

namespace BookLovers.Publication.Domain.Authors.Services.Editors
{
    public class AuthorDescriptionEditor : IAuthorEditor
    {
        private readonly List<Func<Author, IBusinessRule>> _rules;

        public AuthorDescriptionEditor()
        {
            this._rules = new List<Func<Author, IBusinessRule>>();
            this._rules.Add(author => new AggregateMustExist(author.Guid));
            this._rules.Add(author => new AggregateMustBeActive(author.AggregateStatus.Value));
        }

        public Task Edit(Author author, AuthorData authorDto)
        {
            foreach (var rule in this._rules)
            {
                if (!rule(author).IsFulfilled())
                    throw new BusinessRuleNotMetException(rule(author).BrokenRuleMessage);
            }

            var authorDescription = new AuthorDescription(
                authorDto.Description.AboutAuthor,
                authorDto.Description.WebSite,
                authorDto.Description.DescriptionSource);

            if (author.Description == authorDescription)
                return Task.CompletedTask;

            var descriptionChanged = AuthorDescriptionChanged.Initialize()
                .WithAggregate(author.Guid)
                .WithDescription(authorDescription.AboutAuthor, authorDescription.DescriptionSource)
                .WithWebSite(authorDescription.AuthorWebsite);

            author.ApplyChange(descriptionChanged);

            return Task.CompletedTask;
        }
    }
}