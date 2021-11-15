using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Publication.Infrastructure.Dtos.Publications;

namespace BookLovers.Publication.Infrastructure.Queries.Authors
{
    public class AuthorByNameQuery : IQuery<AuthorDto>
    {
        public string Name { get; set; }

        public AuthorByNameQuery()
        {
        }

        public AuthorByNameQuery(string name)
        {
            this.Name = name;
        }
    }
}