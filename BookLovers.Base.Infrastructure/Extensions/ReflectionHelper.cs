using System;
using System.Globalization;
using System.Reflection;

namespace BookLovers.Base.Infrastructure.Extensions
{
    public class ReflectionHelper
    {
        public static Assembly[] Assemblies = AppDomain.CurrentDomain.GetAssemblies();

        public static object CreateInstance(Type type)
        {
            var flags = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public;
            var cultureInfo = CultureInfo.InvariantCulture;

            return Activator.CreateInstance(type, flags, null, null, cultureInfo);
        }

        public static object CreateInstance(string assemblyName, string typeName)
        {
            var flags = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public;
            var cultureInfo = CultureInfo.InvariantCulture;

            return Activator.CreateInstance(assemblyName, typeName, true, flags, null, null, cultureInfo, null)
                .Unwrap();
        }
    }
}