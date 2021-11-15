using BookLovers.Base.Domain.Extensions;

namespace BookLovers.Auth.Domain.Audiences
{
    public class AudienceState : Enumeration
    {
        public static readonly AudienceState Active = new AudienceState(1, nameof(Active));
        public static readonly AudienceState Blocked = new AudienceState(2, nameof(Blocked));

        public AudienceState(int value, string name)
            : base(value, name)
        {
        }
    }
}