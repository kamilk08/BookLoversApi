using BookLovers.Base.Domain.Extensions;

namespace BookLovers.Librarians.Domain.PromotionWaiters
{
    public class PromotionAvailability : Enumeration
    {
        public static readonly PromotionAvailability Available = new PromotionAvailability(1, nameof(Available));
        public static readonly PromotionAvailability Promoted = new PromotionAvailability(3, nameof(Promoted));
        public static readonly PromotionAvailability UnAvailable = new PromotionAvailability(2, nameof(UnAvailable));

        protected PromotionAvailability()
        {
        }

        public PromotionAvailability(int value, string name)
            : base(value, name)
        {
        }
    }
}