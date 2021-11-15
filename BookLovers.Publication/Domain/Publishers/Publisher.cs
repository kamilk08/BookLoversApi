using System;
using System.Collections.Generic;
using System.Linq;
using BookLovers.Base.Domain;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Base.Domain.Events;
using BookLovers.Publication.Domain.Publishers.BusinessRules;
using BookLovers.Publication.Events.Publishers;
using BookLovers.Publication.Mementos;

namespace BookLovers.Publication.Domain.Publishers
{
    [AllowSnapshot]
    public class Publisher :
        EventSourcedAggregateRoot,
        IHandle<PublisherBookAdded>,
        IHandle<PublisherCreated>,
        IHandle<PublisherBookRemoved>,
        IHandle<PublisherArchived>,
        IHandle<PublisherCycleAdded>,
        IHandle<PublisherCycleRemoved>
    {
        private List<PublisherBook> _books = new List<PublisherBook>();
        private List<Cycle> _cycles = new List<Cycle>();

        public string PublisherName { get; private set; }

        public IReadOnlyList<PublisherBook> Books => this._books;

        public IReadOnlyList<Cycle> Cycles => this._cycles;

        private Publisher()
        {
        }

        public Publisher(Guid publisherGuid, string publisherName)
        {
            this.Guid = publisherGuid;
            this.PublisherName = publisherName;
            this.AggregateStatus = AggregateStatus.Active;

            this.ApplyChange(new PublisherCreated(publisherGuid, publisherName));
        }

        public void AddBook(PublisherBook book)
        {
            this.CheckBusinessRules(new AddPublisherBookRules(this, book));

            this.ApplyChange(new PublisherBookAdded(this.Guid, book.BookGuid));
        }

        public void RemoveBook(PublisherBook book)
        {
            this.CheckBusinessRules(new RemovePublisherBookRules(this, book));

            this.ApplyChange(new PublisherBookRemoved(this.Guid, book.BookGuid));
        }

        public void RemoveCycle(Cycle cycle)
        {
            this.CheckBusinessRules(new RemovePublisherCycle(this, cycle));

            this.ApplyChange(new PublisherCycleRemoved(this.Guid, cycle.CycleGuid));
        }

        public void AddCycle(Cycle cycle)
        {
            this.CheckBusinessRules(new AddPublisherCycleRules(this, cycle));

            this.ApplyChange(new PublisherCycleAdded(this.Guid, cycle.CycleGuid));
        }

        public Cycle GetCycle(Guid cycleGuid) =>
            this._cycles.Find(p => p.CycleGuid == cycleGuid);

        public PublisherBook GetBook(Guid bookGuid) =>
            this._books.Find(p => p.BookGuid == bookGuid);

        public override void ApplySnapshot(IMemento memento)
        {
            var publisherMemento = memento as IPublisherMemento;

            this.Guid = publisherMemento.AggregateGuid;
            this.AggregateStatus = AggregateStates.Get(publisherMemento.AggregateStatus);
            this.Version = publisherMemento.Version;
            this.LastCommittedVersion = publisherMemento.LastCommittedVersion;

            this.PublisherName = publisherMemento.PublisherName;
            this._books = publisherMemento.Books.Select(s => new PublisherBook(s)).ToList();
            this._cycles = publisherMemento.Cycles.Select(s => new Cycle(s)).ToList();
        }

        void IHandle<PublisherBookAdded>.Handle(PublisherBookAdded @event)
        {
            this._books.Add(new PublisherBook(@event.BookGuid));
        }

        void IHandle<PublisherBookRemoved>.Handle(PublisherBookRemoved @event)
        {
            var book = this._books.Single(p => p.BookGuid == @event.BookGuid);

            this._books.Remove(book);
        }

        void IHandle<PublisherCreated>.Handle(PublisherCreated @event)
        {
            this.Guid = @event.AggregateGuid;
            this.PublisherName = @event.Name;
            this.AggregateStatus = AggregateStatus.Active;
        }

        void IHandle<PublisherArchived>.Handle(PublisherArchived @event)
        {
            this.Guid = @event.AggregateGuid;
            this.AggregateStatus = AggregateStatus.Archived;
        }

        void IHandle<PublisherCycleAdded>.Handle(PublisherCycleAdded @event)
        {
            this._cycles.Add(new Cycle(@event.CycleGuid));
        }

        void IHandle<PublisherCycleRemoved>.Handle(
            PublisherCycleRemoved @event)
        {
            var cycle = this._cycles.Single(p => p.CycleGuid == @event.CycleGuid);

            this._cycles.Remove(cycle);
        }
    }
}