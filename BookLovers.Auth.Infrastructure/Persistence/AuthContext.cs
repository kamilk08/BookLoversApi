using System.Data.Entity;
using System.Reflection;
using BookLovers.Auth.Infrastructure.Persistence.ReadModels;
using BookLovers.Base.Infrastructure.Messages;

namespace BookLovers.Auth.Infrastructure.Persistence
{
    public class AuthContext : DbContext
    {
        public static readonly string ConnectionStringKey = "AuthContextConnectionString";

        public DbSet<UserReadModel> Users { get; set; }

        public DbSet<AccountReadModel> Accounts { get; set; }

        public DbSet<RefreshTokenReadModel> RefreshTokens { get; set; }

        public DbSet<PasswordResetTokenReadModel> PasswordResetTokens { get; set; }

        public DbSet<UserRoleReadModel> Roles { get; set; }

        public DbSet<OutboxMessage> OutboxMessages { get; set; }

        public DbSet<InBoxMessage> InboxMessages { get; set; }

        public DbSet<AudienceReadModel> Audiences { get; set; }

        public DbSet<RegistrationSummaryReadModel> RegistrationSummaries { get; set; }

        public AuthContext(string connectionString)
            : base(connectionString)
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<AuthContext>());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Configuration.LazyLoadingEnabled = false;
            modelBuilder.Configurations.AddFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}