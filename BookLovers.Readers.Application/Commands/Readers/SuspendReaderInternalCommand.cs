using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Readers.Application.Commands.Readers
{
    internal class SuspendReaderInternalCommand : ICommand, IInternalCommand
    {
        public Guid ReaderGuid { get; private set; }

        public Guid Guid { get; private set; }

        public SuspendReaderInternalCommand(Guid readerGuid)
        {
            Guid = Guid.NewGuid();
            ReaderGuid = readerGuid;
        }
    }
}