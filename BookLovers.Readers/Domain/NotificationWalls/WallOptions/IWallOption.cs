namespace BookLovers.Readers.Domain.NotificationWalls.WallOptions
{
    public interface IWallOption
    {
        bool Enabled { get; }

        WallOptionType Option { get; }

        IWallOption SwitchOption();
    }
}