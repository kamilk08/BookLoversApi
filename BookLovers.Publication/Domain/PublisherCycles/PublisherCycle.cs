using System;
using System.Collections.Generic;
using System.Linq;
using BookLovers.Base.Domain;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Base.Domain.Events;
using BookLovers.Publication.Domain.PublisherCycles.BusinessRules;
using BookLovers.Publication.Events.PublisherCycles;
using BookLovers.Publication.Mementos;

namespace BookLovers.Publication.Domain.PublisherCycles
{
    [AllowSnapshot]
    public class PublisherCycle :
        EventSourcedAggregateRoot,
        IHandle<PublisherCycleCreated>,
        IHandle<BookAddedToCycle>,
        IHandle<BookRemovedFromCycle>,
        IHandle<PublisherCycleArchived>,
        IHandle<PublisherCycleRestored>
    {
        private List<CycleBook> _books = new List<CycleBook>();

        public string CycleName { get; private set; }

        public Guid PublisherGuid { get; private set; }

        public IReadOnlyList<CycleBook> Books => this._books;

        private PublisherCycle()
        {
        }

        public PublisherCycle(Guid cycleGuid, Guid publisherGuid, string cycleName)
        {
            this.Guid = cycleGuid;
            this.CycleName = cycleName;
            this.PublisherGuid = publisherGuid;
            this.AggregateStatus = AggregateStatus.Active;
            this.ApplyChange(new PublisherCycleCreated(this.Guid, publisherGuid, cycleName));
        }

        public void AddBook(CycleBook cycleBook)
        {
            this.CheckBusinessRules(new AddBookToPublisherCycleRules(this, cycleBook));

            this.ApplyChange(new BookAddedToCycle(this.Guid, cycleBook.BookGuid));
        }

        public void RemoveBook(CycleBook book)
        {
            this.CheckBusinessRules(new RemoveBookFromPublisherCycleRules(this, book));

            this.ApplyChange(new BookRemovedFromCycle(this.Guid, book.BookGuid));
        }

        public bool HasBook(Guid bookGuid)
        {
            return this.GetCycleBook(bookGuid) != null;
        }

        public CycleBook GetCycleBook(Guid bookGuid)
        {
            return this._books.Find(p => p.BookGuid == bookGuid);
        }

        public override void ApplySnapshot(IMemento memento)
        {
            var cycleMemento = memento as ICycleMemento;

            this.Guid = cycleMemento.AggregateGuid;
            this.AggregateStatus = AggregateStates.Get(cycleMemento.AggregateStatus);
            this.Version = cycleMemento.Version;
            this.LastCommittedVersion = cycleMemento.LastCommittedVersion;

            this.CycleName = cycleMemento.CycleName;
            this.PublisherGuid = cycleMemento.PublisherGuid;
            this._books = cycleMemento.CycleBooks.Select(s => new CycleBook(s)).ToList();
        }

        void IHandle<PublisherCycleCreated>.Handle(
            PublisherCycleCreated @event)
        {
            this.Guid = @event.AggregateGuid;
            this.PublisherGuid = @event.PublisherGuid;
            this.CycleName = @event.CycleName;
            this.AggregateStatus = AggregateStatus.Active;
        }

        void IHandle<BookAddedToCycle>.Handle(BookAddedToCycle @event)
        {
            this._books.Add(new CycleBook(@event.BookGuid));
        }

        void IHandle<BookRemovedFromCycle>.Handle(BookRemovedFromCycle @event)
        {
            var book = this.Books.Single(p => p.BookGuid == @event.BookGuid);

            this._books.Remove(book);
        }

        void IHandle<PublisherCycleArchived>.Handle(
            PublisherCycleArchived @event)
        {
            this.Guid = @event.AggregateGuid;
            this.AggregateStatus = AggregateStatus.Archived;
        }

        void IHandle<PublisherCycleRestored>.Handle(
            PublisherCycleRestored @event)
        {
            this.Guid = @event.AggregateGuid;
            this.AggregateStatus = AggregateStatus.Active;
            this.PublisherGuid = @event.PublisherGuid;
        }
    }
}