using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Publication.Application.WriteModels.Author;

namespace BookLovers.Publication.Application.Commands.Authors
{
    public class CreateAuthorCommand : ICommand
    {
        public CreateAuthorWriteModel WriteModel { get; }

        public CreateAuthorCommand(CreateAuthorWriteModel writeModel)
        {
            this.WriteModel = writeModel;
        }
    }
}