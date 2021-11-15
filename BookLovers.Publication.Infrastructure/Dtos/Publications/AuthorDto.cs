using System;
using System.Collections.Generic;

namespace BookLovers.Publication.Infrastructure.Dtos.Publications
{
    public class AuthorDto
    {
        public int Id { get; set; }

        public Guid Guid { get; set; }

        public int AuthorStatus { get; set; }

        public string AboutAuthor { get; set; }

        public string DescriptionSource { get; set; }

        public string AuthorWebSite { get; set; }

        public int Sex { get; set; }

        public string FirstName { get; set; }

        public string SecondName { get; set; }

        public DateTime? BirthDate { get; set; }

        public DateTime? DeathDate { get; set; }

        public string BirthPlace { get; set; }

        public IList<int> AuthorBooks { get; set; }

        public IList<int> AuthorQuotes { get; set; }

        public IList<AuthorFollowerDto> AuthorFollowers { get; set; }

        public IList<SubCategoryDto> AuthorSubCategories { get; set; }

        public int AddedByReaderId { get; set; }
    }
}