using BookLovers.Base.Domain.Extensions;

namespace BookLovers.Librarians.Domain.Librarians
{
    public class Decision : Enumeration
    {
        public static readonly Decision Approve = new Decision(1, nameof(Approve));
        public static readonly Decision Decline = new Decision(2, nameof(Decline));
        public static readonly Decision Unknown = new Decision(3, nameof(Unknown));

        protected Decision()
        {
        }

        public Decision(int value, string name)
            : base(value, name)
        {
        }
    }
}