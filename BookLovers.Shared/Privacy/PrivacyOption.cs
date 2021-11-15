using BookLovers.Base.Domain.Extensions;
using Newtonsoft.Json;

namespace BookLovers.Shared.Privacy
{
    public class PrivacyOption : Enumeration
    {
        public static readonly PrivacyOption Private = new PrivacyOption(1, "Private");
        public static readonly PrivacyOption OtherReaders = new PrivacyOption(2,"OtherReaders");
        public static readonly PrivacyOption Public = new PrivacyOption(3, "Public");

        private PrivacyOption()
        {
            
        }
        
        [JsonConstructor]
        protected PrivacyOption(int value, string name) : base(value, name) { }
        
    }
}
