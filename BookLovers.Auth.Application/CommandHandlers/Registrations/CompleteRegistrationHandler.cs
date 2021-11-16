using System.Threading.Tasks;
using BookLovers.Auth.Application.Commands.Registrations;
using BookLovers.Auth.Domain.RegistrationSummaries.Services;
using BookLovers.Base.Domain.Rules;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Auth.Application.CommandHandlers.Registrations
{
    internal class CompleteRegistrationHandler : ICommandHandler<CompleteRegistrationCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRegistrationSummaryRepository _repository;
        private readonly SummaryCompletionService _completionService;

        public CompleteRegistrationHandler(
            IUnitOfWork unitOfWork,
            IRegistrationSummaryRepository repository,
            SummaryCompletionService completionService
            )
        {
            this._unitOfWork = unitOfWork;
            this._repository = repository;
            this._completionService = completionService;
        }

        public async Task HandleAsync(CompleteRegistrationCommand command)
        {
            var summary = await this._repository.GetRegistrationSummaryByEmailAsync(command.Email);

            _completionService.CompleteRegistration(summary, command.Token);

            await this._unitOfWork.CommitAsync(summary);
        }
    }
}