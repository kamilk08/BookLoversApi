using BookLovers.Base.Domain.ValueObject;
using BookLovers.Shared.Privacy;

namespace BookLovers.Bookcases.Domain.Settings
{
    public class BookcasePrivacy : ValueObject<BookcasePrivacy>, IBookcaseOption
    {
        public BookcaseOptionType Type => BookcaseOptionType.Privacy;

        public PrivacyOption PrivacyOption { get; }

        public int SelectedOption => PrivacyOption.Value;

        private BookcasePrivacy()
        {
        }

        public BookcasePrivacy(PrivacyOption privacyOption)
        {
            PrivacyOption = privacyOption;
        }

        public static BookcasePrivacy DefaultOption() =>
            new BookcasePrivacy(PrivacyOption.Private);

        public BookcasePrivacy ChangeTo(int selectedOption) =>
            new BookcasePrivacy(AvailablePrivacyOptions.Get(selectedOption));

        protected override int GetHashCodeCore()
        {
            var hash = 17;

            hash = (hash * 23) + Type.GetHashCode();
            hash = (hash * 23) + SelectedOption.GetHashCode();

            return hash;
        }

        protected override bool EqualsCore(BookcasePrivacy obj) =>
            Type == obj.Type && SelectedOption == obj.SelectedOption;
    }
}