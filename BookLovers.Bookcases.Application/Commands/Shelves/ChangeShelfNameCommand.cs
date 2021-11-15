using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Bookcases.Application.WriteModels;

namespace BookLovers.Bookcases.Application.Commands.Shelves
{
    public class ChangeShelfNameCommand : ICommand
    {
        public ChangeShelfNameWriteModel WriteModel { get; }

        public ChangeShelfNameCommand(ChangeShelfNameWriteModel writeModel)
        {
            WriteModel = writeModel;
        }
    }
}