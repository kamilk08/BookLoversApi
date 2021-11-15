using System;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Librarians.Application.Commands.PromotionWaiters;
using BookLovers.Librarians.Domain.PromotionWaiters;

namespace BookLovers.Librarians.Application.CommandHandlers.PromotionWaiters
{
    internal class CreatePromotionWaiterHandler : ICommandHandler<CreatePromotionWaiterInternalCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreatePromotionWaiterHandler(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public Task HandleAsync(CreatePromotionWaiterInternalCommand command)
        {
            var promotionWaiter = new PromotionWaiter(Guid.NewGuid(), command.ReaderGuid,
                command.ReaderId);

            return this._unitOfWork.CommitAsync(promotionWaiter);
        }
    }
}