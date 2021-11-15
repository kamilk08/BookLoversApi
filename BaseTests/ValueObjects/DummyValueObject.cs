using BookLovers.Base.Domain.ValueObject;

namespace BaseTests.ValueObjects
{
    public class DummyValueObject : ValueObject<DummyValueObject>
    {
        public int DummyInteger { get; }

        public string DummyString { get; }

        public DummyValueObject(int dummyInteger, string dummyString)
        {
            DummyInteger = dummyInteger;
            DummyString = dummyString;
        }

        protected override int GetHashCodeCore()
        {
            return (((17 * 23) + DummyInteger.GetHashCode()) * 23) + DummyString.GetHashCode();
        }

        protected override bool EqualsCore(DummyValueObject obj) =>
            DummyInteger == obj.DummyInteger && DummyString == obj.DummyString;
    }
}