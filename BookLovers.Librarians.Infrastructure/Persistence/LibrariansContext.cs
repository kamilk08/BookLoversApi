using BookLovers.Base.Infrastructure.Messages;
using BookLovers.Librarians.Infrastructure.Persistence.ReadModels;
using System.Data.Entity;
using System.Reflection;

namespace BookLovers.Librarians.Infrastructure.Persistence
{
    public class LibrariansContext : DbContext
    {
        public static readonly string ConnectionStringKey = "LibrariansConnectionString";

        public DbSet<LibrarianReadModel> Librarians { get; set; }

        public DbSet<ResolvedTicketReadModel> ResolvedTickets { get; set; }

        public DbSet<TicketOwnerReadModel> TicketOwners { get; set; }

        public DbSet<CreatedTicketReadModel> CreatedTickets { get; set; }

        public DbSet<TicketReadModel> Tickets { get; set; }

        public DbSet<DecisionReadModel> Decisions { get; set; }

        public DbSet<TicketConcernReadModel> TickerConcerns { get; set; }

        public DbSet<ReviewReportRegisterReadModel> ReviewReports { get; set; }

        public DbSet<ReportReasonReadModel> ReportReasons { get; set; }

        public DbSet<PromotionWaiterReadModel> PromotionWaiters { get; set; }

        public DbSet<PromotionAvailabilityReadModel> PromotionAvailabilities { get; set; }

        public DbSet<OutboxMessage> OutboxMessages { get; set; }

        public DbSet<InBoxMessage> InBoxMessages { get; set; }

        public LibrariansContext(string connectionString)
            : base(connectionString)
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<LibrariansContext>());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            this.Configuration.AutoDetectChangesEnabled = true;
            this.Configuration.LazyLoadingEnabled = false;

            modelBuilder.Configurations.AddFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}