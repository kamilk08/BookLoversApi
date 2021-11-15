using System.Threading.Tasks;
using BookLovers.Auth.Application.Commands.Registrations;
using BookLovers.Auth.Domain.RegistrationSummaries;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Auth.Application.CommandHandlers.Registrations
{
    internal class CreateRegistrationSummaryHandler :
        ICommandHandler<CreateRegistrationSummaryInternalCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateRegistrationSummaryHandler(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public Task HandleAsync(CreateRegistrationSummaryInternalCommand command)
        {
            var identification = new RegistrationIdentification(command.UserGuid, command.Email);
            var registrationSummary = RegistrationSummary.Create(identification, command.Token);

            return this._unitOfWork.CommitAsync(registrationSummary);
        }
    }
}