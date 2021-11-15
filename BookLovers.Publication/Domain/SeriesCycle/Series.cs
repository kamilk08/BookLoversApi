using System;
using System.Collections.Generic;
using System.Linq;
using BookLovers.Base.Domain;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Base.Domain.Events;
using BookLovers.Publication.Domain.SeriesCycle.BusinessRules;
using BookLovers.Publication.Events.SeriesCycle;
using BookLovers.Publication.Mementos;

namespace BookLovers.Publication.Domain.SeriesCycle
{
    [AllowSnapshot]
    public class Series :
        EventSourcedAggregateRoot,
        IHandle<SeriesCreated>,
        IHandle<AddedToSeries>,
        IHandle<BookPositionInSeriesChanged>,
        IHandle<BookRemovedFromSeries>,
        IHandle<SeriesArchived>
    {
        internal const byte LowestSeriesPosition = 1;
        private ISet<SeriesBook> _books = new SortedSet<SeriesBook>();

        public string SeriesName { get; private set; }

        public IReadOnlyList<SeriesBook> Books => this._books.ToList();

        private Series()
        {
        }

        public Series(Guid seriesGuid, string seriesName)
        {
            this.Guid = seriesGuid;
            this.SeriesName = seriesName;
            this.AggregateStatus = AggregateStatus.Active;

            this.ApplyChange(new SeriesCreated(this.Guid, this.SeriesName));
        }

        public void AddToSeries(SeriesBook seriesBook)
        {
            this.CheckBusinessRules(new AddBookToSeriesRules(this, seriesBook));

            this.ApplyChange(new AddedToSeries(this.Guid, seriesBook.BookGuid, seriesBook.Position));
        }

        public void RemoveBook(SeriesBook seriesBook)
        {
            this.CheckBusinessRules(new RemoveBookFromSeriesRules(this, seriesBook));

            this.ApplyChange(new BookRemovedFromSeries(this.Guid, seriesBook.Position, seriesBook.BookGuid));
        }

        public void ChangePosition(SeriesBook seriesBook)
        {
            this.CheckBusinessRules(new ChangeBookPositionInSeriesRules(this, seriesBook));

            this.ApplyChange(new BookPositionInSeriesChanged(this.Guid, seriesBook.BookGuid, seriesBook.Position));
        }

        public int GetPositionInSeries(Guid bookGuid)
        {
            var book = this.GetBook(bookGuid);
            return !(book != null) ? 0 : book.Position;
        }

        public SeriesBook GetBook(Guid bookGuid) =>
            this._books.SingleOrDefault(p => p.BookGuid == bookGuid);

        public SeriesBook GetBook(int position) =>
            this._books.SingleOrDefault(p => p.Position == position);

        public override void ApplySnapshot(IMemento memento)
        {
            var seriesMemento = memento as ISeriesMemento;

            this.Guid = seriesMemento.AggregateGuid;
            this.AggregateStatus = AggregateStates.Get(seriesMemento.AggregateStatus);
            this.Version = seriesMemento.Version;
            this.LastCommittedVersion = seriesMemento.LastCommittedVersion;

            this.SeriesName = seriesMemento.SeriesName;

            foreach (var seriesBook in seriesMemento.SeriesBooks)
                this._books.Add(new SeriesBook(seriesBook.Value, seriesBook.Key));
        }

        void IHandle<AddedToSeries>.Handle(AddedToSeries @event) =>
            this._books.Add(new SeriesBook(@event.BookGuid, @event.Position));

        void IHandle<BookRemovedFromSeries>.Handle(
            BookRemovedFromSeries @event)
        {
            var cycleBook = this._books.Single(p => p.BookGuid == @event.BookGuid);

            this._books.Remove(cycleBook);
        }

        void IHandle<SeriesCreated>.Handle(SeriesCreated @event)
        {
            this.Guid = @event.AggregateGuid;
            this.SeriesName = @event.SeriesName;
            this.AggregateStatus = AggregateStatus.Active;
        }

        void IHandle<SeriesArchived>.Handle(SeriesArchived @event) =>
            this.AggregateStatus = AggregateStatus.Archived;

        void IHandle<BookPositionInSeriesChanged>.Handle(
            BookPositionInSeriesChanged @event)
        {
            var bookToRemove = this._books.Single(p => p.BookGuid == @event.BookGuid);

            this._books.Remove(bookToRemove);
            this._books.Add(new SeriesBook(@event.BookGuid, @event.Position));
        }
    }
}