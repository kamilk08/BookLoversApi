using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Readers.Application.WriteModels.Reviews;

namespace BookLovers.Readers.Application.Commands.Reviews
{
    public class EditReviewCommand : ICommand
    {
        public ReviewWriteModel WriteModel { get; }

        public EditReviewCommand(ReviewWriteModel writeModel)
        {
            WriteModel = writeModel;
        }
    }
}