using BookLovers.Publication.Domain.Books.IsbnValidation;
using FluentAssertions;
using NUnit.Framework;

namespace BookLovers.Publication.Tests.UnitTests.IsbnValidation
{
    [TestFixture]
    public class IsbnTenValidatiorTests
    {
        private IsbnTenValidator _validator;

        [Test]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        [TestCase("0-19-852663-6a")]
        [TestCase("1 86197 271 7")]
        public void IsIsbnValid_InvalidIsbnFormatNumber_ShouldReturnFalse(string isbn)
        {
            _validator = new IsbnTenValidator();
            var result = _validator.IsIsbnValid(isbn);

            result.Should().BeFalse();
        }

        [Test]
        [TestCase("0198526636")]
        [TestCase("1861972717")]
        [TestCase("055326396X")]
        public void IsIsbnValid_ValidIsbnFormatNumber_ShouldReturnTrue(string isbn)
        {
            _validator = new IsbnTenValidator();
            var result = _validator.IsIsbnValid(isbn);

            result.Should().BeTrue();
        }
    }
}