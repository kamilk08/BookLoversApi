using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Ninject.Infrastructure.Language;

namespace BookLovers.Base.Infrastructure.Serialization
{
    internal class NonPublicContractResolver : DefaultContractResolver
    {
        protected override IList<JsonProperty> CreateProperties(
            Type type,
            MemberSerialization memberSerialization)
        {
            return type
                .GetProperties(BindingFlags.Instance
                               | BindingFlags.Static
                               | BindingFlags.Public | BindingFlags.NonPublic)
                .Select(s => CreateProperty(s, memberSerialization))
                .ToList();
        }

        protected override JsonProperty CreateProperty(
            MemberInfo member,
            MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);
            var propertyInfo = member as PropertyInfo;

            if (propertyInfo != null && propertyInfo.IsPrivate())
                property.Readable = true;

            if (!property.Writable && (member as PropertyInfo)?.GetSetMethod(true) != null)
                property.Writable = true;

            return property;
        }
    }
}