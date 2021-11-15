using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Publication.Application.WriteModels.PublisherCycles;

namespace BookLovers.Publication.Application.Commands.PublisherCycles
{
    public class AddPublisherCycleCommand : ICommand
    {
        public AddCycleWriteModel WriteModel { get; }

        public AddPublisherCycleCommand(AddCycleWriteModel writeModel)
        {
            this.WriteModel = writeModel;
        }
    }
}