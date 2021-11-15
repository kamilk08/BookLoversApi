using System.Data.Entity;
using System.Reflection;
using BookLovers.Base.Infrastructure.Messages;
using BookLovers.Readers.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Readers.Infrastructure.Persistence
{
    public class ReadersContext : DbContext
    {
        public static readonly string ConnectionStringKey = "ReadersConnectionString";

        public DbSet<AvatarReadModel> Avatars { get; set; }

        public DbSet<AddedResourceReadModel> AddedResources { get; set; }

        public DbSet<FollowReadModel> FollowObjects { get; set; }

        public DbSet<ReaderReadModel> Readers { get; set; }

        public DbSet<ReviewLikeReadModel> ReviewLikes { get; set; }

        public DbSet<ReviewReadModel> Reviews { get; set; }

        public DbSet<ReviewEditReadModel> EditedReviews { get; set; }

        public DbSet<ReviewSpoilerTagReadModel> ReviewSpoilerTags { get; set; }

        public DbSet<ReviewReportReadModel> ReviewReports { get; set; }

        public DbSet<ProfileReadModel> Profiles { get; set; }

        public DbSet<ProfilePrivacyManagerReadModel> PrivacyManagers { get; set; }

        public DbSet<SexReadModel> Sexes { get; set; }

        public DbSet<TimeLineReadModel> TimeLines { get; set; }

        public DbSet<TimeLineActivityReadModel> TimeLineActivities { get; set; }

        public DbSet<BookReadModel> Books { get; set; }

        public DbSet<AuthorReadModel> Authors { get; set; }

        public DbSet<ProfileFavouriteReadModel> ProfileFavourites { get; set; }

        public DbSet<FavouriteReadModel> Favourites { get; set; }

        public DbSet<FavouriteOwnerReadModel> FavouriteOwners { get; set; }

        public DbSet<NotificationWallReadModel> NotificationWalls { get; set; }

        public DbSet<NotificationReadModel> Notifications { get; set; }

        public DbSet<InBoxMessage> InboxMessages { get; set; }

        public DbSet<OutboxMessage> OutboxMessages { get; set; }

        public DbSet<StatisticsReadModel> Statistics { get; set; }

        public ReadersContext(string connectionString)
            : base(connectionString)
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<ReadersContext>());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            this.Configuration.LazyLoadingEnabled = false;

            modelBuilder.Configurations.AddFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}