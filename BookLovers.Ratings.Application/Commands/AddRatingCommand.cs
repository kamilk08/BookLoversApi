using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Ratings.Application.WriteModels;

namespace BookLovers.Ratings.Application.Commands
{
    public class AddRatingCommand : ICommand
    {
        public AddRatingWriteModel WriteModel { get; }

        public AddRatingCommand(AddRatingWriteModel writeModel)
        {
            this.WriteModel = writeModel;
        }
    }
}