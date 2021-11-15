using System;
using System.Collections.Generic;
using BookLovers.Base.Domain;
using BookLovers.Bookcases.Domain;

namespace BookLovers.Bookcases.Mementos
{
    public interface IBookcaseMemento : IMemento<Bookcase>, IMemento
    {
        Guid ReaderGuid { get; }

        Guid SettingsManagerGuid { get; }

        Guid ShelfRecordTrackerGuid { get; }

        IEnumerable<Shelf> Shelves { get; }
    }
}