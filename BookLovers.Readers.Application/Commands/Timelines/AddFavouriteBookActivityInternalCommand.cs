using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Readers.Application.Commands.Timelines
{
    internal class AddFavouriteBookActivityInternalCommand : IInternalCommand, ICommand
    {
        public Guid Guid { get; private set; }

        public Guid ReaderGuid { get; private set; }

        public Guid BookGuid { get; private set; }

        private AddFavouriteBookActivityInternalCommand()
        {
        }

        public AddFavouriteBookActivityInternalCommand(Guid readerGuid, Guid bookGuid)
        {
            Guid = Guid.NewGuid();
            ReaderGuid = readerGuid;
            BookGuid = bookGuid;
        }
    }
}