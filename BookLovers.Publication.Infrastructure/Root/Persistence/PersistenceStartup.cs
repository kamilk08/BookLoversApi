using System.Linq;
using System.Reflection;
using BookLovers.Base.Infrastructure.Persistence;
using BookLovers.Publication.Domain.Books.CoverTypes;
using BookLovers.Publication.Domain.Books.Languages;
using BookLovers.Publication.Infrastructure.Persistence;
using BookLovers.Publication.Infrastructure.Persistence.ReadModels.Books;
using BookLovers.Publication.Store.Persistence;
using BookLovers.Shared.Categories;
using DbUp;
using Ninject;

namespace BookLovers.Publication.Infrastructure.Root.Persistence
{
    internal static class PersistenceStartup
    {
        internal static void Initialize(PersistenceSettings settings)
        {
            var context = CompositionRoot.Kernel.Get<PublicationsContext>();
            var storeContext = CompositionRoot.Kernel.Get<PublicationsStoreContext>();

            using (storeContext)
            {
                if (settings.InitialSettings.DropDatabase)
                    storeContext.Database.Delete();

                storeContext.Database.CreateIfNotExists();

                if (settings.InitialSettings.CleanContext)
                    storeContext.ClearPublicationsStore();
            }

            using (context)
            {
                if (settings.InitialSettings.DropDatabase)
                    context.Database.Delete();

                context.Database.CreateIfNotExists();

                if (settings.InitialSettings.CleanContext)
                    context.CleanPublicationsContext();

                SeedInitialData(context);

                if (!settings.InitialSettings.SeedProcedures)
                    return;

                SeedProcedures(context);
            }
        }

        private static void SeedProcedures(PublicationsContext context)
        {
            var upgrader = DeployChanges.To
                .SqlDatabase(context.Database.Connection.ConnectionString)
                .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                .LogToConsole().Build();

            upgrader.PerformUpgrade();
        }

        private static void SeedInitialData(PublicationsContext context)
        {
            if (!context.Categories.Any())
            {
                context.Categories.AddRange(CategoryList.Categories.Select(c =>
                    new CategoryReadModel()
                    {
                        CategoryName = c.Name,
                        Id = c.Value
                    }));
                context.SubCategories.AddRange(SubCategoryList.SubCategories.Select(
                    sc => new SubCategoryReadModel()
                    {
                        Id = sc.Value,
                        SubCategoryName = sc.Name,
                        CategoryId = sc.Category.Value
                    }));
            }

            if (!context.SubCategories.Any())
                context.Languages.AddRange(BookLanguages.Languages.Select(l =>
                    new LanguageReadModel()
                    {
                        Id = l.Value,
                        Name = l.Name
                    }));
            if (!context.CoverTypes.Any())
                context.CoverTypes.AddRange(BookCovers.CoverTypes.Select(s =>
                    new CoverTypeReadModel()
                    {
                        Id = s.Value,
                        CoverType = s.Name
                    }));
            context.SaveChanges();
        }
    }
}