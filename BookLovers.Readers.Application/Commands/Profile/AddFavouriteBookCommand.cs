using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Readers.Application.WriteModels.Profiles;

namespace BookLovers.Readers.Application.Commands.Profile
{
    public class AddFavouriteBookCommand : ICommand
    {
        public AddFavouriteBookWriteModel WriteModel { get; }

        public AddFavouriteBookCommand(AddFavouriteBookWriteModel writeModel)
        {
            WriteModel = writeModel;
        }
    }
}