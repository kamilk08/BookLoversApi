using BookLovers.Base.Domain.Extensions;

namespace BaseTests.Infrastructure
{
    internal class DummyEnumeration : Enumeration
    {
        internal static readonly DummyEnumeration DummyOne = new DummyEnumeration(1, nameof(DummyOne));
        internal static readonly DummyEnumeration DummyTwo = new DummyEnumeration(2, nameof(DummyTwo));

        protected DummyEnumeration(int value, string name)
            : base(value, name)
        {
        }
    }
}