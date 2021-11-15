using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Readers.Application.Commands.Readers
{
    internal class AddBookResourceInternalCommand : ICommand, IInternalCommand
    {
        public Guid Guid { get; private set; }

        public Guid ReaderGuid { get; private set; }

        public Guid BookGuid { get; private set; }

        public int BookId { get; private set; }

        public DateTime Date { get; private set; }

        private AddBookResourceInternalCommand()
        {
        }

        public AddBookResourceInternalCommand(
            Guid readerGuid,
            Guid bookGuid,
            int bookId,
            DateTime date)
        {
            Guid = Guid.NewGuid();
            ReaderGuid = readerGuid;
            BookGuid = bookGuid;
            BookId = bookId;
            Date = date;
        }
    }
}