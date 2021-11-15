using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Librarians.Application.WriteModels;

namespace BookLovers.Librarians.Application.Commands
{
    public class CreateLibrarianCommand : ICommand
    {
        public CreateLibrarianWriteModel WriteModel { get; }

        public CreateLibrarianCommand(CreateLibrarianWriteModel writeModel)
        {
            this.WriteModel = writeModel;
        }
    }
}