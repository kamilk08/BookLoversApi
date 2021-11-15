using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Bookcases.Application.WriteModels;

namespace BookLovers.Bookcases.Application.Commands.Shelves
{
    public class ChangeShelfCommand : ICommand
    {
        public ChangeShelfWriteModel WriteModel { get; }

        public ChangeShelfCommand(ChangeShelfWriteModel writeModel)
        {
            WriteModel = writeModel;
        }
    }
}