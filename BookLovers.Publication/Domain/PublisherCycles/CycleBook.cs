using System;
using BookLovers.Base.Domain.ValueObject;

namespace BookLovers.Publication.Domain.PublisherCycles
{
    public class CycleBook : ValueObject<CycleBook>
    {
        public Guid BookGuid { get; }

        private CycleBook()
        {
        }

        public CycleBook(Guid bookGuid)
        {
            this.BookGuid = bookGuid;
        }

        protected override bool EqualsCore(CycleBook obj)
        {
            return this.BookGuid == obj.BookGuid;
        }

        protected override int GetHashCodeCore()
        {
            return (17 * 23) + this.BookGuid.GetHashCode();
        }
    }
}