using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookLovers.Base.Domain.Rules;
using BookLovers.Publication.Domain.Authors.BusinessRules;
using BookLovers.Publication.Events.Authors;
using BookLovers.Shared;

namespace BookLovers.Publication.Domain.Authors.Services.Editors
{
    public class AuthorBasicsEditor : IAuthorEditor
    {
        private readonly List<Func<Author, AuthorData, IBusinessRule>> _rules =
            new List<Func<Author, AuthorData, IBusinessRule>>();

        public AuthorBasicsEditor()
        {
            this._rules.Add((author, dto) => new AggregateMustExist(author.Guid));
            this._rules.Add((author, dto) => new AggregateMustBeActive(author.AggregateStatus.Value));
            this._rules.Add((author, dto) => new ValidSexCategory(dto.Basics.Sex));
        }

        public Task Edit(Author author, AuthorData authorDto)
        {
            foreach (var rule in this._rules)
            {
                if (!rule(author, authorDto).IsFulfilled())
                    throw new BusinessRuleNotMetException(rule(author, authorDto).BrokenRuleMessage);
            }

            var authorBasics =
                new AuthorBasics(
                    new FullName(authorDto.Basics.FullName.FirstName, authorDto.Basics.FullName.SecondName),
                    authorDto.Basics.Sex);

            if (authorBasics == author.Basics)
                return Task.CompletedTask;

            var authorBasicsChanged = AuthorBasicsChanged.Initialize()
                .WithAggregate(author.Guid)
                .WithFullName(authorBasics.FullName.FirstName, authorBasics.FullName.SecondName)
                .WithSex(authorBasics.Sex.Value);

            author.ApplyChange(authorBasicsChanged);

            return Task.CompletedTask;
        }
    }
}