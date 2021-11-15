using System.Linq;
using BookLovers.Base.Infrastructure.Extensions;

namespace BookLovers.Publication.Domain.Books.IsbnValidation
{
    public class IsbnThirteenValidator : IIsbnValidator
    {
        public IsbnType Type => IsbnType.ISBN13;

        // POSITION OF CONTROL CHARACTER IN ISBN-13.
        // BASSICALLY IS A LAST NUMBER IN A CODE.
        private readonly byte ControlCharacter = 13;

        public bool IsIsbnValid(string isbn)
        {
            if (isbn.IsEmpty())
                return false;

            isbn = NormalizeIsbn(isbn);
            var isbnCharArray = isbn.ToCharArray();

            if (!CheckIsbnLength(isbnCharArray))
                return false;

            var isbnAddition = Addition(isbnCharArray);
            return IsRemainderEqualToZero(isbnAddition);
        }

        private int Addition(char[] charArray)
        {
            var sum = 0;
            for (var i = 0; i < charArray.Length; i++)
            {
                if (!byte.TryParse(charArray[i].ToString(), out var digit))
                    return 0;

                // sum += i % 2 == 0 ? byte.Parse(charArray[i].ToString())
                //    : byte.Parse(charArray[i].ToString()) * 3;
                sum += i % 2 == 0 ? digit : digit * 3;
            }

            return sum;
        }

        private string NormalizeIsbn(string isbn)
        {
            return isbn.Any(p => p == '-') ? isbn.Replace("-", string.Empty) : isbn;
        }

        private bool CheckIsbnLength(char[] isbnCharArray)
        {
            return isbnCharArray.Length == this.ControlCharacter;
        }

        private string GetIsbnControlCharacter(char[] isbnArray)
        {
            return isbnArray[ControlCharacter].ToString();
        }

        private bool IsRemainderEqualToZero(int isbnAddition)
        {
            return isbnAddition % 10 == 0;
        }
    }
}