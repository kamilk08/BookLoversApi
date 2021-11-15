using BookLovers.Base.Infrastructure.Queries;

namespace BookLovers.Bookcases.Infrastructure.Queries
{
    public class ShowBookcaseToOthersQuery : IQuery<bool>
    {
        public int BookcaseId { get; }

        public ShowBookcaseToOthersQuery(int bookcaseId)
        {
            BookcaseId = bookcaseId;
        }
    }
}