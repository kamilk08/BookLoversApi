using System.Data.Entity;
using System.Reflection;
using BookLovers.Base.Infrastructure.Messages;
using BookLovers.Publication.Infrastructure.Persistence.ReadModels;
using BookLovers.Publication.Infrastructure.Persistence.ReadModels.Authors;
using BookLovers.Publication.Infrastructure.Persistence.ReadModels.Books;
using BookLovers.Publication.Infrastructure.Persistence.ReadModels.Quotes;

namespace BookLovers.Publication.Infrastructure.Persistence
{
    public class PublicationsContext : DbContext
    {
        public static readonly string ConnectionStringKey = "PublicationsConnectionString";

        public DbSet<AuthorReadModel> Authors { get; set; }

        public DbSet<AuthorImageReadModel> AuthorImages { get; set; }

        public DbSet<AuthorFollowerReadModel> AuthorFollowers { get; set; }

        public DbSet<BookReadModel> Books { get; set; }

        public DbSet<BookCoverReadModel> BookCovers { get; set; }

        public DbSet<QuoteReadModel> Quotes { get; set; }

        public DbSet<QuoteLikeReadModel> QuoteLikes { get; set; }

        public DbSet<ReaderReadModel> Readers { get; set; }

        public DbSet<ReviewReadModel> Reviews { get; set; }

        public DbSet<PublisherReadModel> Publishers { get; set; }

        public DbSet<CategoryReadModel> Categories { get; set; }

        public DbSet<SubCategoryReadModel> SubCategories { get; set; }

        public DbSet<SeriesReadModel> Series { get; set; }

        public DbSet<CoverTypeReadModel> CoverTypes { get; set; }

        public DbSet<PublisherCycleReadModel> PublisherCycles { get; set; }

        public DbSet<LanguageReadModel> Languages { get; set; }

        public DbSet<OutboxMessage> OutboxMessages { get; set; }

        public DbSet<InBoxMessage> InBoxMessages { get; set; }

        public PublicationsContext(string connectionString)
            : base(connectionString)
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<PublicationsContext>());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            this.Configuration.LazyLoadingEnabled = false;

            modelBuilder.Configurations.AddFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}