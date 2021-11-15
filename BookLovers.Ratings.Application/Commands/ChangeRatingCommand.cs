using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Ratings.Application.WriteModels;

namespace BookLovers.Ratings.Application.Commands
{
    public class ChangeRatingCommand : ICommand
    {
        public ChangeRatingWriteModel WriteModel { get; }

        public ChangeRatingCommand(ChangeRatingWriteModel writeModel)
        {
            this.WriteModel = writeModel;
        }
    }
}