using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BookLovers.Ratings.Domain.RatingStars
{
    public static class StarList
    {
        public static readonly IReadOnlyCollection<Star> Stars = typeof
                (Star).GetFields(BindingFlags.Static | BindingFlags.Public)
            .Select(s => s.GetValue(s) as Star)
            .ToList();

        public static Star ChooseStar(int star) => StarList.Stars
            .SingleOrDefault(p => p.Value == star);
    }
}