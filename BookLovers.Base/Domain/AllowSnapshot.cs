using System;

namespace BookLovers.Base.Domain
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class AllowSnapshot : Attribute
    {
    }
}