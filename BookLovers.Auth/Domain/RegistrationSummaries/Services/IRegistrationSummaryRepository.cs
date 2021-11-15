using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;

namespace BookLovers.Auth.Domain.RegistrationSummaries.Services
{
    public interface IRegistrationSummaryRepository : IRepository<RegistrationSummary>
    {
        Task<RegistrationSummary> GetRegistrationSummaryByUserGuidAsync(
            Guid userGuid);

        Task<RegistrationSummary> GetRegistrationSummaryByEmailAsync(string email);

        Task<IEnumerable<RegistrationSummary>> GetRegistrationsWithoutCompletionAsync(
            int? amount = null);
    }
}