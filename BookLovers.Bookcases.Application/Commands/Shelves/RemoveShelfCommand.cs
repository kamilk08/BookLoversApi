using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Bookcases.Application.WriteModels;

namespace BookLovers.Bookcases.Application.Commands.Shelves
{
    public class RemoveShelfCommand : ICommand
    {
        public RemoveShelfWriteModel ShelfWriteModel { get; }

        public RemoveShelfCommand(RemoveShelfWriteModel writeModel)
        {
            ShelfWriteModel = writeModel;
        }
    }
}