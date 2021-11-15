using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Readers.Application.Commands.Favourites
{
    internal class RemoveFavouriteOwnerInternalCommand : IInternalCommand, ICommand
    {
        public Guid Guid { get; private set; }

        public Guid FavouriteGuid { get; private set; }

        public Guid OwnerGuid { get; private set; }

        private RemoveFavouriteOwnerInternalCommand()
        {
        }

        public RemoveFavouriteOwnerInternalCommand(Guid favouriteGuid, Guid ownerGuid)
        {
            Guid = Guid.NewGuid();
            FavouriteGuid = favouriteGuid;
            OwnerGuid = ownerGuid;
        }
    }
}