using BookLovers.Publication.Domain.Books.IsbnValidation;
using FluentAssertions;
using NUnit.Framework;

namespace BookLovers.Publication.Tests.UnitTests.IsbnValidation
{
    [TestFixture]
    public class IsbnThirteenValidatorTests
    {
        private IIsbnValidator _validator;

        [Test]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        [TestCase(" 978 83 7515 495 57 ")]
        [TestCase("-979ABCDEF459678")]
        public void IsIsbnValid_WhenCalled_ShouldReturnFalse(string isbn)
        {
            _validator = new IsbnThirteenValidator();

            var result = _validator.IsIsbnValid(isbn);

            result.Should().BeFalse();
        }

        [Test]
        [TestCase("9788375154955")]
        [TestCase("978-83-7515-495-5")]
        [TestCase("9788375155280")]
        public void IsIsbnValid_ValidIsbnFormat_ShouldReturnTrue(string isbn)
        {
            _validator = new IsbnThirteenValidator();

            var result = _validator.IsIsbnValid(isbn);

            result.Should().BeTrue();
        }
    }
}