using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Readers.Application.Commands.Favourites
{
    internal class CreateFavouriteInternalCommand : IInternalCommand, ICommand
    {
        public Guid Guid { get; private set; }

        public Guid FavouriteGuid { get; private set; }

        public Guid AddedByGuid { get; private set; }

        public int FavouriteId { get; private set; }

        private CreateFavouriteInternalCommand()
        {
        }

        public CreateFavouriteInternalCommand(Guid favouriteGuid, int favouriteId, Guid addedByGuid)
        {
            Guid = Guid.NewGuid();
            FavouriteGuid = favouriteGuid;
            FavouriteId = favouriteId;
            AddedByGuid = addedByGuid;
        }
    }
}