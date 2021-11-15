using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Publication.Application.WriteModels.Series;

namespace BookLovers.Publication.Application.Commands.Series
{
    public class CreateSeriesCommand : ICommand
    {
        public SeriesWriteModel WriteModel { get; }

        public CreateSeriesCommand(SeriesWriteModel writeModel)
        {
            this.WriteModel = writeModel;
        }
    }
}