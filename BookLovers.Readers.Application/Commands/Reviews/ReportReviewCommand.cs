using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Readers.Application.WriteModels.Reviews;

namespace BookLovers.Readers.Application.Commands.Reviews
{
    public class ReportReviewCommand : ICommand
    {
        public ReportReviewWriteModel WriteModel { get; }

        public ReportReviewCommand(ReportReviewWriteModel writeModel)
        {
            WriteModel = writeModel;
        }
    }
}