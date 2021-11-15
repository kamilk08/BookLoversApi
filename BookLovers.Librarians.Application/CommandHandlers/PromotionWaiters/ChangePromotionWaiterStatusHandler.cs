using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Librarians.Application.Commands.PromotionWaiters;
using BookLovers.Librarians.Domain.PromotionWaiters;

namespace BookLovers.Librarians.Application.CommandHandlers.PromotionWaiters
{
    internal class ChangePromotionWaiterStatusHandler :
        ICommandHandler<ChangePromotionWaiterStatusInternalCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPromotionWaiterRepository _repository;
        private readonly IPromotionAvailabilityProvider _provider;

        public ChangePromotionWaiterStatusHandler(
            IUnitOfWork unitOfWork,
            IPromotionWaiterRepository repository,
            IPromotionAvailabilityProvider provider)
        {
            this._unitOfWork = unitOfWork;
            this._repository = repository;
            this._provider = provider;
        }

        public async Task HandleAsync(ChangePromotionWaiterStatusInternalCommand command)
        {
            var promotionWaiter = await this._repository.GetPromotionWaiterByReaderGuid(command.ReaderGuid);
            var promotionAvailability = this._provider.GetPromotionAvailability(command.PromotionWaiterStatus);

            promotionWaiter.ChangeAvailability(promotionAvailability);

            await this._unitOfWork.CommitAsync(promotionWaiter);
        }
    }
}