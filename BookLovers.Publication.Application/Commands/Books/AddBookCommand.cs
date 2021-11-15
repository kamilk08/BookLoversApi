using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Publication.Application.WriteModels.Books;

namespace BookLovers.Publication.Application.Commands.Books
{
    public class AddBookCommand : ICommand
    {
        public CreateBookWriteModel WriteModel { get; }

        public AddBookCommand(CreateBookWriteModel writeModel)
        {
            this.WriteModel = writeModel;
        }
    }
}