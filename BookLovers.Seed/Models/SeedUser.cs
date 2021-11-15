using System;

namespace BookLovers.Seed.Models
{
    public class SeedUser
    {
        public Guid UserGuid { get; set; }

        public Guid BookcaseGuid { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }
    }
}