using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Bookcases.Application.Commands.Bookcases
{
    internal class CreateBookcaseInternalCommand : ICommand, IInternalCommand
    {
        public Guid BookcaseGuid { get; private set; }

        public Guid ReaderGuid { get; private set; }

        public int ReaderId { get; private set; }

        public Guid Guid { get; private set; }

        private CreateBookcaseInternalCommand()
        {
        }

        public CreateBookcaseInternalCommand(Guid bookcaseGuid, Guid readerGuid, int readerId)
        {
            Guid = Guid.NewGuid();
            BookcaseGuid = bookcaseGuid;
            ReaderGuid = readerGuid;
            ReaderId = readerId;
        }
    }
}