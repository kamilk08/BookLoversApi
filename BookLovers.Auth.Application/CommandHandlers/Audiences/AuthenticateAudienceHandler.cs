using System;
using System.Threading.Tasks;
using BookLovers.Auth.Application.Commands.Audiences;
using BookLovers.Auth.Domain.Audiences;
using BookLovers.Auth.Domain.Users.Services;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Auth.Application.CommandHandlers.Audiences
{
    internal class AuthenticateAudienceHandler : ICommandHandler<AuthenticateAudienceCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHashingService _hashingService;

        public AuthenticateAudienceHandler(IUnitOfWork unitOfWork, IHashingService hashingService)
        {
            this._unitOfWork = unitOfWork;
            this._hashingService = hashingService;
        }

        public async Task HandleAsync(AuthenticateAudienceCommand command)
        {
            var isParsed = Guid.TryParse(command.AudienceId, out var audienceGuid);
            if (!isParsed) return;

            var audience = await this._unitOfWork.GetAsync<Audience>(audienceGuid);

            if (audience == null) return;

            var generatedHash = this._hashingService.GetHash(command.SecretKey, audience.AudienceSecurity.Salt);

            command.IsAuthenticated = audience.IsAuthenticated(generatedHash);
        }
    }
}