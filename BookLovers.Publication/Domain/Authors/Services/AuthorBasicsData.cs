using BookLovers.Shared;
using BookLovers.Shared.SharedSexes;

namespace BookLovers.Publication.Domain.Authors.Services
{
    public class AuthorBasicsData
    {
        public FullName FullName { get; }

        public Sex Sex { get; }

        internal AuthorBasicsData(FullName fullName, Sex sex)
        {
            this.FullName = fullName;
            this.Sex = sex;
        }
    }
}