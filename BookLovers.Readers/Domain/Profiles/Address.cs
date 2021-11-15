using BookLovers.Base.Domain.ValueObject;

namespace BookLovers.Readers.Domain.Profiles
{
    public class Address : ValueObject<Address>
    {
        public string Country { get; }

        public string City { get; }

        private Address()
        {
        }

        public Address(string country, string city)
        {
            Country = country;
            City = city;
        }

        public static Address Default()
        {
            return new Address(null, null);
        }

        protected override bool EqualsCore(Address obj)
        {
            return City == obj.City && Country == obj.Country;
        }

        protected override int GetHashCodeCore()
        {
            var hash = 17;

            hash = (hash * 23) + this.City.GetHashCode();
            hash = (hash * 23) + this.Country.GetHashCode();

            return hash;
        }
    }
}