using BookLovers.Base.Domain.Extensions;

namespace BookLovers.Shared.Categories
{
    public class Category : Enumeration
    {
        public static readonly Category NonFiction = new Category(1, "Non-Fiction");
        public static readonly Category Fiction = new Category(2, "Fiction");

        private Category()
        {
        }

        protected Category(int value, string name) : base(value, name)
        {

        }
    }
}
