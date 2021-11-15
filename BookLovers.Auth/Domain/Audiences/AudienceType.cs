using BookLovers.Base.Domain.Extensions;

namespace BookLovers.Auth.Domain.Audiences
{
    public class AudienceType : Enumeration
    {
        public static readonly AudienceType AngularSpa = new AudienceType(1, "AngularSPA");
        public static readonly AudienceType ExternalAudience = new AudienceType(2, nameof(ExternalAudience));

        private AudienceType()
        {
        }

        public AudienceType(int value, string name)
            : base(value, name)
        {
        }
    }
}