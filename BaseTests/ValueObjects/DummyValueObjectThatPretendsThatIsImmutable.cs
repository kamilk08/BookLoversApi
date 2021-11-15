using BookLovers.Base.Domain.ValueObject;

namespace BaseTests.ValueObjects
{
    public class DummyValueObjectThatPretendsThatIsImmutable :
        ValueObject<DummyValueObjectThatPretendsThatIsImmutable>
    {
        public int DummyInteger { get; set; }

        public string DummyString { get; set; }

        public DummyValueObjectThatPretendsThatIsImmutable(int dummyInteger, string dummyString)
        {
            DummyInteger = dummyInteger;
            DummyString = dummyString;
        }

        protected override int GetHashCodeCore() =>
            (((17 * 23) + DummyInteger.GetHashCode()) * 23) + DummyString.GetHashCode();

        protected override bool EqualsCore(DummyValueObjectThatPretendsThatIsImmutable obj) =>
            DummyInteger == obj.DummyInteger && DummyString == obj.DummyString;
    }
}