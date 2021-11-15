using System.Linq;
using BookLovers.Base.Infrastructure.Persistence;
using BookLovers.Readers.Infrastructure.Persistence;
using BookLovers.Readers.Infrastructure.Persistence.ReadModels;
using BookLovers.Readers.Store.Persistence;
using BookLovers.Shared.SharedSexes;
using Ninject;

namespace BookLovers.Readers.Infrastructure.Root.Persistence
{
    internal static class PersistenceStartup
    {
        internal static void Initialize(PersistenceSettings settings)
        {
            var context = CompositionRoot.Kernel.Get<ReadersContext>();
            var storeContext = CompositionRoot.Kernel.Get<ReadersStoreContext>();
            using (context)
            {
                if (settings.InitialSettings.DropDatabase)
                    context.Database.Delete();

                context.Database.CreateIfNotExists();
                if (settings.InitialSettings.CleanContext)
                    context.CleanReadersContext();

                SeedInitialData(context);
            }

            using (storeContext)
            {
                if (settings.InitialSettings.DropDatabase)
                    storeContext.Database.Delete();

                storeContext.Database.CreateIfNotExists();

                if (!settings.InitialSettings.CleanContext)
                    return;

                storeContext.CleanReadersStore();
            }
        }

        private static void SeedInitialData(ReadersContext context)
        {
            if (!context.Sexes.Any())
                context.Sexes.AddRange(Sexes.Choices.Select(s => new SexReadModel()
                {
                    Id = s.Value,
                    Name = s.Name
                }));

            context.SaveChanges();
        }
    }
}