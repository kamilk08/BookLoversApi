using System;
using System.Collections.Generic;

namespace BookLovers.Auth.Infrastructure.Dtos.Users
{
    public class UserDto
    {
        public int Id { get; set; }

        public Guid Guid { get; set; }

        public string Email { get; set; }

        public List<string> Roles { get; set; }
    }
}