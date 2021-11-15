using System.Threading.Tasks;
using BookLovers.Auth.Application.Commands.Users;
using BookLovers.Auth.Domain.RegistrationSummaries.Services;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Auth.Application.CommandHandlers.Users
{
    internal class CheckAccountsConfirmationHandler : ICommandHandler<CheckAccountsConfirmationCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRegistrationSummaryRepository _repository;

        public CheckAccountsConfirmationHandler(
            IUnitOfWork unitOfWork,
            IRegistrationSummaryRepository repository)
        {
            this._unitOfWork = unitOfWork;
            this._repository = repository;
        }

        public async Task HandleAsync(CheckAccountsConfirmationCommand command)
        {
            var summaries = await this._repository.GetRegistrationsWithoutCompletionAsync();

            foreach (var summary in summaries)
            {
                if (summary.IsExpired())
                    summary.EndWithoutCompletion();

                await this._unitOfWork.CommitAsync(summary);
            }
        }
    }
}