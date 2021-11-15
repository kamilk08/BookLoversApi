using BookLovers.Base.Domain.ValueObject;

namespace BookLovers.Readers.Domain.Readers
{
    public class ReaderIdentification : ValueObject<ReaderIdentification>
    {
        public string Email { get; }

        public string Username { get; }

        public int ReaderId { get; }

        private ReaderIdentification()
        {
        }

        public ReaderIdentification(int readerId, string username, string email)
        {
            ReaderId = readerId;
            Username = username;
            Email = email;
        }

        protected override bool EqualsCore(ReaderIdentification obj)
        {
            return Username == obj.Username
                   && ReaderId == obj.ReaderId && Email == obj.Email;
        }

        protected override int GetHashCodeCore()
        {
            var hash = 17;

            hash = (hash * 23) + this.Username.GetHashCode();
            hash = (hash * 23) + this.ReaderId.GetHashCode();
            hash = (hash * 23) + this.Email.GetHashCode();

            return hash;
        }
    }
}