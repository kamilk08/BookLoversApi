using System;
using System.Collections.Generic;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Ratings.Application.Commands.Books
{
    internal class CreateBookInternalCommand : IInternalCommand, ICommand
    {
        public Guid Guid { get; private set; }

        public Guid BookGuid { get; private set; }

        public int BookId { get; private set; }

        public IEnumerable<Guid> AuthorsGuides { get; private set; }

        public Guid PublisherGuid { get; private set; }

        public Guid SeriesGuid { get; private set; }

        public IEnumerable<Guid> CyclesGuides { get; private set; }

        private CreateBookInternalCommand()
        {
        }

        public CreateBookInternalCommand(
            Guid bookGuid,
            int bookId,
            IEnumerable<Guid> authorsGuides,
            Guid publisherGuid,
            Guid seriesGuid,
            IEnumerable<Guid> cyclesGuides)
        {
            this.Guid = Guid.NewGuid();
            this.BookGuid = bookGuid;
            this.BookId = bookId;
            this.AuthorsGuides = authorsGuides;
            this.PublisherGuid = publisherGuid;
            this.SeriesGuid = seriesGuid;
            this.CyclesGuides = cyclesGuides;
        }
    }
}