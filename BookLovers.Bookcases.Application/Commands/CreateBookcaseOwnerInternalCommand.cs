using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Bookcases.Application.Commands
{
    internal class CreateBookcaseOwnerInternalCommand : ICommand, IInternalCommand
    {
        public Guid Guid { get; private set; }

        public Guid BookcaseGuid { get; private set; }

        public Guid ReaderGuid { get; private set; }

        public int ReaderId { get; private set; }

        private CreateBookcaseOwnerInternalCommand()
        {
        }

        public CreateBookcaseOwnerInternalCommand(Guid bookcaseGuid, Guid readerGuid, int readerId)
        {
            Guid = Guid.NewGuid();
            BookcaseGuid = bookcaseGuid;
            ReaderGuid = readerGuid;
            ReaderId = readerId;
        }
    }
}