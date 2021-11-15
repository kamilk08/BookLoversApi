using System.Linq;
using BookLovers.Base.Infrastructure.Extensions;

namespace BookLovers.Publication.Domain.Books.IsbnValidation
{
    public class IsbnTenValidator : IIsbnValidator
    {
        public IsbnType Type => IsbnType.ISBN10;

        private const char SpecialCharacter = 'X';
        private readonly byte ControlCharacter = 10;

        public bool IsIsbnValid(string isbn)
        {
            if (isbn.IsEmpty())
                return false;

            isbn = NormalizeIsbnNumber(isbn);
            var isbnCharArray = isbn.ToCharArray();

            if (!CheckLength(isbnCharArray))
                return false;

            var addition = GetAddition(isbnCharArray);

            return IsRemainderEqualToZero(addition);
        }

        private int GetAddition(char[] isbnCharArray)
        {
            var sum = 0;
            var weight = 10;

            for (var i = 0; i < isbnCharArray.Length; i++)
            {
                if (isbnCharArray[i] == SpecialCharacter)
                    sum += 10;
                else
                {
                    byte.TryParse(isbnCharArray[i].ToString(), out var digit);
                    sum += weight * digit;
                }

                weight--;
            }

            return sum;
        }

        private bool IsRemainderEqualToZero(int addition)
        {
            return addition % 11 == 0;
        }

        private string NormalizeIsbnNumber(string isbn)
        {
            return isbn.Any(p => p == '-') ? isbn.Replace("-", string.Empty) : isbn;
        }

        private bool CheckLength(char[] isbnCharArray)
        {
            return isbnCharArray.Length == ControlCharacter;
        }
    }
}