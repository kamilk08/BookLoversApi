using BookLovers.Base.Infrastructure.Persistence;
using BookLovers.Ratings.Infrastructure.Persistence;
using Ninject;

namespace BookLovers.Ratings.Infrastructure.Root.Persistence
{
    internal class PersistenceStartup
    {
        internal static void Initialize(PersistenceSettings settings)
        {
            var context = CompositionRoot.Kernel.Get<RatingsContext>();

            using (context)
            {
                if (settings.InitialSettings.DropDatabase)
                    context.Database.Delete();

                context.Database.CreateIfNotExists();

                if (!settings.InitialSettings.CleanContext)
                    return;

                context.CleanRatingsContext();
            }
        }
    }
}