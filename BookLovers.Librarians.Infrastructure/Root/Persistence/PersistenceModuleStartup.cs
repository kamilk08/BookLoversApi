using BookLovers.Base.Infrastructure.Persistence;
using BookLovers.Librarians.Domain.Librarians;
using BookLovers.Librarians.Domain.PromotionWaiters;
using BookLovers.Librarians.Domain.ReviewReportRegisters;
using BookLovers.Librarians.Domain.Tickets;
using BookLovers.Librarians.Infrastructure.Persistence;
using BookLovers.Librarians.Infrastructure.Persistence.ReadModels;
using Ninject;
using System.Linq;

namespace BookLovers.Librarians.Infrastructure.Root.Persistence
{
    internal static class PersistenceModuleStartup
    {
        internal static void Initialize(PersistenceSettings settings)
        {
            var context = CompositionRoot.Kernel.Get<LibrariansContext>();
            using (context)
            {
                if (settings.InitialSettings.DropDatabase)
                    context.Database.Delete();

                context.Database.CreateIfNotExists();
                if (settings.InitialSettings.CleanContext)
                    context.CleanLibrarianContext();

                SeedInitialData(context);
            }
        }

        private static void SeedInitialData(LibrariansContext context)
        {
            if (!context.Decisions.Any())
            {
                context.Decisions.Add(new DecisionReadModel()
                {
                    Name = Decision.Approve.Name,
                    Value = Decision.Approve.Value
                });
                context.Decisions.Add(new DecisionReadModel()
                {
                    Name = Decision.Decline.Name,
                    Value = Decision.Decline.Value
                });
                context.Decisions.Add(new DecisionReadModel()
                {
                    Name = Decision.Unknown.Name,
                    Value = Decision.Unknown.Value
                });
            }

            if (!context.TickerConcerns.Any())
            {
                context.TickerConcerns.Add(new TicketConcernReadModel()
                {
                    Name = TicketConcern.Author.Name,
                    Value = TicketConcern.Author.Value
                });
                context.TickerConcerns.Add(new TicketConcernReadModel()
                {
                    Name = TicketConcern.Book.Name,
                    Value = TicketConcern.Book.Value
                });
            }

            if (!context.ReportReasons.Any())
            {
                context.ReportReasons.Add(new ReportReasonReadModel()
                {
                    ReasonId = ReportReason.AbusiveContent.Value,
                    ReasonName = ReportReason.AbusiveContent.Name
                });
                context.ReportReasons.Add(new ReportReasonReadModel()
                {
                    ReasonId = ReportReason.Spam.Value,
                    ReasonName = ReportReason.Spam.Name
                });
                context.ReportReasons.Add(new ReportReasonReadModel()
                {
                    ReasonId = ReportReason.ContainsThreats.Value,
                    ReasonName = ReportReason.ContainsThreats.Name
                });
            }

            if (!context.PromotionAvailabilities.Any())
            {
                context.PromotionAvailabilities.Add(new PromotionAvailabilityReadModel()
                {
                    AvailabilityId = PromotionAvailability.Available.Value,
                    Name = PromotionAvailability.Available.Name
                });
                context.PromotionAvailabilities.Add(new PromotionAvailabilityReadModel()
                {
                    AvailabilityId = PromotionAvailability.Promoted.Value,
                    Name = PromotionAvailability.Promoted.Name
                });
                context.PromotionAvailabilities.Add(new PromotionAvailabilityReadModel()
                {
                    AvailabilityId = PromotionAvailability.UnAvailable.Value,
                    Name = PromotionAvailability.UnAvailable.Name
                });
            }

            context.SaveChanges();
        }
    }
}