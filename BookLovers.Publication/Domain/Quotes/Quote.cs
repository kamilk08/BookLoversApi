using System;
using System.Collections.Generic;
using System.Linq;
using BookLovers.Base.Domain;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Base.Domain.Events;
using BookLovers.Publication.Domain.Quotes.BusinessRules;
using BookLovers.Publication.Events.Quotes;
using BookLovers.Publication.Mementos;
using BookLovers.Shared.Likes;

namespace BookLovers.Publication.Domain.Quotes
{
    [AllowSnapshot]
    public class Quote :
        EventSourcedAggregateRoot,
        IHandle<BookQuoteAdded>,
        IHandle<AuthorQuoteAdded>,
        IHandle<QuoteArchived>,
        IHandle<QuoteLiked>,
        IHandle<QuoteUnLiked>
    {
        private List<Like> _likes = new List<Like>();

        public QuoteContent QuoteContent { get; private set; }

        public QuoteDetails QuoteDetails { get; private set; }

        public IReadOnlyList<Like> Likes => this._likes;

        private Quote()
        {
        }

        public Quote(Guid quoteGuid, QuoteContent quoteContent, QuoteDetails details)
        {
            this.Guid = quoteGuid;
            this.QuoteContent = quoteContent;
            this.QuoteDetails = details;
            this.AggregateStatus = AggregateStatus.Active;
        }

        public void AddLike(Like like)
        {
            this.CheckBusinessRules(new LikeQuoteRules(this, like));

            this.ApplyChange(new QuoteLiked(this.Guid, like.ReaderGuid));
        }

        public void UnLike(Like like)
        {
            this.CheckBusinessRules(new UnlikeQuoteRules(this, like));

            this.ApplyChange(new QuoteUnLiked(this.Guid, like.ReaderGuid));
        }

        public Like GetLike(Guid readerGuid) =>
            this._likes.Find(p => p.ReaderGuid == readerGuid);

        void IHandle<BookQuoteAdded>.Handle(BookQuoteAdded @event)
        {
            this.Guid = @event.AggregateGuid;
            this.QuoteContent = new QuoteContent(@event.Quote, @event.BookGuid);
            this.QuoteDetails = new QuoteDetails(@event.AddedBy, @event.AddedAt, QuoteType.BookQuote);
            this.AggregateStatus = AggregateStatus.Active;
        }

        void IHandle<AuthorQuoteAdded>.Handle(AuthorQuoteAdded @event)
        {
            this.Guid = @event.AggregateGuid;
            this.QuoteContent = new QuoteContent(@event.Quote, @event.AuthorGuid);
            this.QuoteDetails = new QuoteDetails(@event.AddedBy, @event.AddedAt, QuoteType.AuthorQuote);
            this.AggregateStatus = AggregateStatus.Active;
        }

        void IHandle<QuoteLiked>.Handle(QuoteLiked @event)
        {
            this._likes.Add(Like.NewLike(@event.AddedBy));
        }

        void IHandle<QuoteUnLiked>.Handle(QuoteUnLiked @event)
        {
            var like = this._likes.SingleOrDefault(p => p.ReaderGuid == @event.UnlikedByGuid);

            this._likes.Remove(like);
        }

        void IHandle<QuoteArchived>.Handle(QuoteArchived @event) =>
            this.AggregateStatus = AggregateStatus.Archived;

        public override void ApplySnapshot(IMemento memento)
        {
            var quoteMemento = memento as IQuoteMemento;

            this.Guid = quoteMemento.AggregateGuid;
            this.AggregateStatus = AggregateStates.Get(quoteMemento.AggregateStatus);
            this.Version = quoteMemento.Version;
            this.LastCommittedVersion = quoteMemento.LastCommittedVersion;

            var quotedGuid = quoteMemento.AuthorGuid == Guid.Empty ? quoteMemento.BookGuid : quoteMemento.AuthorGuid;
            this.QuoteContent = new QuoteContent(quoteMemento.QuoteContent, quotedGuid);
            this.QuoteDetails = new QuoteDetails(quoteMemento.AddedByGuid, quoteMemento.AddedAt,
                QuoteType.Get(quoteMemento.QuoteTypeId));

            this._likes = quoteMemento.QuoteLikes.Select(Like.NewLike).ToList();
        }
    }
}