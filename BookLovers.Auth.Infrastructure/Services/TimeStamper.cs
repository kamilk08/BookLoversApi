using System;

namespace BookLovers.Auth.Infrastructure.Services
{
    public static class TimeStamper
    {
        public static long ToTimeStamp() => DateTimeOffset.Now.ToUnixTimeSeconds();
    }
}