using System;

namespace BookLovers.Auth.Application.WriteModels
{
    public class SignUpWriteModel
    {
        public Guid BookcaseGuid { get; set; }

        public Guid UserGuid { get; set; }

        public Guid ProfileGuid { get; set; }

        public AccountWriteModel Account { get; set; }
    }
}