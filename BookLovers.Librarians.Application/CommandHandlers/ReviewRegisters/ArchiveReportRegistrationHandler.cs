using System.Threading.Tasks;
using BookLovers.Base.Domain.Services;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Librarians.Application.Commands.ReviewRegisters;
using BookLovers.Librarians.Domain.ReviewReportRegisters;

namespace BookLovers.Librarians.Application.CommandHandlers.ReviewRegisters
{
    internal class ArchiveReportRegistrationHandler :
        ICommandHandler<ArchiveReportRegistrationInternalCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAggregateManager<ReviewReportRegister> _manager;
        private readonly IReviewReportRegisterRepository _repository;

        public ArchiveReportRegistrationHandler(
            IUnitOfWork unitOfWork,
            IAggregateManager<ReviewReportRegister> manager,
            IReviewReportRegisterRepository repository)
        {
            this._unitOfWork = unitOfWork;
            this._manager = manager;
            this._repository = repository;
        }

        public async Task HandleAsync(ArchiveReportRegistrationInternalCommand command)
        {
            var reviewReportRegister = await this._repository.GetByReviewGuidAsync(command.ReviewGuid);

            this._manager.Archive(reviewReportRegister);

            await this._unitOfWork.CommitAsync(reviewReportRegister);
        }
    }
}