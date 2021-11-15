using System.Data.Entity;
using System.Reflection;

namespace BookLovers.Bookcases.Store.Persistence
{
    public class BookcaseStoreContext : DbContext
    {
        public static readonly string ConnectionStringKey = "BookcaseStoreConnectionString";

        public DbSet<EventEntity> EventEntities { get; set; }

        public DbSet<Snapshot> Snapshots { get; set; }

        public BookcaseStoreContext(string connectionString)
            : base(connectionString)
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<BookcaseStoreContext>());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.AddFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}