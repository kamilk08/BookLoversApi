using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Publication.Application.WriteModels.Books;

namespace BookLovers.Publication.Application.Commands.Books
{
    public class EditBookCommand : ICommand
    {
        public EditBookWriteModel WriteModel { get; }

        public EditBookCommand(EditBookWriteModel writeModel)
        {
            this.WriteModel = writeModel;
        }
    }
}