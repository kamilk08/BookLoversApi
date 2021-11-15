using BookLovers.Base.Domain.ValueObject;

namespace BaseTests.ValueObjects
{
    public class DummyObjectThatSupposedToBeImmutable :
        ValueObject<DummyObjectThatSupposedToBeImmutable>
    {
        public int DummyInteger { get; private set; }

        public string DummyString { get; private set; }

        public DummyObjectThatSupposedToBeImmutable(int dummyInteger, string dummyString)
        {
            DummyInteger = dummyInteger;
            DummyString = dummyString;
        }

        protected override int GetHashCodeCore()
        {
            return (((17 * 23) + DummyInteger.GetHashCode()) * 23)
                   + DummyString.GetHashCode();
        }

        protected override bool EqualsCore(DummyObjectThatSupposedToBeImmutable obj) =>
            DummyInteger == obj.DummyInteger && DummyString == obj.DummyString;
    }
}