using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Readers.Application.Commands.Profile
{
    internal class RemoveFavouriteInternalCommand : IInternalCommand, ICommand
    {
        public Guid Guid { get; private set; }

        public Guid FavouriteGuid { get; private set; }

        public Guid ProfileGuid { get; private set; }

        private RemoveFavouriteInternalCommand()
        {
        }

        public RemoveFavouriteInternalCommand(Guid favouriteGuid, Guid profileGuid)
        {
            Guid = Guid.NewGuid();
            FavouriteGuid = favouriteGuid;
            ProfileGuid = profileGuid;
        }
    }
}