using System;
using System.Collections.Generic;
using System.Linq;
using BookLovers.Base.Domain.Rules;
using BookLovers.Publication.Domain.Authors.BusinessRules;
using BookLovers.Publication.Domain.BookReaders;
using BookLovers.Publication.Events.Authors;
using BookLovers.Shared;
using BookLovers.Shared.Categories;

namespace BookLovers.Publication.Domain.Authors.Services
{
    public class AuthorFactory
    {
        private readonly AuthorBuilder _authorBuilder;

        private readonly List<Func<Author, BookReader, IBusinessRule>> _rules =
            new List<Func<Author, BookReader, IBusinessRule>>();

        public AuthorFactory(
            AuthorBuilder authorBuilder,
            IAuthorUniquenessChecker checker)
        {
            this._authorBuilder = authorBuilder;

            this._rules.Add((author, bookReader) => new BookReaderMustBeValid(bookReader));
            this._rules.Add((author, bookReader) => new AggregateMustBeActive(author.AggregateStatus.Value));
            this._rules.Add((author, bookReader) => new ValidSexCategory(author.Basics.Sex));
            this._rules.Add((author, bookReader) => new AllAuthorGenresMustBeValid(author));
            this._rules.Add((author, bookReader) => new AuthorMustHaveSecondName(author.Basics.FullName));
            this._rules.Add((author, bookReader) => new AuthorMustBeUnique(checker, author.Guid));
        }

        public Author CreateAuthor(AuthorData data)
        {
            var fullName = new FullName(data.Basics.FullName.FirstName, data.Basics.FullName.SecondName);
            var author = this._authorBuilder
                .InitializeAuthor(data.AuthorGuid, fullName, data.Basics.Sex)
                .AddAuthorDetails(data.Details.BirthPlace, data.Details.LifeLength.BirthDate,
                    data.Details.LifeLength.DeathDate)
                .AddDescription(
                    data.Description.AboutAuthor,
                    data.Description.WebSite, data.Description.DescriptionSource)
                .Build();

            foreach (var rule in this._rules)
            {
                if (!rule(author, data.BookReader).IsFulfilled())
                    throw new BusinessRuleNotMetException(rule(author, data.BookReader).BrokenRuleMessage);
            }

            var authorCreated = this.CreateEvent(data, author);

            author.ApplyChange(authorCreated);

            data.Genres.ForEach(genreId =>
                author.AddGenre(SubCategoryList.SubCategories.Single(p => p.Value == genreId)));

            data.AuthorBooks.ForEach(bookGuid => author.AddBook(new AuthorBook(bookGuid)));

            return author;
        }

        private AuthorCreated CreateEvent(AuthorData data, Author author)
        {
            return AuthorCreated.Initialize()
                .WithAggregate(author.Guid)
                .WithFullName(author.Basics.FullName.FirstName, author.Basics.FullName.SecondName)
                .WithSex(author.Basics.Sex.Value).WithAddedBy(data.BookReader.ReaderGuid)
                .WithDescription(author.Description.AboutAuthor, author.Description.AuthorWebsite,
                    author.Description.DescriptionSource).WithDetails(
                    author.Details.BirthPlace,
                    author.Details.BirthDate,
                    author.Details.DeathDate);
        }
    }
}