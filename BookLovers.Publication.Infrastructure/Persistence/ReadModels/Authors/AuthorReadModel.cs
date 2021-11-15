using System;
using System.Collections.Generic;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Publication.Infrastructure.Persistence.ReadModels.Books;
using BookLovers.Publication.Infrastructure.Persistence.ReadModels.Quotes;

namespace BookLovers.Publication.Infrastructure.Persistence.ReadModels.Authors
{
    public class AuthorReadModel : IReadModel<AuthorReadModel>, IReadModel
    {
        public int Id { get; set; }

        public Guid Guid { get; set; }

        public int Status { get; set; }

        public string FullName { get; set; }

        public string FirstName { get; set; }

        public string SecondName { get; set; }

        public DateTime? BirthDate { get; set; }

        public DateTime? DeathDate { get; set; }

        public string BirthPlace { get; set; }

        public int Sex { get; set; }

        public string AboutAuthor { get; set; }

        public string DescriptionSource { get; set; }

        public string AuthorWebsite { get; set; }

        public IList<BookReadModel> AuthorBooks { get; set; }

        public IList<AuthorFollowerReadModel> AuthorFollowers { get; set; }

        public IList<QuoteReadModel> Quotes { get; set; }

        public IList<SubCategoryReadModel> SubCategories { get; set; }

        public ReaderReadModel AddedBy { get; set; }

        public int? AddedById { get; set; }

        public AuthorReadModel()
        {
            this.AuthorBooks = new List<BookReadModel>();
            this.AuthorFollowers = new List<AuthorFollowerReadModel>();
            this.Quotes = new List<QuoteReadModel>();
        }
    }
}