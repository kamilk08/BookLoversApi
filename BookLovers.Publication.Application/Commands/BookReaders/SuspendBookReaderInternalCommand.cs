using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Publication.Application.Commands.BookReaders
{
    internal class SuspendBookReaderInternalCommand : IInternalCommand, ICommand
    {
        public Guid Guid { get; private set; }

        public Guid ReaderGuid { get; private set; }

        private SuspendBookReaderInternalCommand()
        {
        }

        public SuspendBookReaderInternalCommand(Guid readerGuid)
        {
            this.Guid = Guid.NewGuid();
            this.ReaderGuid = readerGuid;
        }
    }
}