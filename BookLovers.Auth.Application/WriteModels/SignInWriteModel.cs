using System;

namespace BookLovers.Auth.Application.WriteModels
{
    public class SignInWriteModel
    {
        public string Password { get; set; }

        public Guid TokenGuid { get; set; }

        public string Email { get; set; }

        public string UserName { get; set; }
    }
}