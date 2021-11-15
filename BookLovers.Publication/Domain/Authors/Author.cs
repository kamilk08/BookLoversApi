using System;
using System.Collections.Generic;
using System.Linq;
using BookLovers.Base.Domain;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Base.Domain.Events;
using BookLovers.Base.Domain.Rules;
using BookLovers.Publication.Domain.Authors.BusinessRules;
using BookLovers.Publication.Events.Authors;
using BookLovers.Publication.Mementos;
using BookLovers.Shared;
using BookLovers.Shared.Categories;
using BookLovers.Shared.SharedSexes;

namespace BookLovers.Publication.Domain.Authors
{
    [AllowSnapshot]
    public class Author :
        EventSourcedAggregateRoot,
        IHandle<AuthorCreated>,
        IHandle<AuthorBookAdded>,
        IHandle<AuthorBookRemoved>,
        IHandle<AuthorBasicsChanged>,
        IHandle<AuthorDetailsChanged>,
        IHandle<AuthorDescriptionChanged>,
        IHandle<AuthorArchived>,
        IHandle<AuthorFollowed>,
        IHandle<AuthorUnFollowed>,
        IHandle<AuthorGenreAdded>,
        IHandle<AuthorGenreRemoved>,
        IHandle<QuoteAddedToAuthor>,
        IHandle<QuoteRemovedFromAuthor>
    {
        private List<AuthorBook> _books = new List<AuthorBook>();
        private List<Follower> _followers = new List<Follower>();
        private List<SubCategory> _authorGenres = new List<SubCategory>();
        private List<AuthorQuote> _authorQuotes = new List<AuthorQuote>();

        public AuthorBasics Basics { get; private set; }

        public AuthorDetails Details { get; internal set; }

        public AuthorDescription Description { get; internal set; }

        public IReadOnlyList<AuthorBook> Books => this._books;

        public IReadOnlyList<Follower> Followers => this._followers;

        public IReadOnlyList<SubCategory> Genres => this._authorGenres;

        public IReadOnlyList<AuthorQuote> AuthorQuotes => this._authorQuotes;

        protected Author()
        {
        }

        public Author(Guid authorGuid, FullName fullName, Sex sex)
        {
            this.Guid = authorGuid;
            this.Basics = new AuthorBasics(fullName, sex);
            this.Description = AuthorDescription.Default();
            this.Details = AuthorDetails.Default();
            this.AggregateStatus = AggregateStatus.Active;
        }

        public void AddBook(AuthorBook book)
        {
            this.CheckBusinessRules(new AddAuthorBookRules(this, book));

            this.ApplyChange(new AuthorBookAdded(this.Guid, book.BookGuid));
        }

        public void RemoveBook(AuthorBook book)
        {
            this.CheckBusinessRules(new RemoveAuthorBookBusinessRules(this, book));

            this.ApplyChange(new AuthorBookRemoved(this.Guid, book.BookGuid));
        }

        public void AddFollower(Follower follower)
        {
            this.CheckBusinessRules(new AddAuthorFollowerRules(this, follower));

            this.ApplyChange(new AuthorFollowed(this.Guid, follower.FollowedBy));
        }

        public void RemoveFollower(Follower follower)
        {
            this.CheckBusinessRules(new RemoveAuthorFollowerBusinessRule(this, follower));

            this.ApplyChange(new AuthorUnFollowed(this.Guid, follower.FollowedBy));
        }

        public void AddGenre(SubCategory subCategory)
        {
            this.CheckBusinessRules(new AddAuthorGenreRules(this, subCategory));

            this.ApplyChange(new AuthorGenreAdded(this.Guid, subCategory.Value));
        }

        public void RemoveGenre(SubCategory subCategory)
        {
            this.CheckBusinessRules(new RemoveAuthorGenreRules(this, subCategory));

            this.ApplyChange(new AuthorGenreRemoved(this.Guid, subCategory.Value));
        }

        public void AddQuote(AuthorQuote authorQuote)
        {
            this.CheckBusinessRules(new AggregateMustBeActive(this.AggregateStatus.Value));

            this.ApplyChange(new QuoteAddedToAuthor(this.Guid, authorQuote.QuoteGuid));
        }

        public void RemoveQuote(AuthorQuote authorQuote)
        {
            this.CheckBusinessRules(new AggregateMustBeActive(this.AggregateStatus.Value));

            this.ApplyChange(new QuoteRemovedFromAuthor(this.Guid, authorQuote.QuoteGuid));
        }

        public Follower GetFollower(Guid followerGuid) => this._followers.Find(s => s.FollowedBy == followerGuid);

        public bool HasFollower(Guid followerGuid) => this.GetFollower(followerGuid) != null;

        public AuthorBook GetBook(Guid bookGuid) => this._books.Find(p => p.BookGuid == bookGuid);

        public AuthorQuote GetAuthorQuote(Guid quoteGuid) =>
            this._authorQuotes.SingleOrDefault(p => p.QuoteGuid == quoteGuid);

        void IHandle<AuthorBookAdded>.Handle(AuthorBookAdded @event)
        {
            this._books.Add(new AuthorBook(@event.BookGuid));
        }

        void IHandle<AuthorBookRemoved>.Handle(AuthorBookRemoved @event)
        {
            this._books.Remove(this._books.Single(p => p.BookGuid == @event.BookGuid));
        }

        void IHandle<AuthorCreated>.Handle(AuthorCreated @event)
        {
            this.Guid = @event.AggregateGuid;
            this.Basics = new AuthorBasics(new FullName(@event.FirstName, @event.SecondName), Sexes.Get(@event.Sex));
            this.Details = new AuthorDetails(@event.BirthPlace, @event.BirthDate, @event.DeathDate);
            this.Description =
                new AuthorDescription(@event.AboutAuthor, @event.AuthorWebsite, @event.DescriptionSource);
            this.AggregateStatus = AggregateStatus.Active;
        }

        void IHandle<AuthorBasicsChanged>.Handle(AuthorBasicsChanged @event)
        {
            this.Basics = new AuthorBasics(new FullName(@event.FirstName, @event.SecondName), Sexes.Get(@event.SexId));
        }

        void IHandle<AuthorDetailsChanged>.Handle(AuthorDetailsChanged @event)
        {
            this.Details = new AuthorDetails(@event.BirthPlace, @event.BirthDate, @event.DeathDate);
        }

        void IHandle<AuthorArchived>.Handle(AuthorArchived @event)
        {
            this.Guid = @event.AggregateGuid;
            this.AggregateStatus = AggregateStatus.Archived;
        }

        void IHandle<AuthorDescriptionChanged>.Handle(
            AuthorDescriptionChanged @event)
        {
            this.Description =
                new AuthorDescription(@event.AboutAuthor, @event.AuthorWebsite, @event.DescriptionSource);
        }

        void IHandle<AuthorFollowed>.Handle(AuthorFollowed @event)
        {
            this._followers.Add(new Follower(@event.FollowedBy));
        }

        void IHandle<AuthorUnFollowed>.Handle(AuthorUnFollowed @event)
        {
            var follower = this._followers.Single(p => p.FollowedBy == @event.FollowedBy);

            this._followers.Remove(follower);
        }

        void IHandle<AuthorGenreAdded>.Handle(AuthorGenreAdded @event)
        {
            var genre = SubCategoryList.SubCategories.Single(p => p.Value == @event.SubCategoryId);

            this._authorGenres.Add(genre);
        }

        void IHandle<AuthorGenreRemoved>.Handle(AuthorGenreRemoved @event)
        {
            var genre = this._authorGenres.Find(p => p.Value == @event.SubCategoryId);

            this._authorGenres.Remove(genre);
        }

        void IHandle<QuoteAddedToAuthor>.Handle(QuoteAddedToAuthor @event)
        {
            this._authorQuotes.Add(new AuthorQuote(@event.QuoteGuid));
        }

        void IHandle<QuoteRemovedFromAuthor>.Handle(
            QuoteRemovedFromAuthor @event)
        {
            var quote = this._authorQuotes.Single(p => p.QuoteGuid == @event.QuoteGuid);

            this._authorQuotes.Remove(quote);
        }

        public override void ApplySnapshot(IMemento memento)
        {
            var authorMemento = memento as IAuthorMemento;

            this.Guid = authorMemento.AggregateGuid;
            this.AggregateStatus = AggregateStates.Get(authorMemento.AggregateStatus);
            this.Version = authorMemento.Version;
            this.LastCommittedVersion = authorMemento.LastCommittedVersion;

            this.Basics = new AuthorBasics(
                new FullName(authorMemento.FirstName, authorMemento.SecondName),
                Sexes.Get(authorMemento.Sex));
            this.Details =
                new AuthorDetails(authorMemento.BirthPlace, authorMemento.BirthDate, authorMemento.DeathDate);
            this.Description = new AuthorDescription(authorMemento.AboutAuthor, authorMemento.AuthorWebsite,
                authorMemento.DescriptionSource);

            this._books = authorMemento.AuthorBooks.Select(s => new AuthorBook(s)).ToList();
            this._followers = authorMemento.Followers.Select(p => new Follower(p)).ToList();
            this._authorQuotes = authorMemento.Quotes.Select(s => new AuthorQuote(s))
                .ToList();
            this._authorGenres = SubCategoryList.SubCategories
                .Where(p => authorMemento.Genres.Contains(p.Value)).ToList();
        }
    }
}