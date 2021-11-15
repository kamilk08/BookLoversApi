using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Readers.Application.Commands.Favourites
{
    internal class ArchiveFavouriteInternalCommand : IInternalCommand, ICommand
    {
        public Guid Guid { get; private set; }

        public Guid FavouriteGuid { get; private set; }

        private ArchiveFavouriteInternalCommand()
        {
        }

        public ArchiveFavouriteInternalCommand(Guid favouriteGuid)
        {
            Guid = Guid.NewGuid();
            FavouriteGuid = favouriteGuid;
        }
    }
}