using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BookLovers.Auth.Domain.RegistrationSummaries;
using BookLovers.Auth.Domain.RegistrationSummaries.Services;
using BookLovers.Base.Infrastructure;

namespace BookLovers.Auth.Infrastructure.Persistence.Repositories
{
    internal class RegistrationSummaryRepository :
        IRegistrationSummaryRepository,
        IRepository<RegistrationSummary>
    {
        private const int DefaultAmountOfSummaries = 25;

        private readonly AuthContext _authContext;
        private readonly IMapper _mapper;

        public RegistrationSummaryRepository(AuthContext authContext, IMapper mapper)
        {
            _authContext = authContext;
            _mapper = mapper;
        }

        public async Task<RegistrationSummary> GetRegistrationSummaryByUserGuidAsync(
            Guid userGuid)
        {
            var readModel = await _authContext.RegistrationSummaries.SingleOrDefaultAsync(p => p.UserGuid == userGuid);

            var registrationSummary = _mapper.Map<RegistrationSummary>(readModel);

            return registrationSummary;
        }

        public async Task<RegistrationSummary> GetRegistrationSummaryByEmailAsync(
            string email)
        {
            var readModel = await _authContext.RegistrationSummaries.SingleOrDefaultAsync(p => p.Email == email);

            var registrationSummary = _mapper.Map<RegistrationSummary>(readModel);

            return registrationSummary;
        }

        public async Task<IEnumerable<RegistrationSummary>> GetRegistrationsWithoutCompletionAsync(
            int? amount = null)
        {
            var readModels = await _authContext.RegistrationSummaries
                .Where(p => p.CompletedAt == null)
                .Take(amount ?? 25).ToListAsync();

            var registrationSummaries =
                _mapper.Map<List<RegistrationSummary>>(readModels);

            return registrationSummaries;
        }

        public async Task<RegistrationSummary> GetAsync(Guid aggregateGuid)
        {
            var readModel = await _authContext.RegistrationSummaries
                .SingleOrDefaultAsync(p => p.Guid == aggregateGuid);

            var registrationSummary = _mapper.Map<RegistrationSummary>(readModel);

            return registrationSummary;
        }

        public async Task CommitChangesAsync(RegistrationSummary aggregate)
        {
            var readModel = await _authContext.RegistrationSummaries
                .SingleOrDefaultAsync(p => p.Guid == aggregate.Guid);

            readModel = _mapper.Map(aggregate, readModel);

            _authContext.RegistrationSummaries
                .AddOrUpdate(p => p.Email, readModel);

            await _authContext.SaveChangesAsync();
        }
    }
}