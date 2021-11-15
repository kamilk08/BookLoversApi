using System;
using BookLovers.Auth.Application.WriteModels;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Auth.Application.Commands.Users
{
    public class CreateSuperAdminCommand : ICommand
    {
        public SignUpWriteModel SignUpWriteModel { get; }

        public CreateSuperAdminCommand(SignUpWriteModel signUpWriteModel)
        {
            this.SignUpWriteModel = signUpWriteModel;
        }

        public static CreateSuperAdminCommand Create()
        {
            return new CreateSuperAdminCommand(new SignUpWriteModel()
            {
                BookcaseGuid = Guid.NewGuid(),
                ProfileGuid = Guid.NewGuid(),
                UserGuid = Guid.NewGuid(),
                Account = new AccountWriteModel()
                {
                    AccountDetails = new AccountDetailsWriteModel()
                    {
                        Email = "superadmin@gmail.com",
                        UserName = "SuperAdmin"
                    },
                    AccountSecurity = new AccountSecurityWriteModel()
                    {
                        Password = "Babcia123!"
                    }
                }
            });
        }

        public static CreateSuperAdminCommand Create(Guid userGuid)
        {
            return new CreateSuperAdminCommand(
                new SignUpWriteModel()
                {
                    BookcaseGuid = Guid.NewGuid(),
                    ProfileGuid = Guid.NewGuid(),
                    UserGuid = userGuid,
                    Account = new AccountWriteModel()
                    {
                        AccountDetails = new AccountDetailsWriteModel()
                        {
                            Email = "superadmin@gmail.com",
                            UserName = "SuperAdmin"
                        },
                        AccountSecurity = new AccountSecurityWriteModel()
                        {
                            Password = "Babcia123!"
                        }
                    }
                });
        }
    }
}