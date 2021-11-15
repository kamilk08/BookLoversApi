using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookLovers.Base.Infrastructure.Extensions
{
    public static class GlobalExtensions
    {
        public static void ForEach<T>(this IList<T> list, Action<T> action)
        {
            foreach (var obj in list)
                action(obj);
        }

        public static void ForEach<T>(this IList<T> list, Action<T, int> action)
        {
            for (var index = 0; index < list.Count; ++index)
                action(list[index], index);
        }

        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (var obj in enumerable)
                action(obj);
        }

        public static async Task ForEach<T>(this IEnumerable<T> enumerable, Func<T, Task> func)
        {
            foreach (var obj in enumerable)
            {
                var item = obj;
                await func(item);
            }
        }

        public static bool IsEmpty(this string input) =>
            string.IsNullOrEmpty(input) || string.IsNullOrWhiteSpace(input);

        public static bool IsEmpty(this Guid guid) => guid == Guid.Empty;

        public static bool AreEqualWithoutTicks(this DateTime date, DateTime anotherDate)
        {
            return date.Year == anotherDate.Year && date.Day == anotherDate.Day && date.Hour == anotherDate.Hour &&
                   date.Minute == anotherDate.Minute && date.Second == anotherDate.Second &&
                   date.Month == anotherDate.Month;
        }

        public static IEnumerable<IEnumerable<T>> Partition<T>(
            this IEnumerable<T> source,
            int partitionSize)
        {
            return source.Select((t, index) => new
                {
                    index, t
                })
                .GroupBy(p => p.index % partitionSize)
                .Select(s => s.Select(y => y.t));
        }

        public static IEnumerable<T> DistinctBy<T, TKey>(
            this IEnumerable<T> source,
            Func<T, TKey> selector)
        {
            return source.GroupBy(selector)
                .Select(s => s.First())
                .AsEnumerable();
        }
    }
}