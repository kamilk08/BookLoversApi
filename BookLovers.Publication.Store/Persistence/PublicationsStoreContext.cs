using System.Data.Entity;
using System.Reflection;

namespace BookLovers.Publication.Store.Persistence
{
    public class PublicationsStoreContext : DbContext
    {
        public static readonly string ConnectionStringKey = "PublicationsStoreConnectionString";

        public DbSet<EventEntity> EventEntities { get; set; }

        public DbSet<Snapshot> Snapshots { get; set; }

        public PublicationsStoreContext(string connectionString)
            : base(connectionString)
        {
            Database.SetInitializer(
                new CreateDatabaseIfNotExists<PublicationsStoreContext>());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.AddFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}