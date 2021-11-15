using System;
using AutoMapper;

namespace BookLovers.Seed.Mappings
{
    public class DateTimeConverter : ITypeConverter<string, DateTime?>
    {
        public DateTime? Convert(string source, DateTime? destination, ResolutionContext context)
        {
            return DateTime.TryParse(source, out var result) ? result : (DateTime?) null;
        }
    }
}