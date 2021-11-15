using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Threading.Tasks;
using AutoMapper;
using BookLovers.Auth.Domain.Audiences;
using BookLovers.Base.Infrastructure;

namespace BookLovers.Auth.Infrastructure.Persistence.Repositories
{
    internal class AudienceRepository : IRepository<Audience>
    {
        private readonly AuthContext _authContext;
        private readonly IMapper _mapper;

        public AudienceRepository(AuthContext authContext, IMapper mapper)
        {
            _authContext = authContext;
            _mapper = mapper;
        }

        public async Task<Audience> GetAsync(Guid aggregateGuid)
        {
            var readModel = await _authContext.Audiences
                .SingleOrDefaultAsync(p => p.AudienceGuid == aggregateGuid);

            var audience = _mapper.Map<Audience>(readModel);

            return audience;
        }

        public async Task CommitChangesAsync(Audience aggregate)
        {
            var readModel = await _authContext.Audiences
                .SingleOrDefaultAsync(p => p.AudienceGuid == aggregate.Guid);

            readModel = _mapper.Map(aggregate, readModel);

            _authContext.Audiences.AddOrUpdate(p => p.AudienceGuid, readModel);

            await _authContext.SaveChangesAsync();
        }
    }
}