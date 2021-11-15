using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Readers.Application.WriteModels.Profiles;

namespace BookLovers.Readers.Application.Commands.Profile
{
    public class AddFavouriteAuthorCommand : ICommand
    {
        public AddFavouriteAuthorWriteModel WriteModel { get; }

        public AddFavouriteAuthorCommand(AddFavouriteAuthorWriteModel writeModel)
        {
            WriteModel = writeModel;
        }
    }
}