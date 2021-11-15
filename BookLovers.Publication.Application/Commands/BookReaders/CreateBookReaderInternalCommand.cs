using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Publication.Application.Commands.BookReaders
{
    internal class CreateBookReaderInternalCommand : IInternalCommand, ICommand
    {
        public Guid Guid { get; private set; }

        public Guid ReaderGuid { get; private set; }

        public int ReaderId { get; private set; }

        private CreateBookReaderInternalCommand()
        {
        }

        public CreateBookReaderInternalCommand(Guid readerGuid, int readerId)
        {
            this.Guid = Guid.NewGuid();
            this.ReaderGuid = readerGuid;
            this.ReaderId = readerId;
        }
    }
}