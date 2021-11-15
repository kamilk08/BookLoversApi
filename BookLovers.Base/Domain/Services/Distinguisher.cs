using System.Collections.Generic;
using System.Linq;

namespace BookLovers.Base.Domain.Services
{
    public static class Distinguisher<T>
    {
        public static IEnumerable<T> ToAdd(IEnumerable<T> oldSequence, IEnumerable<T> newSequence)
        {
            if (oldSequence.SequenceEqual(newSequence))
                return Enumerable.Empty<T>();

            return newSequence.Where(p => !oldSequence.Contains(p)).AsEnumerable();
        }

        public static IEnumerable<T> ToRemove(IEnumerable<T> oldSequence, IEnumerable<T> newSequence)
        {
            if (oldSequence.SequenceEqual(newSequence))
                return Enumerable.Empty<T>();

            return oldSequence.Where(p => !newSequence.Contains(p)).AsEnumerable();
        }
    }
}