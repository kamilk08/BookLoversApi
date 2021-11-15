using System.Data.Entity;
using System.Reflection;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Messages;
using BookLovers.Bookcases.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Bookcases.Infrastructure.Persistence
{
    public class BookcaseContext : DbContext
    {
        public static readonly string ConnectionStringKey = "BookcaseConnectionString";

        public DbSet<BookcaseReadModel> Bookcases { get; set; }

        public DbSet<SettingsManagerReadModel> SettingsManagers { get; set; }

        public DbSet<ShelfReadModel> Shelves { get; set; }

        public DbSet<BookReadModel> Books { get; set; }

        public DbSet<ReaderReadModel> Readers { get; set; }

        public DbSet<OutboxMessage> OutboxMessages { get; set; }

        public DbSet<InBoxMessage> InBoxMessages { get; set; }

        public DbSet<ShelfRecordReadModel> BookOnShelvesRecords { get; set; }

        public DbSet<ShelfRecordTrackerReadModel> ShelfRecordTrackers { get; set; }

        public DbSet<InternalCommand> InternalCommands { get; set; }

        public BookcaseContext(string connectionString)
            : base(connectionString)
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<BookcaseContext>());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Configuration.LazyLoadingEnabled = false;

            modelBuilder.Configurations.AddFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}