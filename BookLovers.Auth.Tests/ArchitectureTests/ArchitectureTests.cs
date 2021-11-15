using System.Linq;
using System.Reflection;
using BaseTests;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Base.Domain.Entity;
using BookLovers.Base.Domain.Events;
using BookLovers.Base.Domain.Rules;
using BookLovers.Base.Domain.ValueObject;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;
using BookLovers.Base.Infrastructure.Queries;
using FluentAssertions;
using FluentValidation;
using NUnit.Framework;

namespace BookLovers.Auth.Tests.ArchitectureTests
{
    [TestFixture]
    public class ArchitectureTests
    {
        private Assembly _applicationAssembly;
        private Assembly _infrastructureAssembly;
        private Assembly _domainAssembly;

        [Test]
        public void Command_Should_BeImmutable()
        {
            var commands = _applicationAssembly.GetTypes()
                .Where(p => !p.IsInterface && typeof(ICommand)
                    .IsAssignableFrom(p)).ToList();

            foreach (var subject in commands)
                subject.Should().BeImmutable();
        }

        [Test]
        public void Command_ShouldHaveNameEndingWith_Command()
        {
            var commands = _applicationAssembly.GetTypes()
                .Where(p => !p.Name.EndsWith("InternalCommand"))
                .Where(p => typeof(ICommand).IsAssignableFrom(p));

            foreach (var memberInfo in commands)
                memberInfo.Name.Should().EndWith("Command");
        }

        [Test]
        public void Command_EachClass_ShouldBePublic()
        {
            var commands = _applicationAssembly.GetTypes()
                .Where(p => !p.Name.EndsWith("InternalCommand"))
                .Where(p => typeof(ICommand).IsAssignableFrom(p));

            foreach (var type in commands)
                type.IsPublic.Should().BeTrue();
        }

        [Test]
        public void InternalCommand_EachClassNameShouldEndWith_InternalCommandPostFix()
        {
            var commands = _applicationAssembly.GetTypes()
                .Where(p => p.Name.EndsWith("InternalCommand"));

            foreach (var memberInfo in commands)
                memberInfo.Name.Should().EndWith("InternalCommand");
        }

        [Test]
        public void InternalCommand_EachClassShouldNotBePublic()
        {
            var commands = _applicationAssembly.GetTypes()
                .Where(p => p.Name.EndsWith("InternalCommand"));

            foreach (var type in commands)
                type.IsPublic.Should().BeFalse();
        }

        [Test]
        public void Query_Should_BeImmutable()
        {
            var queries = _infrastructureAssembly.GetTypes()
                .Where(p =>
                    p.GetInterfaces().Any(a => a.IsGenericType && a.GetGenericTypeDefinition() == typeof(IQuery<>)));

            foreach (var subject in queries)
                subject.Should().BeImmutable();
        }

        [Test]
        public void Query_ShouldHaveNameEndingWith_Query()
        {
            var queries = _infrastructureAssembly.GetTypes()
                .Where(p => p.GetInterfaces()
                    .Any(a => a.IsGenericType && a.GetGenericTypeDefinition() ==
                        typeof(IQuery<>)));

            foreach (var memberInfo in queries)
                memberInfo.Name.Should().EndWith("Query");
        }

        [Test]
        public void Query_EachClass_ShouldBePublic()
        {
            var queries = _infrastructureAssembly.GetTypes()
                .Where(p =>
                    p.GetInterfaces().Any(a => a.IsGenericType && a.GetGenericTypeDefinition() ==
                        typeof(IQuery<>)));

            foreach (var type in queries)
                type.IsPublic.Should().BeTrue();
        }

        [Test]
        public void Dto_ShouldHaveNameEndingWith_Dto()
        {
            var dtos = _infrastructureAssembly
                .GetTypes()
                .Where(p => p.IsClass && p.Namespace == "BookLovers.Auth.Infrastructure.Dtos");

            foreach (var memberInfo in dtos)
                memberInfo.Name.Should().EndWith("Dto");
        }

        [Test]
        public void Dto_ShouldNot_BeImmutable()
        {
            var dtos = _infrastructureAssembly
                .GetTypes()
                .Where(p => p.IsClass && p.Namespace == "BookLovers.Auth.Infrastructure.Dtos");

            foreach (var subject in dtos)
                subject.Should().NotBeImmutable();
        }

        [Test]
        public void WriteModel_ShouldNot_BeImmutable()
        {
            var writeModels = _applicationAssembly.GetTypes()
                .Where(p => p.IsClass && p.Namespace == "BookLovers.Auth.Application.WriteModels").ToList();

            foreach (var subject in writeModels)
                subject.Should().NotBeImmutable();
        }

        [Test]
        public void WriteModel_Should_EndWithWriteModelPostFix()
        {
            var writeModels = _applicationAssembly.GetTypes()
                .Where(p => p.IsClass && p.Namespace == "BookLovers.Auth.Application.WriteModels").ToList();

            foreach (var memberInfo in writeModels)
                memberInfo.Name.Should().EndWith("WriteModel");
        }

        [Test]
        public void CommandHandler_ShouldHaveNameEndingWith_Handler()
        {
            var commandHandlers = _applicationAssembly.GetTypes().Where(p =>
                p.GetInterfaces().Any(a =>
                    a.IsGenericType && a.GetGenericTypeDefinition() == typeof(ICommandHandler<>)));

            foreach (var memberInfo in commandHandlers)
                memberInfo.Name.Should().EndWith("Handler");
        }

        [Test]
        public void CommandHandler_EachClass_ShouldNotBePublic()
        {
            var commandHandlers = _applicationAssembly.GetTypes().Where(p =>
                p.GetInterfaces().Any(a =>
                    a.IsGenericType && a.GetGenericTypeDefinition() == typeof(ICommandHandler<>)));

            foreach (var type in commandHandlers)
                type.IsPublic.Should().BeFalse();
        }

        [Test]
        public void QueryHandler_EachClass_ShouldNotBePublic()
        {
            var queryHandlers = _infrastructureAssembly.GetTypes().Where(p =>
                p.GetInterfaces().Any(a =>
                    a.IsGenericType && a.GetGenericTypeDefinition() == typeof(IQueryHandler<,>)));

            foreach (var type in queryHandlers)
                type.IsPublic.Should().BeFalse();
        }

        [Test]
        public void QueryHandler_EachClassNameShould_EndWithHandlerPostFix()
        {
            var queryHandlers = _infrastructureAssembly.GetTypes().Where(p =>
                p.GetInterfaces().Any(a =>
                    a.IsGenericTypeDefinition && a.GetGenericTypeDefinition() == typeof(IQueryHandler<,>)));

            foreach (var memberInfo in queryHandlers)
                memberInfo.Name.Should().EndWith("Handler");
        }

        [Test]
        public void IntegrationHandler_ShouldHaveNameEndingWith_Handler()
        {
            var handlers = _applicationAssembly.GetTypes()
                .Where(p => p.Namespace == "BookLovers.Auth.Application.Integration").Where(p =>
                    p.GetInterfaces().Any(a =>
                        a.IsGenericType && a.GetGenericTypeDefinition() == typeof(IIntegrationEventHandler<>)));
            foreach (var memberInfo in handlers)

                memberInfo.Name.Should().EndWith("Handler");
        }

        [Test]
        public void IntegrationHandler_EachClass_ShouldNotBePublic()
        {
            var handlers = _applicationAssembly.GetTypes()
                .Where(p => p.Namespace == "BookLovers.Auth.Application.Integration").Where(p =>
                    p.GetInterfaces().Any(a =>
                        a.IsGenericType && a.GetGenericTypeDefinition() == typeof(IIntegrationEventHandler<>)));

            foreach (var type in handlers)
                type.IsPublic.Should().BeFalse();
        }

        [Test]
        public void DomainEventHandler_EachClassShouldEndWith_HandlerPostFix()
        {
            var handlers = _applicationAssembly.GetTypes()
                .Where(p => p.Namespace == "BookLovers.Auth.Application.Events").Where(p =>
                    p.GetInterfaces().Any(a =>
                        a.IsGenericTypeDefinition &&
                        a.GetGenericTypeDefinition() == typeof(IDomainEventHandler<>)));

            foreach (var memberInfo in handlers)
                memberInfo.Name.Should().EndWith("Handler");
        }

        [Test]
        public void DomainEventHandler_EachClassShould_NotBePublic()
        {
            var handlers = _applicationAssembly.GetTypes()
                .Where(p => p.Namespace == "BookLovers.Auth.Application.Events")
                .Where(p =>
                    p.IsGenericTypeDefinition && p.GetGenericTypeDefinition() == typeof(IDomainEventHandler<>));

            foreach (var type in handlers)
                type.IsPublic.Should().BeFalse();
        }

        [Test]
        public void Validator_ShouldHaveNameEndingWith_Validator()
        {
            var validators = _infrastructureAssembly.GetTypes()
                .Where(p => p.BaseType != null)
                .Where(p => p.BaseType.GenericTypeArguments.Length == 1).Where(p =>
                    p.IsSubclassOf(
                        typeof(AbstractValidator<>).MakeGenericType(p.BaseType.GenericTypeArguments.First())));

            foreach (var memberInfo in validators)
                memberInfo.Name.Should().EndWith("Validator");
        }

        [Test]
        public void Validator_EachClass_ShouldNotBePublic()
        {
            var validators = _infrastructureAssembly.GetTypes()
                .Where(p => p.BaseType != null)
                .Where(p => p.BaseType.GenericTypeArguments.Length == 1).Where(p =>
                    p.IsSubclassOf(
                        typeof(AbstractValidator<>).MakeGenericType(p.BaseType.GenericTypeArguments.First())));

            foreach (var type in validators)
                type.IsPublic.Should().BeFalse();
        }

        [Test]
        public void BusinessRule_Should_NotBePublic()
        {
            var rules = _domainAssembly.GetTypes()
                .Where(p => p.IsSubclassOf(typeof(BaseBusinessRule)))
                .Where(p => typeof(IBusinessRule).IsAssignableFrom(p));

            foreach (var type in rules)
                type.IsPublic.Should().BeFalse();
        }

        [Test]
        public void BusinessRule_AllShould_ImplementIBusinessRule()
        {
            var rules = _domainAssembly.GetTypes().Where(p => p.IsSubclassOf(typeof(BaseBusinessRule)))
                .Where(p => typeof(IBusinessRule).IsAssignableFrom(p));

            foreach (var subject in rules)
                subject.Should().Implement<IBusinessRule>();
        }

        [Test]
        public void AggregateRoot_Should_BeImmutable()
        {
            var roots = _domainAssembly.GetTypes()
                .Where(p => p.IsSubclassOf(typeof(AggregateRoot)))
                .ToList();

            foreach (var subject in roots)
                subject.Should().BeImmutable();
        }

        [Test]
        public void AggregateRoot_ShouldNotHave_PublicParameterlessConstructor()
        {
            var roots = _domainAssembly.GetTypes()
                .Where(p => p.IsSubclassOf(typeof(AggregateRoot)))
                .ToList();

            foreach (var subject in roots)
                subject.Should().NotHavePublicParameterlessConstructor();
        }

        [Test]
        public void Entity_ShouldNotHaveReferenceTo_OtherOrOwnAggregateRoot()
        {
            var entities = _domainAssembly.GetTypes()
                .Where(p => p.IsSubclassOf(typeof(IEntityObject)))
                .ToList();

            foreach (var subject in entities)
                subject.Should().NotHaveReferenceTo(typeof(AggregateRoot));
        }

        [Test]
        public void Entity_ShouldHave_PrivateParameterlessConstructor()
        {
            var entities = _domainAssembly.GetTypes()
                .Where(p => p.IsSubclassOf(typeof(IEntityObject)))
                .ToList();

            foreach (var subject in entities)
                subject.Should().HaveNonPublicParameterlessConstructor();
        }

        [Test]
        public void ValueObject_ShouldBe_Immutable()
        {
            var valueObjects = _domainAssembly.GetTypes()
                .Where(p => p.BaseType != null)
                .Where(p => p.BaseType.GenericTypeArguments.Length > 0).Where(p =>
                    p.IsSubclassOf(typeof(ValueObject<>).MakeGenericType(p.BaseType.GenericTypeArguments.First())))
                .ToList();

            foreach (var subject in valueObjects)
                subject.Should().BeImmutable();
        }

        [Test]
        public void ValueObject_ShouldHave_PrivateParameterlessConstructor()
        {
            foreach (var subject in _domainAssembly.GetTypes()
                .Where(p => p.BaseType != null)
                .Where(p => p.BaseType.GenericTypeArguments.Length > 0)
                .Where(p =>
                    p.IsSubclassOf(typeof(ValueObject<>).MakeGenericType(p.BaseType.GenericTypeArguments.First())))
                .ToList())
                subject.Should().HaveNonPublicParameterlessConstructor();
        }

        [Test]
        public void Event_EachClassShould_BeImmutable()
        {
            var events = _domainAssembly.GetTypes().Where(p =>
                typeof(IEvent).IsAssignableFrom(p) || typeof(IStateEvent).IsAssignableFrom(p));

            foreach (var subject in events)
                subject.Should().BeImmutable();
        }

        [Test]
        public void DomainAssembly_ShouldNot_HaveDependencyFromApplicationAssembly()
        {
            _domainAssembly
                .GetReferencedAssemblies().Should()
                .NotContain(p => p.FullName == _applicationAssembly.FullName);
        }

        [Test]
        public void DomainAssembly_ShouldNot_HaveDependencyFromInfrastructureAssembly()
        {
            _domainAssembly
                .GetReferencedAssemblies().Should()
                .NotContain(p => p.FullName == _infrastructureAssembly.FullName);
        }

        [Test]
        public void ApplicationAssembly_ShouldNot_HaveDependencyFromInfrastructureAssembly()
        {
            _applicationAssembly
                .GetReferencedAssemblies().Should()
                .NotContain(p => p.FullName == _infrastructureAssembly.FullName);
        }

        [SetUp]
        public void SetUp()
        {
            _applicationAssembly = Assembly.Load("BookLovers.Auth.Application");
            _infrastructureAssembly = Assembly.Load("BookLovers.Auth.Infrastructure");
            _domainAssembly = Assembly.Load("BookLovers.Auth");
        }
    }
}