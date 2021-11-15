using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Readers.Application.WriteModels.Profiles;

namespace BookLovers.Readers.Application.Commands.Profile
{
    public class RemoveFavouriteCommand : ICommand
    {
        public RemoveFavouriteWriteModel WriteModel { get; }

        public RemoveFavouriteCommand(RemoveFavouriteWriteModel writeModel)
        {
            WriteModel = writeModel;
        }
    }
}