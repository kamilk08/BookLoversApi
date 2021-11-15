using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using FluentAssertions.Primitives;
using FluentAssertions.Types;

namespace BaseTests
{
    public static class Extensions
    {
        public static void AllPropertiesBeDecoratedWith(this TypeAssertions typeAssertions, Type type)
        {
            var subject = typeAssertions.Subject;

            var isDecorated = true;

            var properties = subject.GetProperties();

            foreach (var property in properties)
            {
                var selectedAttribute = property.GetCustomAttribute(type);
                if (selectedAttribute == null)
                    isDecorated = false;
            }

            isDecorated.Should().BeTrue();
        }

        public static void HaveNonPublicParameterlessConstructor(this TypeAssertions typeAssertions)
        {
            var subject = typeAssertions.Subject;

            var constructors = subject.GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance);

            var constructor = constructors.FirstOrDefault(p => p.IsPrivate && p.GetParameters().Length == 0);

            constructor.Should().NotBeNull();
        }

        public static void NotHavePublicParameterlessConstructor(this TypeAssertions typeAssertions)
        {
            var invalidConstructors = new List<ConstructorInfo>();

            var subject = typeAssertions.Subject;

            var constructors = subject.GetConstructors();

            foreach (var constructor in constructors)
            {
                if (constructor.IsPublic)
                {
                    if (constructor.GetParameters().Length == 0)
                        invalidConstructors.Add(constructor);
                }
            }

            invalidConstructors.Should().BeEmpty();
        }

        public static void NotHaveReferenceTo(this TypeAssertions typeAssertions, Type type)
        {
            var propertiesReferences = new List<PropertyInfo>();
            var fieldsReferences = new List<FieldInfo>();

            var subject = typeAssertions.Subject;
            var properties = subject.GetProperties();
            var fields = subject.GetFields();

            foreach (var property in properties)
            {
                if (property.PropertyType == type || property.PropertyType.BaseType == type)
                    propertiesReferences.Add(property);
            }

            foreach (var field in fields)
            {
                if (field.FieldType == type || field.FieldType.BaseType == type)
                    fieldsReferences.Add(field);
            }

            propertiesReferences.Should().BeEmpty();
            fieldsReferences.Should().BeEmpty();
        }

        public static void BeImmutable(this ObjectAssertions objectAssertions)
        {
            var notImmutable = new List<PropertyInfo>();

            var objectType = objectAssertions.GetType();
            var properties = objectType.GetProperties();
            foreach (var property in properties)
            {
                if (!property.CanWrite)
                    continue;

                if (property.CanWrite)
                {
                    var setter = property.GetSetMethod();
                    if (setter == null)
                        continue;
                    else
                    {
                        notImmutable.Add(property);
                        break;
                    }
                }
            }

            notImmutable.Should().BeEmpty();
        }

        public static void BeImmutable(this TypeAssertions typeAssertions)
        {
            var notImmutable = new List<PropertyInfo>();

            var properties = typeAssertions.Subject.GetProperties();

            foreach (var property in properties)
            {
                if (!property.CanWrite)
                    continue;

                if (property.CanWrite)
                {
                    var setter = property.GetSetMethod();
                    if (setter == null)
                        continue;
                    else
                    {
                        notImmutable.Add(property);
                    }
                }
            }

            notImmutable.Should().BeEmpty();
        }

        public static void NotBeImmutable(this TypeAssertions typeAssertions)
        {
            var immutable = new List<PropertyInfo>();

            var objectType = typeAssertions.Subject;
            var properties = objectType.GetProperties();

            foreach (var property in properties)
            {
                if (!property.CanWrite)
                {
                    immutable.Add(property);
                    break;
                }

                if (property.CanRead && property.CanWrite)
                {
                    var setter = property.GetSetMethod();
                    if (setter == null)
                    {
                        immutable.Add(property);
                        break;
                    }
                    else continue;
                }
            }

            immutable.Should().BeEmpty();
        }

        public static void NotBeImmutable(this ObjectAssertions objectAssertions)
        {
            var immutable = new List<PropertyInfo>();

            var objectType = objectAssertions.Subject.GetType();
            var properties = objectType.GetProperties();

            foreach (var property in properties)
            {
                if (!property.CanWrite)
                {
                    immutable.Add(property);
                    break;
                }

                if (property.CanRead && property.CanWrite)
                {
                    var setter = property.GetSetMethod();
                    if (setter == null)
                    {
                        immutable.Add(property);
                        break;
                    }
                    else continue;
                }
            }

            immutable.Should().BeEmpty();
        }
    }
}