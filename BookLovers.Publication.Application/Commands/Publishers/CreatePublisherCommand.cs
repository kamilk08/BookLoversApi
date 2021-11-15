using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Publication.Application.WriteModels.Publisher;

namespace BookLovers.Publication.Application.Commands.Publishers
{
    public class CreatePublisherCommand : ICommand
    {
        public AddPublisherWriteModel WriteModel { get; }

        public CreatePublisherCommand(AddPublisherWriteModel writeModel)
        {
            this.WriteModel = writeModel;
        }
    }
}