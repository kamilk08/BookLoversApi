using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Bookcases.Application.WriteModels;

namespace BookLovers.Bookcases.Application.Commands.Bookcases
{
    public class AddToBookcaseCommand : ICommand
    {
        public AddBookToBookcaseWriteModel WriteModel { get; }

        public AddToBookcaseCommand(AddBookToBookcaseWriteModel writeModel)
        {
            WriteModel = writeModel;
        }
    }
}