using System;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Librarians.Application.Commands.ReviewRegisters;
using BookLovers.Librarians.Domain.ReviewReportRegisters;

namespace BookLovers.Librarians.Application.CommandHandlers.ReviewRegisters
{
    internal class CreateReviewReportRegisterHandler :
        ICommandHandler<CreateReviewReportRegisterInternalCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateReviewReportRegisterHandler(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public Task HandleAsync(CreateReviewReportRegisterInternalCommand command)
        {
            return this._unitOfWork.CommitAsync(
                new ReviewReportRegister(Guid.NewGuid(), command.ReviewGuid));
        }
    }
}