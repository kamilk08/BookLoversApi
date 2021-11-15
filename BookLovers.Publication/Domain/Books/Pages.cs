using BookLovers.Base.Domain.ValueObject;

namespace BookLovers.Publication.Domain.Books
{
    public class Pages : ValueObject<Pages>
    {
        public int Amount { get; }

        private Pages()
        {
        }

        public Pages(int amount)
        {
            this.Amount = amount;
        }

        public static Pages Unknown()
        {
            return new Pages(0);
        }

        protected override bool EqualsCore(Pages obj)
        {
            return this.Amount == obj.Amount;
        }

        protected override int GetHashCodeCore()
        {
            return (17 * 23) + this.Amount.GetHashCode();
        }
    }
}