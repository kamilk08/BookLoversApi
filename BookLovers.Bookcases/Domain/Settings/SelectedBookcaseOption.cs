using System.Linq;

namespace BookLovers.Bookcases.Domain.Settings
{
    public class SelectedBookcaseOption : BookLovers.Base.Domain.ValueObject.ValueObject<SelectedBookcaseOption>
    {
        public int Value { get; }

        public BookcaseOptionType OptionType { get; }

        private SelectedBookcaseOption()
        {
        }

        public SelectedBookcaseOption(int value, int optionTypeId)
        {
            Value = value;
            OptionType = BookcaseOptionType.AvailableTypes
                .SingleOrDefault(p => p.Value == optionTypeId);
        }

        public SelectedBookcaseOption(int value, BookcaseOptionType optionType)
        {
            Value = value;
            OptionType = optionType;
        }

        protected override int GetHashCodeCore()
        {
            var hash = 17;

            hash = (hash * 23) + Value.GetHashCode();
            hash = (hash * 23) + OptionType.GetHashCode();

            return hash;
        }

        protected override bool EqualsCore(SelectedBookcaseOption obj) =>
            Value == obj.Value && OptionType == obj.OptionType;
    }
}