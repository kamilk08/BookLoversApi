using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Publication.Application.WriteModels.Books;

namespace BookLovers.Publication.Application.Commands.Authors
{
    public class EditAuthorCommand : ICommand
    {
        public EditAuthorWriteModel WriteModel { get; }

        public EditAuthorCommand(EditAuthorWriteModel writeModel)
        {
            this.WriteModel = writeModel;
        }
    }
}