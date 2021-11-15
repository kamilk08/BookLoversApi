using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Ratings.Application.WriteModels;

namespace BookLovers.Ratings.Application.Commands
{
    public class RemoveRatingCommand : ICommand
    {
        public RemoveRatingWriteModel WriteModel { get; }

        public RemoveRatingCommand(RemoveRatingWriteModel writeModel)
        {
            this.WriteModel = writeModel;
        }
    }
}