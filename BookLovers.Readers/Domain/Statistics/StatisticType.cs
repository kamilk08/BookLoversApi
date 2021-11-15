using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BookLovers.Base.Domain.Extensions;
using BookLovers.Base.Infrastructure.Extensions;
using Newtonsoft.Json;

namespace BookLovers.Readers.Domain.Statistics
{
    public class StatisticType : Enumeration
    {
        public static readonly StatisticType ReceivedLikes = new StatisticType(0, "Received likes");
        public static readonly StatisticType GivenLikes = new StatisticType(1, "Given likes");
        public static readonly StatisticType Shelves = new StatisticType(2, nameof(Shelves));
        public static readonly StatisticType Reviews = new StatisticType(3, nameof(Reviews));
        public static readonly StatisticType Followers = new StatisticType(4, nameof(Followers));
        public static readonly StatisticType Followings = new StatisticType(5, nameof(Followings));
        public static readonly StatisticType BooksInBookcase = new StatisticType(6, "Books in bookcase");
        public static readonly StatisticType AddedQuotes = new StatisticType(7, "Added quotes");
        public static readonly StatisticType AddedAuthors = new StatisticType(8, "Added authors");
        public static readonly StatisticType AddedBooks = new StatisticType(9, "Added books");

        public static readonly List<IStatistic> AvailableStatistics =
            Assembly.GetExecutingAssembly().GetTypes()
                .Where(p => !p.IsInterface && typeof(IStatistic).IsAssignableFrom(p))
                .Select(s => ReflectionHelper.CreateInstance(s) as IStatistic).OrderBy(p => p.Type.Value).ToList();

        [JsonConstructor]
        protected StatisticType(byte value, string name)
            : base(value, name)
        {
        }
    }
}