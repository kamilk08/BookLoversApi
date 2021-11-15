using System;
using BookLovers.Base.Domain.ValueObject;

namespace BookLovers.Librarians.Domain.Tickets
{
    public class SolvedBy : ValueObject<SolvedBy>
    {
        public Guid? LibrarianGuid { get; private set; }

        private SolvedBy()
        {
        }

        public SolvedBy(Guid librarianGuid)
        {
            this.LibrarianGuid = librarianGuid;
        }

        public static SolvedBy NotSolvedByAnyOne() =>
            new SolvedBy(Guid.Empty);

        public bool HasBeenSolved()
        {
            var librarianGuid = this.LibrarianGuid;

            if (!librarianGuid.HasValue)
                return true;

            return librarianGuid.HasValue &&
                   librarianGuid.GetValueOrDefault() != Guid.Empty;
        }

        protected override int GetHashCodeCore()
        {
            return (17 * 23) + this.LibrarianGuid.GetHashCode();
        }

        protected override bool EqualsCore(SolvedBy obj)
        {
            return this.LibrarianGuid == obj.LibrarianGuid;
        }
    }
}