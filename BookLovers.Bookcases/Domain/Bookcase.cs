using System;
using System.Collections.Generic;
using System.Linq;
using BookLovers.Base.Domain;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Base.Domain.Events;
using BookLovers.Bookcases.Domain.BookcaseBooks;
using BookLovers.Bookcases.Domain.BusinessRules;
using BookLovers.Bookcases.Domain.ShelfCategories;
using BookLovers.Bookcases.Events.Bookcases;
using BookLovers.Bookcases.Events.Shelf;
using BookLovers.Bookcases.Mementos;

namespace BookLovers.Bookcases.Domain
{
    [AllowSnapshot]
    public class Bookcase :
        EventSourcedAggregateRoot,
        IHandle<BookcaseCreated>,
        IHandle<CoreShelfCreated>,
        IHandle<CustomShelfCreated>,
        IHandle<ShelfRemoved>,
        IHandle<BookAddedToShelf>,
        IHandle<BookRemovedFromShelf>,
        IHandle<BookShelfChanged>,
        IHandle<BookcaseArchived>,
        IHandle<ShelfNameChanged>
    {
        private List<Shelf> _shelves = new List<Shelf>();

        public IReadOnlyList<Shelf> Shelves => _shelves;

        public BookcaseAdditions Additions { get; private set; }

        private Bookcase()
        {
        }

        internal Bookcase(Guid bookcaseGuid, BookcaseAdditions additions)
        {
            Guid = bookcaseGuid;
            AggregateStatus = AggregateStatus.Active;
            Additions = additions;
        }

        public void AddCustomShelf(Shelf shelf)
        {
            CheckBusinessRules(new AddCustomShelfRules(this, shelf));

            var @event = new CustomShelfCreated(
                Guid,
                shelf.Guid,
                shelf.ShelfDetails.ShelfName,
                ShelfCategory.Custom.Value);

            ApplyChange(@event);
        }

        public void AddCustomShelf(Guid shelfGuid, string shelfName)
        {
            var customShelf = Shelf.CreateCustomShelf(shelfGuid, shelfName);

            CheckBusinessRules(new AddCustomShelfRules(this, customShelf));

            var @event = new CustomShelfCreated(
                Guid,
                customShelf.Guid,
                customShelf.ShelfDetails.ShelfName,
                ShelfCategory.Custom.Value);

            ApplyChange(@event);
        }

        public void RemoveShelf(Shelf shelf)
        {
            CheckBusinessRules(new RemoveCustomShelfRules(this, shelf));

            ApplyChange(new ShelfRemoved(Guid, shelf.Guid));
        }

        internal void AddToShelf(Guid bookGuid, Shelf shelf)
        {
            CheckBusinessRules(new AddToShelfRules(this, shelf, bookGuid));

            var @event = BookAddedToShelf.Initialize()
                .WithBookcase(Guid)
                .WithBookAndShelf(bookGuid, shelf.Guid)
                .WithReader(Additions.ReaderGuid)
                .WithTracker(Additions.ShelfRecordTrackerGuid)
                .WithAddedAt(DateTime.UtcNow);

            ApplyChange(@event);
        }

        internal void ChangeShelf(Guid bookGuid, Shelf oldShelf, Shelf newShelf)
        {
            CheckBusinessRules(new ChangeShelfRules(this, oldShelf, newShelf, bookGuid));

            var @event = BookShelfChanged.Initialize()
                .WithBookcaseAndBook(Guid, bookGuid)
                .WithOldShelf(oldShelf.Guid)
                .WithNewShelf(newShelf.Guid)
                .WithTracker(Additions.ShelfRecordTrackerGuid)
                .WithChangedAt(DateTime.UtcNow);

            ApplyChange(@event);
        }

        public void RemoveFromShelf(BookcaseBook book, Shelf shelf)
        {
            CheckBusinessRules(new RemoveFromShelfRules(this, shelf, book));

            var @event = BookRemovedFromShelf.Initialize()
                .WithBookcase(Guid)
                .WithBook(book.BookGuid)
                .WithShelf(shelf.Guid)
                .WithTracker(Additions.ShelfRecordTrackerGuid)
                .WithRemovedAt(DateTime.UtcNow);

            ApplyChange(@event);
        }

        public void ChangeShelfName(Guid shelfGuid, string shelfName)
        {
            CheckBusinessRules(new ChangeShelfNameRules(this, GetShelf(shelfGuid)));

            ApplyChange(new ShelfNameChanged(Guid, shelfGuid, shelfName));
        }

        public bool ContainsBook(Guid bookGuid)
        {
            return _shelves.Any(p => p.Books.Contains(bookGuid));
        }

        public List<Shelf> GetShelvesByCategory(ShelfCategory category)
            => _shelves.FindAll(p => p.ShelfDetails.Category == category);

        public Shelf GetShelf(string shelfName) => _shelves.Find(p => p.ShelfDetails.ShelfName == shelfName);

        public Shelf GetShelf(Guid shelfGuid) => _shelves.Find(p => p.Guid == shelfGuid);

        public bool HasShelf(Guid shelfGuid) => GetShelf(shelfGuid) != null;

        public bool HasShelf(string shelfName) => GetShelf(shelfName) != null;

        public IEnumerable<Shelf> GetShelvesWithBook(Guid bookGuid)
            => _shelves.Where(p => p.Books.Contains(bookGuid));

        void IHandle<BookAddedToShelf>.Handle(BookAddedToShelf @event) =>
            GetShelf(@event.ShelfGuid).Books.Add(@event.BookGuid);

        void IHandle<BookcaseCreated>.Handle(BookcaseCreated @event)
        {
            Guid = @event.AggregateGuid;
            Additions = new BookcaseAdditions(@event.ReaderGuid, @event.SettingsManagerGuid, @event.ShelfTrackerGuid);
            AggregateStatus = AggregateStatus.Active;
        }

        void IHandle<BookRemovedFromShelf>.Handle(BookRemovedFromShelf @event) =>
            GetShelf(@event.ShelfGuid).Books.Remove(@event.BookGuid);

        void IHandle<BookShelfChanged>.Handle(BookShelfChanged @event)
        {
            GetShelf(@event.OldShelfGuid).Books.Remove(@event.BookGuid);
            GetShelf(@event.NewShelfGuid).Books.Add(@event.BookGuid);
        }

        void IHandle<CustomShelfCreated>.Handle(CustomShelfCreated @event)
            => _shelves.Add(Shelf.CreateCustomShelf(@event.ShelfGuid, @event.ShelfName));

        void IHandle<ShelfRemoved>.Handle(ShelfRemoved @event)
            => _shelves.Remove(GetShelf(@event.ShelfGuid));

        void IHandle<CoreShelfCreated>.Handle(CoreShelfCreated @event)
        {
            var shelf = Shelf.CreateCoreShelf(
                @event.ShelfGuid,
                @event.ShelfName,
                ShelfCategoryList.Get(@event.ShelfCategory));

            _shelves.Add(shelf);
        }

        void IHandle<BookcaseArchived>.Handle(BookcaseArchived @event) => AggregateStatus = AggregateStatus.Archived;

        void IHandle<ShelfNameChanged>.Handle(ShelfNameChanged @event) =>
            GetShelf(@event.ShelfGuid).ChangeShelfName(@event.ShelfName);

        public override void ApplySnapshot(IMemento memento)
        {
            var bookcaseMemento = memento as IBookcaseMemento;

            Guid = bookcaseMemento.AggregateGuid;
            AggregateStatus = AggregateStates.Get(bookcaseMemento.AggregateStatus);
            LastCommittedVersion = bookcaseMemento.LastCommittedVersion;
            Version = bookcaseMemento.Version;

            Additions = new BookcaseAdditions(
                bookcaseMemento.ReaderGuid,
                bookcaseMemento.SettingsManagerGuid,
                bookcaseMemento.ShelfRecordTrackerGuid);

            _shelves = bookcaseMemento.Shelves.ToList();
        }
    }
}