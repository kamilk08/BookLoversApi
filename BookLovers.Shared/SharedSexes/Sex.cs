using BookLovers.Base.Domain.Extensions;

namespace BookLovers.Shared.SharedSexes
{
    public class Sex : Enumeration
    {
        public static readonly Sex Hidden = new Sex(1, "Hidden");
        public static readonly Sex Male = new Sex(2, "Male");
        public static readonly Sex Female = new Sex(3, "Female");

        private Sex()
        {
        }

        internal Sex(int value, string name) : base(value, name)
        {
        }
    }
}