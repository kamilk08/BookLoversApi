using BookLovers.Base.Domain.Extensions;

namespace BookLovers.Shared.Categories
{
    public class SubCategory : Enumeration
    {
        private SubCategory() { }

        public Category Category { get; }

        protected SubCategory(Category category, int value, string name) : base(value, name)
        {
            this.Category = category;
        }

        public class NonFictionSubCategory : SubCategory
        {
            public static readonly NonFictionSubCategory History = new NonFictionSubCategory(Category.NonFiction, 0, "History");
            public static readonly NonFictionSubCategory Academic = new NonFictionSubCategory(Category.NonFiction, 1, "Academic");
            public static readonly NonFictionSubCategory Politics = new NonFictionSubCategory(Category.NonFiction, 2, "Politics");
            public static readonly NonFictionSubCategory Design = new NonFictionSubCategory(Category.NonFiction, 3, "Design");
            public static readonly NonFictionSubCategory Health = new NonFictionSubCategory(Category.NonFiction, 4, "Health");
            public static readonly NonFictionSubCategory Travel = new NonFictionSubCategory(Category.NonFiction, 5, "Travel");

            protected NonFictionSubCategory(Category category, byte value, string name) : base(category, value, name) { }


        }

        public class FictionSubCategory : SubCategory
        {
            public static readonly FictionSubCategory Fantasy = new FictionSubCategory(Category.Fiction, 100, "Fantasy");
            public static readonly FictionSubCategory SciFi = new FictionSubCategory(Category.Fiction, 101, "Sci-Fi");
            public static readonly FictionSubCategory Romance = new FictionSubCategory(Category.Fiction, 102, "Romance");
            public static readonly FictionSubCategory Drama = new FictionSubCategory(Category.Fiction, 103, "Drama");
            public static readonly FictionSubCategory Thriller = new FictionSubCategory(Category.Fiction, 104, "Thriller");
            public static readonly FictionSubCategory Action = new FictionSubCategory(Category.Fiction, 105, "Action");

            protected FictionSubCategory(Category category, byte value, string name) : base(category, value, name) { }
        }
    }
}
