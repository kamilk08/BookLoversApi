using System.Threading.Tasks;
using BookLovers.Auth.Application.Commands.Audiences;
using BookLovers.Auth.Application.Contracts.Tokens;
using BookLovers.Auth.Domain.Audiences;
using BookLovers.Auth.Domain.Users.Services;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Auth.Application.CommandHandlers.Audiences
{
    internal class AddAudienceHandler : ICommandHandler<AddAudienceCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHashingService _hashingService;
        private readonly IRandomSecretKeyGenerator _secretKeyGenerator;

        public AddAudienceHandler(
            IUnitOfWork unitOfWork,
            IHashingService hashingService,
            IRandomSecretKeyGenerator secretKeyGenerator)
        {
            this._unitOfWork = unitOfWork;
            this._hashingService = hashingService;
            this._secretKeyGenerator = secretKeyGenerator;
        }

        public async Task HandleAsync(AddAudienceCommand command)
        {
            var secretKey = this._secretKeyGenerator.CreateSecretKey();
            var hashWithSalt = this._hashingService.CreateHashWithSalt(secretKey);

            var security = new AudienceSecurity(hashWithSalt.Item1, hashWithSalt.Item2);
            var audienceDetails = new AudienceDetails(AudienceType.ExternalAudience, command.WriteModel.TokenLifeTime);

            var audience = Audience.Create(command.WriteModel.AudienceGuid, security, audienceDetails);

            await this._unitOfWork.CommitAsync(audience);

            command.WriteModel.AudienceSecretKey = secretKey;
        }
    }
}