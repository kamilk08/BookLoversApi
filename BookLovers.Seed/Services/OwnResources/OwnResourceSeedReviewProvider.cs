using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookLovers.Seed.Models;
using BookLovers.Seed.Models.Configuration;

namespace BookLovers.Seed.Services.OwnResources
{
    internal class OwnResourceSeedReviewProvider :
        IConfigurableSeedProvider<SeedReview, OwnResourceConfiguration>,
        ISeedProvider<SeedReview>,
        ISeedProvider
    {
        private OwnResourceConfiguration _configuration;
        private List<SeedReview> _seedReviews;
        private List<SeedUser> _seedUsers;

        public SeedProviderType ProviderType => SeedProviderType.Reviews;

        public SourceType SourceType => SourceType.OwnSource;

        public Task<IEnumerable<SeedReview>> ProvideAsync()
        {
            this._seedReviews = new List<SeedReview>();

            for (var index = 0; index < this._configuration.ReviewsCount; ++index)
            {
                var seedUser = this._seedUsers[index];

                this._seedReviews.Add(new SeedReview()
                {
                    ReviewDate = DateTime.UtcNow.AddDays(index + 1),
                    ReviewGuid = Guid.NewGuid(),
                    Content = Guid.NewGuid().ToString("N"),
                    MarkedAsSpoiler = false,
                    SeedUser = seedUser
                });
            }

            var task = Task.FromResult(this._seedReviews.ToList().AsEnumerable());

            this._seedReviews.Clear();

            return task;
        }

        public IConfigurableSeedProvider<SeedReview, OwnResourceConfiguration> SetSeedConfiguration(
            OwnResourceConfiguration configuration)
        {
            this._configuration = configuration;

            return this;
        }

        public IConfigurableSeedProvider<SeedReview, OwnResourceConfiguration> SetSeedUsers(
            IEnumerable<SeedUser> seedUsers)
        {
            this._seedUsers = seedUsers.ToList();

            return this;
        }
    }
}