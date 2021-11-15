using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookLovers.Seed.Models;
using BookLovers.Seed.Models.Configuration;

namespace BookLovers.Seed.Services.OwnResources
{
    public class OwnResourceSeedUserProvider :
        IConfigurableSeedProvider<SeedUser, OwnResourceConfiguration>,
        ISeedProvider<SeedUser>,
        ISeedProvider
    {
        private OwnResourceConfiguration _configuration;

        public SeedProviderType ProviderType => SeedProviderType.Users;

        public SourceType SourceType => SourceType.OwnSource;

        public Task<IEnumerable<SeedUser>> ProvideAsync()
        {
            var source = new List<SeedUser>();

            for (var index = 0; index < this._configuration.UserCount; ++index)
            {
                source.Add(new SeedUser()
                {
                    Email = $"user{index + 1}@gmail.com",
                    Password = "Babcia123",
                    UserGuid = Guid.NewGuid(),
                    BookcaseGuid = Guid.NewGuid(),
                    UserName = $"user{index + 1}"
                });
            }

            return Task.FromResult(source.AsEnumerable());
        }

        public IConfigurableSeedProvider<SeedUser, OwnResourceConfiguration> SetSeedConfiguration(
            OwnResourceConfiguration configuration)
        {
            this._configuration = configuration;

            return this;
        }
    }
}