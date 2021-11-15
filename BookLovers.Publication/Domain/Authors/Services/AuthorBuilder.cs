using System;
using System.Collections.Generic;
using BookLovers.Base.Domain.Builders;
using BookLovers.Base.Infrastructure.Extensions;
using BookLovers.Shared;
using BookLovers.Shared.SharedSexes;

namespace BookLovers.Publication.Domain.Authors.Services
{
    public class AuthorBuilder : IBuilder<Author>, IBuilder
    {
        private readonly IList<Action<Author>> _builderActions =
            new List<Action<Author>>();

        internal Author Author { get; private set; }

        internal AuthorBuilder InitializeAuthor(
            Guid authorGuid,
            FullName fullName,
            Sex sex)
        {
            this.Author = new Author(authorGuid, fullName, sex);

            return this;
        }

        internal AuthorBuilder AddAuthorDetails(
            string birthPlace,
            DateTime? birthDate,
            DateTime? deathDate)
        {
            var authorDetails = new AuthorDetails(birthPlace, birthDate, deathDate);

            this._builderActions.Add(author => author.Details = authorDetails);

            return this;
        }

        internal AuthorBuilder AddDescription(
            string description,
            string webSite,
            string descriptionSource)
        {
            var authorDescription = new AuthorDescription(description, webSite, descriptionSource);

            this._builderActions.Add(author => author.Description = authorDescription);

            return this;
        }

        public Author Build()
        {
            this._builderActions.ForEach(ba => ba(this.Author));

            this._builderActions.Clear();

            return this.Author;
        }
    }
}