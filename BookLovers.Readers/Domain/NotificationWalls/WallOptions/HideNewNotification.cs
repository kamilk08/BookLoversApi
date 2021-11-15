using BookLovers.Base.Domain.ValueObject;

namespace BookLovers.Readers.Domain.NotificationWalls.WallOptions
{
    public class HideNewNotification : ValueObject<HideNewNotification>, IWallOption
    {
        public bool Enabled { get; }

        public WallOptionType Option => WallOptionType.HideNotification;

        private HideNewNotification()
        {
        }

        public HideNewNotification(bool enabled) => Enabled = enabled;

        public IWallOption SwitchOption() => new HideNewNotification(!Enabled);

        protected override int GetHashCodeCore()
        {
            var hash = 17;

            hash = (hash * 23) + Enabled.GetHashCode();
            hash = (hash * 23) + Option.GetHashCode();

            return hash;
        }

        protected override bool EqualsCore(HideNewNotification obj)
        {
            return Option == obj.Option && Enabled == obj.Enabled;
        }
    }
}