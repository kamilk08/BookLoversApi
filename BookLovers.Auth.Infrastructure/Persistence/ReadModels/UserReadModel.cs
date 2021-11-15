using System;
using System.Collections.Generic;
using BookLovers.Base.Infrastructure.Queries;

namespace BookLovers.Auth.Infrastructure.Persistence.ReadModels
{
    public class UserReadModel : IReadModel<UserReadModel>, IReadModel
    {
        public int Id { get; set; }

        public Guid Guid { get; set; }

        public string UserName { get; set; }

        public AccountReadModel Account { get; set; }

        public int AccountId { get; set; }

        public int Status { get; set; }

        public IList<UserRoleReadModel> Roles { get; set; }

        public UserReadModel() => Roles = new List<UserRoleReadModel>();
    }
}