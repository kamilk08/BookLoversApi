using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Readers.Application.WriteModels.Timelines;

namespace BookLovers.Readers.Application.Commands.Timelines
{
    public class HideActivityCommand : ICommand
    {
        public HideTimeLineActivityWriteModel WriteModel { get; }

        public HideActivityCommand(HideTimeLineActivityWriteModel writeModel)
        {
            WriteModel = writeModel;
        }
    }
}