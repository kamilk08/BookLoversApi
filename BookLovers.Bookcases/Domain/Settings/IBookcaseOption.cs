namespace BookLovers.Bookcases.Domain.Settings
{
    public interface IBookcaseOption
    {
        BookcaseOptionType Type { get; }

        int SelectedOption { get; }
    }
}