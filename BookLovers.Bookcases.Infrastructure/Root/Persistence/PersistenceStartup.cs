using System.Reflection;
using BookLovers.Base.Infrastructure.Persistence;
using BookLovers.Bookcases.Infrastructure.Persistence;
using BookLovers.Bookcases.Store.Persistence;
using DbUp;
using Ninject;

namespace BookLovers.Bookcases.Infrastructure.Root.Persistence
{
    internal static class PersistenceStartup
    {
        internal static void Initialize(PersistenceSettings settings)
        {
            var bookcaseContext = CompositionRoot.Kernel.Get<BookcaseContext>();
            var storeContext = CompositionRoot.Kernel.Get<BookcaseStoreContext>();

            using (bookcaseContext)
            {
                if (settings.InitialSettings.DropDatabase)
                    bookcaseContext.Database.Delete();

                bookcaseContext.Database.CreateIfNotExists();

                if (settings.InitialSettings.CleanContext)
                    bookcaseContext.CleanBookcaseContext();

                if (settings.InitialSettings.SeedProcedures)
                    SeedProcedures(bookcaseContext);
            }

            using (storeContext)
            {
                if (settings.InitialSettings.DropDatabase)
                    storeContext.Database.Delete();

                storeContext.Database.CreateIfNotExists();

                if (!settings.InitialSettings.CleanContext)
                    return;

                storeContext.ClearBookcaseStore();
            }
        }

        private static void SeedProcedures(BookcaseContext context)
        {
            var upgrader = DeployChanges.To
                .SqlDatabase(context.Database.Connection.ConnectionString)
                .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                .LogToConsole().Build();

            upgrader.PerformUpgrade();
        }
    }
}