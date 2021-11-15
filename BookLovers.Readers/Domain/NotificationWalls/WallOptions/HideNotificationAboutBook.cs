using BookLovers.Base.Domain.ValueObject;

namespace BookLovers.Readers.Domain.NotificationWalls.WallOptions
{
    public class HideNotificationAboutBook : ValueObject<HideNotificationAboutBook>, IWallOption
    {
        public bool Enabled { get; }

        public WallOptionType Option => WallOptionType.HideNotificationAboutBook;

        private HideNotificationAboutBook()
        {
        }

        public HideNotificationAboutBook(bool enabled) => Enabled = enabled;

        public IWallOption SwitchOption() => new HideNotificationAboutBook(!Enabled);

        protected override int GetHashCodeCore()
        {
            var hash = 17;

            hash = (hash * 23) + Enabled.GetHashCode();
            hash = (hash * 23) + Option.GetHashCode();

            return hash;
        }

        protected override bool EqualsCore(HideNotificationAboutBook obj)
        {
            return Enabled == obj.Enabled && Option == obj.Option;
        }
    }
}