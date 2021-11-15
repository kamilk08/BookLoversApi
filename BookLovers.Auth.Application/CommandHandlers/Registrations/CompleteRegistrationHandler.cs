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

        public CompleteRegistrationHandler(
            IUnitOfWork unitOfWork,
            IRegistrationSummaryRepository repository)
        {
            this._unitOfWork = unitOfWork;
            this._repository = repository;
        }

        public async Task HandleAsync(CompleteRegistrationCommand command)
        {
            var registrationSummary = await this._repository.GetRegistrationSummaryByEmailAsync(command.Email);

            if (registrationSummary == null)
                throw new BusinessRuleNotMetException("Account with given email does not exist.");

            registrationSummary.Complete(command.Token);

            await this._unitOfWork.CommitAsync(registrationSummary);
        }
    }
}