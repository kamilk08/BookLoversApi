using System.Collections.Generic;
using System.Linq;
using BookLovers.Base.Infrastructure.Extensions;
using BookLovers.Publication.Domain.Books.IsbnValidation;

namespace BookLovers.Publication.Domain.Books.Services
{
    public class IsbnValidatorFactory
    {
        private const byte Isbn10 = 10;
        private const byte Isbn13 = 13;
        private readonly IDictionary<IsbnType, IIsbnValidator> _validators;

        public IsbnValidatorFactory(IDictionary<IsbnType, IIsbnValidator> validators)
        {
            this._validators = validators;
        }

        public IIsbnValidator GetValidator(string isbn)
        {
            isbn = this.FormatIsbn(isbn);
            return this.GetIsbnValidatorBasedOnIsbnLength(isbn);
        }

        private string FormatIsbn(string isbn)
        {
            if (isbn.IsEmpty())
                return string.Empty;

            isbn = isbn.Replace("-", string.Empty);

            return isbn;
        }

        private IIsbnValidator GetIsbnValidatorBasedOnIsbnLength(string isbn)
        {
            return this._validators.SingleOrDefault(p => p.Key.IsbnNumberLength
                                                         == isbn.Length).Value;
        }
    }
}