using FluentAssertions;
using NUnit.Framework;

namespace BaseTests.Infrastructure
{
    [TestFixture]
    public class EnumerationTests
    {
        private DummyEnumeration _dummyEnumeration;
        private DummyEnumeration _secondDummyEnumeration;

        [Test]
        public void WhenAssigned_DummyEnumeration_ShouldHaveDesiredNameAndValue()
        {
            _dummyEnumeration = DummyEnumeration.DummyOne;
            _dummyEnumeration.Should().NotBeNull();
            _dummyEnumeration.Name.Should().Be(DummyEnumeration.DummyOne.Name);
            _dummyEnumeration.Value.Should().Be(DummyEnumeration.DummyOne.Value);
        }

        [Test]
        public void WhenEqualityIsChecked_AndBothObjectsAreOfTypeEnumerationAndTheSameObject_ShouldReturnTrue()
        {
            _dummyEnumeration = DummyEnumeration.DummyOne;
            _secondDummyEnumeration = DummyEnumeration.DummyOne;

            (_dummyEnumeration == _secondDummyEnumeration).Should().BeTrue();
        }

        [Test]
        public void WhenEqualityIsChecked_AndObjectsAreOfTypeEnumerationButNotTheSameObject_ShouldReturnFalse()
        {
            _dummyEnumeration = DummyEnumeration.DummyOne;
            _secondDummyEnumeration = DummyEnumeration.DummyTwo;

            (_dummyEnumeration == _secondDummyEnumeration).Should().BeFalse();
        }

        [SetUp]
        public void SetUp()
        {
        }
    }
}