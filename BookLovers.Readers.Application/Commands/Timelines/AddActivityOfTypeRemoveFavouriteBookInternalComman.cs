using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Readers.Application.Commands.Timelines
{
    internal class AddActivityOfTypeRemoveFavouriteBookInternalCommand : IInternalCommand, ICommand
    {
        public Guid Guid { get; private set; }

        public Guid ReaderGuid { get; private set; }

        public Guid BookGuid { get; private set; }

        private AddActivityOfTypeRemoveFavouriteBookInternalCommand()
        {
        }

        public AddActivityOfTypeRemoveFavouriteBookInternalCommand(Guid readerGuid, Guid bookGuid)
        {
            Guid = Guid.NewGuid();
            ReaderGuid = readerGuid;
            BookGuid = bookGuid;
        }
    }
}