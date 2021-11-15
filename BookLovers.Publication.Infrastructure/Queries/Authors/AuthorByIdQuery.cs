using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Publication.Infrastructure.Dtos.Publications;

namespace BookLovers.Publication.Infrastructure.Queries.Authors
{
    public class AuthorByIdQuery : IQuery<AuthorDto>
    {
        public int AuthorId { get; set; }

        public AuthorByIdQuery()
        {
        }

        public AuthorByIdQuery(int authorId)
        {
            this.AuthorId = authorId;
        }
    }
}