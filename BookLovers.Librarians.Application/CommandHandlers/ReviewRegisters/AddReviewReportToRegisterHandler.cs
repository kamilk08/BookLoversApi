using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Librarians.Application.Commands.ReviewRegisters;
using BookLovers.Librarians.Domain.ReviewReportRegisters;

namespace BookLovers.Librarians.Application.CommandHandlers.ReviewRegisters
{
    internal class AddReviewReportToRegisterHandler :
        ICommandHandler<AddReviewReportToRegisterInternalCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IReviewReportRegisterRepository _repository;
        private readonly IReportReasonProvider _reportReasonProvider;

        public AddReviewReportToRegisterHandler(
            IUnitOfWork unitOfWork,
            IReviewReportRegisterRepository repository,
            IReportReasonProvider reportReasonProvider)
        {
            this._unitOfWork = unitOfWork;
            this._repository = repository;
            this._reportReasonProvider = reportReasonProvider;
        }

        public async Task HandleAsync(AddReviewReportToRegisterInternalCommand command)
        {
            var reviewReportRegister = await this._repository.GetByReviewGuidAsync(command.ReviewGuid);
            var reportReason = this._reportReasonProvider.GetReportReason(command.ReportReasonId);

            reviewReportRegister.AddReportToRegister(new ReportRegisterItem(command.ReaderGuid, reportReason));

            await this._unitOfWork.CommitAsync(reviewReportRegister);
        }
    }
}