using BookLovers.Base.Domain.ValueObject;

namespace BookLovers.Readers.Domain.NotificationWalls.WallOptions
{
    public class HideNotificationAboutReview : ValueObject<HideNotificationAboutReview>, IWallOption
    {
        public bool Enabled { get; }

        public WallOptionType Option => WallOptionType.HideNotificationAboutReview;

        private HideNotificationAboutReview()
        {
        }

        public HideNotificationAboutReview(bool enabled)
        {
            Enabled = enabled;
        }

        public IWallOption SwitchOption()
        {
            return new HideNotificationAboutReview(!Enabled);
        }

        protected override int GetHashCodeCore()
        {
            var hash = 17;

            hash = (hash * 23) + Enabled.GetHashCode();
            hash = (hash * 23) + Option.GetHashCode();

            return hash;
        }

        protected override bool EqualsCore(HideNotificationAboutReview obj)
        {
            return Enabled == obj.Enabled
                   && Option == obj.Option;
        }
    }
}