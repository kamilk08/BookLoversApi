namespace BookLovers.Bookcases.Domain.Settings
{
    public class ShelfCapacity : BookLovers.Base.Domain.ValueObject.ValueObject<ShelfCapacity>, IBookcaseOption
    {
        public static readonly int MinCapacity = 20;
        public static readonly int MaxCapacity = 100;

        public BookcaseOptionType Type => BookcaseOptionType.ShelfCapacity;

        public int SelectedOption { get; }

        private ShelfCapacity()
        {
        }

        public ShelfCapacity(int selectedOption)
        {
            SelectedOption = selectedOption;
        }

        public static ShelfCapacity DefaultCapacity()
        {
            return new ShelfCapacity(MinCapacity);
        }

        public ShelfCapacity SetCapacity(int input)
        {
            return new ShelfCapacity(input);
        }

        public bool CurrentCapacityExceeded(Shelf shelf)
        {
            return shelf.Books.Count > SelectedOption;
        }

        public bool MaxCapacityExceeded(Shelf shelf)
        {
            return shelf.Books.Count > MaxCapacity;
        }

        protected override int GetHashCodeCore()
        {
            var hash = 17;

            hash = (hash * 23) + SelectedOption.GetHashCode();
            hash = (hash * 23) + Type.GetHashCode();

            return hash;
        }

        protected override bool EqualsCore(ShelfCapacity obj)
            => SelectedOption == obj.SelectedOption && Type.Equals(obj.Type);
    }
}