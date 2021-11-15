using System.Data.Entity;
using System.Reflection;
using BookLovers.Base.Infrastructure.Messages;
using BookLovers.Ratings.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Ratings.Infrastructure.Persistence
{
    public class RatingsContext : DbContext
    {
        public static readonly string ConnectionStringKey = "RatingsConnectionString";

        public DbSet<BookReadModel> Books { get; set; }

        public DbSet<AuthorReadModel> Authors { get; set; }

        public DbSet<RatingReadModel> Ratings { get; set; }

        public DbSet<InBoxMessage> InboxMessages { get; set; }

        public DbSet<SeriesReadModel> Series { get; set; }

        public DbSet<ReaderReadModel> Readers { get; set; }

        public DbSet<PublisherReadModel> Publishers { get; set; }

        public DbSet<PublisherCycleReadModel> PublisherCycles { get; set; }

        public DbSet<OutboxMessage> OutboxMessages { get; set; }

        public RatingsContext(string connectionString)
            : base(connectionString)
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<RatingsContext>());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            this.Configuration.LazyLoadingEnabled = false;

            modelBuilder.Configurations.AddFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}