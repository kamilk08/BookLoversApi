using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookLovers.Librarians.Domain.Tickets;
using BookLovers.Seed.Models;
using BookLovers.Seed.Models.Configuration;

namespace BookLovers.Seed.Services.OwnResources
{
    internal class OwnResourceSeedTicketsProvider :
        IConfigurableSeedProvider<SeedTicket, OwnResourceConfiguration>,
        ISeedProvider<SeedTicket>,
        ISeedProvider
    {
        private OwnResourceConfiguration _configuration;

        public SeedProviderType ProviderType => SeedProviderType.UserTickets;

        public SourceType SourceType => SourceType.OwnSource;

        public Task<IEnumerable<SeedTicket>> ProvideAsync()
        {
            var source = new List<SeedTicket>();
            for (var index = 0; index < this._configuration.TicketsCount; ++index)
            {
                var seedTicket = new SeedTicket();

                seedTicket.Title = Guid.NewGuid().ToString("N");
                seedTicket.Description = Guid.NewGuid().ToString("N");
                seedTicket.CreatedAt = DateTime.UtcNow.AddMinutes(index + 1);
                seedTicket.TicketConcern = TicketConcern.Author.Value;
                seedTicket.TicketData = null;

                source.Add(seedTicket);
            }

            return Task.FromResult(source.AsEnumerable());
        }

        public IConfigurableSeedProvider<SeedTicket, OwnResourceConfiguration> SetSeedConfiguration(
            OwnResourceConfiguration configuration)
        {
            this._configuration = configuration;

            return this;
        }
    }
}