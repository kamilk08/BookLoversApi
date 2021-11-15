using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Bookcases.Application.WriteModels;

namespace BookLovers.Bookcases.Application.Commands.Shelves
{
    public class AddShelfCommand : ICommand
    {
        public AddShelfWriteModel WriteModel { get; }

        public AddShelfCommand(AddShelfWriteModel writeModel)
        {
            WriteModel = writeModel;
        }
    }
}