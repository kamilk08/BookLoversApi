using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Readers.Application.WriteModels.Timelines;

namespace BookLovers.Readers.Application.Commands.Timelines
{
    public class ShowActivityCommand : ICommand
    {
        public ShowTimeLineActivityWriteModel WriteModel { get; }

        public ShowActivityCommand(ShowTimeLineActivityWriteModel writeModel)
        {
            WriteModel = writeModel;
        }
    }
}