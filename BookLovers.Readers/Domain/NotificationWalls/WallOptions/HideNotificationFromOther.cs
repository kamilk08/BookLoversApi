namespace BookLovers.Readers.Domain.NotificationWalls.WallOptions
{
    public class HideNotificationFromOther : IWallOption
    {
        public bool Enabled { get; }

        public WallOptionType Option => WallOptionType.HideNotificationFromOther;

        private HideNotificationFromOther()
        {
        }

        public HideNotificationFromOther(bool enabled)
        {
            Enabled = enabled;
        }

        public IWallOption SwitchOption()
        {
            return new HideNotificationFromOther(!Enabled);
        }
    }
}