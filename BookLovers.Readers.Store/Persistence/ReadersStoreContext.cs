using System.Data.Entity;
using System.Reflection;

namespace BookLovers.Readers.Store.Persistence
{
    public class ReadersStoreContext : DbContext
    {
        public static readonly string ConnectionStringKey = "ReadersStoreConnectionString";

        public DbSet<EventEntity> EventEntities { get; set; }

        public DbSet<Snapshot> Snapshots { get; set; }

        public ReadersStoreContext(string connectionString) : base(connectionString)
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<ReadersStoreContext>());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.AddFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}