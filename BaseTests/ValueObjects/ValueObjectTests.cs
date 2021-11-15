using FluentAssertions;
using NUnit.Framework;

namespace BaseTests.ValueObjects
{
    [TestFixture]
    public class ValueObjectTests
    {
        private DummyValueObject _valueObject;
        private DummyValueObject _secondValueObject;
        private DummyObjectThatSupposedToBeImmutable _valueObjectThatSupposedToBeImmutable;
        private DummyValueObjectThatPretendsThatIsImmutable _valueObjectWithPublicSetters;

        [Test]
        public void Equals_WhenCalledWithTwoSameValueObjects_ShouldReturnTrue()
        {
            _valueObject = new DummyValueObject(1, "1");
            _secondValueObject = new DummyValueObject(1, "1");

            _valueObject.Should().BeEquivalentTo(_secondValueObject);
        }

        [Test]
        public void Equals_WhenCalledWithTwoDifferentObjects_ShouldReturnFalse()
        {
            _valueObject = new DummyValueObject(1, "1");
            _secondValueObject = new DummyValueObject(2, "2");

            _valueObject.Should().NotBeSameAs(_secondValueObject);
        }

        [Test]
        public void ValueObject_WhenCreated_ShouldBeImmutable()
        {
            _valueObject = new DummyValueObject(1, "2");

            _valueObject.Should().BeImmutable();
        }

        [Test]
        public void ValueObjectWithPrivateSetters_WhenCreated_ShouldBeImmutable()
        {
            _valueObjectThatSupposedToBeImmutable =
                new DummyObjectThatSupposedToBeImmutable(1, "2");

            _valueObjectThatSupposedToBeImmutable.Should().BeImmutable();
        }

        [Test]
        public void ValueObjectWithPublicSetters_WhenCreated_ShouldNotImmutable()
        {
            _valueObjectWithPublicSetters = new DummyValueObjectThatPretendsThatIsImmutable(1, "1");

            _valueObjectWithPublicSetters.Should().NotBeImmutable();
        }

        [SetUp]
        public void SetUp()
        {
        }
    }
}