using System;
using System.Linq;
using BookLovers.Auth.Domain.Audiences;
using BookLovers.Auth.Domain.Users;
using BookLovers.Auth.Infrastructure.Persistence;
using BookLovers.Auth.Infrastructure.Persistence.ReadModels;
using BookLovers.Auth.Infrastructure.Services;
using BookLovers.Auth.Infrastructure.Services.Hashing;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Base.Infrastructure.Persistence;
using Ninject;

namespace BookLovers.Auth.Infrastructure.Root.Persistence
{
    internal static class PersistenceStartup
    {
        internal static void Initialize(PersistenceSettings settings)
        {
            var context = CompositionRoot.Kernel.Get<AuthContext>();

            using (context)
            {
                if (settings.InitialSettings.DropDatabase)
                    context.Database.Delete();

                context.Database.CreateIfNotExists();

                if (settings.InitialSettings.CleanContext)
                    context.CleanAuthContext();

                SeedInitialData(context);
            }
        }

        private static void SeedInitialData(AuthContext context)
        {
            if (!context.Roles.Any())
            {
                context.Roles.Add(new UserRoleReadModel()
                {
                    Name = Role.Reader.Name,
                    Value = Role.Reader.Value
                });
                context.Roles.Add(new UserRoleReadModel()
                {
                    Name = Role.Librarian.Name,
                    Value = Role.Librarian.Value
                });
                context.Roles.Add(new UserRoleReadModel()
                {
                    Name = Role.SuperAdmin.Name,
                    Value = Role.SuperAdmin.Value
                });
            }

            if (!context.Audiences.Any())
            {
                var hashWithSalt = new HashingService(new Pbkdf2Hasher()).CreateHashWithSalt("DUPA");
                context.Audiences.Add(new AudienceReadModel()
                {
                    AudienceGuid = new Guid("00f80a32-0205-4aff-94d9-46635d8c431c"),
                    Hash = hashWithSalt.Item1,
                    Salt = hashWithSalt.Item2,
                    AudienceName = AudienceType.AngularSpa.Name,
                    AudienceType = AudienceType.AngularSpa.Value,
                    Status = AggregateStatus.Active.Value,
                    AudienceState = AudienceState.Active.Value,
                    AudienceStateName = AudienceState.Active.Name,
                    RefreshTokenLifeTime = 10080
                });
            }

            context.SaveChanges();
        }
    }
}