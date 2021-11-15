using System.Threading.Tasks;
using BookLovers.Auth.Domain.Users.Services;
using BookLovers.Base.Infrastructure;

namespace BookLovers.Auth.Domain.Tokens.Services
{
    public class RefreshTokenFactory
    {
        private readonly IHashingService _hashingService;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RefreshTokenFactory(
            IHashingService hashingService,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork)
        {
            _hashingService = hashingService;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Create(string protectedToken, RefreshTokenProperties tokenProperties)
        {
            var user = await _userRepository.GetUserByEmailAsync(tokenProperties.Email);
            var hashWithSalt = _hashingService.CreateHashWithSalt(protectedToken);

            var tokenIdentification = new RefreshTokenIdentification(user.Guid, tokenProperties.AudienceGuid);
            var tokenSecurity = new RefreshTokenSecurity(hashWithSalt.Item1, hashWithSalt.Item2);

            var refreshTokenDetails = new RefreshTokenDetails(
                tokenProperties.IssuedAt.Value.UtcDateTime,
                tokenProperties.ExpiresAt.Value.UtcDateTime,
                tokenProperties.SerializedTicket);

            var refreshToken = RefreshToken.Create(
                tokenProperties.TokenGuid,
                tokenIdentification,
                tokenSecurity,
                refreshTokenDetails);

            await _unitOfWork.CommitAsync(refreshToken);
        }
    }
}