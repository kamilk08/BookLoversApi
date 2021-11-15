using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Threading.Tasks;
using AutoMapper;
using BookLovers.Auth.Domain.PasswordResets;
using BookLovers.Auth.Domain.Users.Services;
using BookLovers.Auth.Infrastructure.Persistence.ReadModels;
using BookLovers.Base.Infrastructure;

namespace BookLovers.Auth.Infrastructure.Persistence.Repositories
{
    public class PasswordResetTokenRepository :
        IPasswordResetTokenRepository,
        IRepository<PasswordResetToken>
    {
        private readonly AuthContext _authContext;
        private readonly IMapper _mapper;

        public PasswordResetTokenRepository(AuthContext authContext, IMapper mapper)
        {
            _authContext = authContext;
            _mapper = mapper;
        }

        public async Task<PasswordResetToken> GetAsync(Guid aggregateGuid)
        {
            var readModel =
                await _authContext.PasswordResetTokens.SingleOrDefaultAsync(p => p.Guid == aggregateGuid);

            var passwordResetToken = _mapper.Map<PasswordResetToken>(readModel);

            return passwordResetToken;
        }

        public async Task<PasswordResetToken> GetEmailAsync(string email)
        {
            var readModel = await _authContext.PasswordResetTokens.SingleOrDefaultAsync(p => p.Email == email);

            var passwordResetToken = _mapper.Map<PasswordResetToken>(readModel);

            return passwordResetToken;
        }

        public async Task<PasswordResetToken> GetByGeneratedTokenAsync(
            string token)
        {
            var readModel = await _authContext.PasswordResetTokens.SingleOrDefaultAsync(p => p.Token == token);

            var passwordResetToken = _mapper.Map<PasswordResetToken>(readModel);

            return passwordResetToken;
        }

        public async Task CommitChangesAsync(PasswordResetToken aggregate)
        {
            var readModel = await _authContext.PasswordResetTokens
                    .SingleOrDefaultAsync(p => p.Guid == aggregate.Guid);

            readModel = _mapper.Map<PasswordResetTokenReadModel>(aggregate);

            _authContext.PasswordResetTokens.AddOrUpdate(p => p.Id, readModel);

            await _authContext.SaveChangesAsync();
        }
    }
}