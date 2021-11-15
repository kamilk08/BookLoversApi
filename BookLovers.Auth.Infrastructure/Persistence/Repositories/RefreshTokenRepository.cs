using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Threading.Tasks;
using AutoMapper;
using BookLovers.Auth.Domain.Tokens;
using BookLovers.Auth.Domain.Tokens.Services;
using BookLovers.Base.Infrastructure;

namespace BookLovers.Auth.Infrastructure.Persistence.Repositories
{
    internal class RefreshTokenRepository : ITokenRepository, IRepository<RefreshToken>
    {
        private readonly AuthContext _authContext;
        private readonly IMapper _mapper;

        public RefreshTokenRepository(AuthContext authContext, IMapper mapper)
        {
            _authContext = authContext;
            _mapper = mapper;
        }

        public async Task<RefreshToken> GetTokenAsync(string token)
        {
            var readModel = await _authContext.RefreshTokens
                .SingleOrDefaultAsync(p => p.Hash == token);

            var refreshToken = _mapper.Map<RefreshToken>(readModel);

            return refreshToken;
        }

        public async Task<RefreshToken> GetAsync(Guid aggregateGuid)
        {
            var readModel =
                await _authContext.RefreshTokens
                    .SingleOrDefaultAsync(p => p.TokenGuid == aggregateGuid);

            var refreshToken = _mapper.Map<RefreshToken>(readModel);

            return refreshToken;
        }

        public async Task CommitChangesAsync(RefreshToken aggregate)
        {
            var readModel =
                await _authContext.RefreshTokens
                    .SingleOrDefaultAsync(p => p.TokenGuid == aggregate.Guid);

            readModel = _mapper.Map(aggregate, readModel);

            _authContext.RefreshTokens.AddOrUpdate(p => p.UserGuid, readModel);

            await _authContext.SaveChangesAsync();
        }
    }
}