using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Readers.Application.WriteModels.Reviews;

namespace BookLovers.Readers.Application.Commands.Reviews
{
    public class AddReviewCommand : ICommand
    {
        public ReviewWriteModel WriteModel { get; }

        public AddReviewCommand(ReviewWriteModel writeModel)
        {
            WriteModel = writeModel;
        }
    }
}