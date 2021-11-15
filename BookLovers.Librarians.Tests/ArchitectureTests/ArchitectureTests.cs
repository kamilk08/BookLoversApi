using System;
using System.Linq;
using System.Reflection;
using BaseTests;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Base.Domain.Entity;
using BookLovers.Base.Domain.Events;
using BookLovers.Base.Domain.Rules;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;
using BookLovers.Base.Infrastructure.Queries;
using FluentAssertions;
using FluentValidation;
using NUnit.Framework;

namespace BookLovers.Librarians.Tests.ArchitectureTests
{
    [TestFixture]
    public class ArchitectureTests
    {
        private Assembly _domainAssembly;
        private Assembly _applicationAssembly;
        private Assembly _infrastructureAssembly;

        [Test]
        public void Command_Should_BeImmutable()
        {
            var commands = _applicationAssembly.GetTypes()
                .Where(p => !p.IsInterface && typeof(ICommand).IsAssignableFrom(p)).ToList();

            foreach (var command in commands)
                command.Should().BeImmutable();
        }

        [Test]
        public void Command_ShouldHaveNameEndingWith_Command()
        {
            var commands = _applicationAssembly.GetTypes()
                .Where(p => !p.Name.EndsWith("InternalCommand"))
                .Where(p => typeof(ICommand).IsAssignableFrom(p));

            foreach (var command in commands)
                command.Name.Should().EndWith("Command");
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
            var internalCommands = _applicationAssembly.GetTypes()
                .Where(p => p.Name.EndsWith("InternalCommand"));

            foreach (var internalCommand in internalCommands)
                internalCommand.Name.Should().EndWith("InternalCommand");
        }

        [Test]
        public void InternalCommand_EachClassShouldNotBePublic()
        {
            var internalCommands = _applicationAssembly.GetTypes()
                .Where(p => p.Name.EndsWith("InternalCommand"));

            foreach (var internalCommand in internalCommands)
                internalCommand.IsPublic.Should().BeFalse();
        }

        [Test]
        public void Query_ShouldHaveNameEndingWith_Query()
        {
            var queries = _applicationAssembly.GetTypes().Where(p =>
                p.GetInterfaces().Any(a => a.IsGenericType && a.GetGenericTypeDefinition() == typeof(IQuery<>)));

            foreach (var query in queries)
                query.Name.Should().EndWith("Query");
        }

        [Test]
        public void Query_EachClass_ShouldBePublic()
        {
            var queries = _applicationAssembly.GetTypes().Where(p =>
                p.GetInterfaces().Any(a => a.IsGenericType && a.GetGenericTypeDefinition() == typeof(IQuery<>)));

            foreach (var query in queries)
                query.IsPublic.Should().BeTrue();
        }

        [Test]
        public void Dto_EachClass_ShouldBePublic()
        {
            var dtos = _infrastructureAssembly.GetTypes().Where(p => p.Namespace != null)
                .Where(p => p.Namespace.Contains("BookLovers.Librarians.Infrastructure.Dtos"));

            foreach (var dto in dtos)
                dto.IsPublic.Should().BeTrue();
        }

        [Test]
        public void Dto_EachClassShouldHave_DtoPostFix()
        {
            var dtos = _infrastructureAssembly.GetTypes()
                .Where(p => p.Namespace != null)
                .Where(p => p.Namespace.Contains("BookLovers.Librarians.Infrastructure.Dtos"));

            foreach (var dto in dtos)
                dto.Name.Should().EndWith("Dto");
        }

        [Test]
        public void Dto_EachClassShouldNot_BeImmutable()
        {
            var dtos = _infrastructureAssembly.GetTypes().Where(p => p.Namespace != null)
                .Where(p => p.Namespace.Contains("BookLovers.Librarians.Infrastructure.Dtos"));

            foreach (var dto in dtos)
                dto.Should().NotBeImmutable();
        }

        [Test]
        public void WriteModel_EachClass_ShouldBePublic()
        {
            var writeModels = _applicationAssembly.GetTypes().Where(p => p.Namespace != null)
                .Where(p => p.IsClass && p.Namespace == "BookLovers.Librarians.Application.WriteModels");

            foreach (var writeModel in writeModels)
                writeModel.IsPublic.Should().BeTrue();
        }

        [Test]
        public void WriteModel_EachClassNameShouldEndWith_WriteModelPostFix()
        {
            var writeModels = _applicationAssembly.GetTypes().Where(p => p.Namespace != null)
                .Where(p => p.IsClass && p.Namespace == "BookLovers.Librarians.Application.WriteModels");

            foreach (var writeModel in writeModels)
                writeModel.Name.Should().EndWith("WriteModel");
        }

        [Test]
        public void CommandHandler_ShouldHaveNameEndingWith_Handler()
        {
            var commandHandlers = _applicationAssembly.GetTypes().Where(p =>
                p.GetInterfaces().Any(a =>
                    a.IsGenericType && a.GetGenericTypeDefinition() == typeof(ICommandHandler<>)));

            foreach (var commandHandler in commandHandlers)
                commandHandler.Name.Should().EndWith("Handler");
        }

        [Test]
        public void CommandHandler_EachClass_ShouldNotBePublic()
        {
            var commandHandlers = _applicationAssembly.GetTypes().Where(p =>
                p.GetInterfaces().Any(a =>
                    a.IsGenericType && a.GetGenericTypeDefinition() == typeof(ICommandHandler<>)));

            foreach (var commandHandler in commandHandlers)
                commandHandler.IsPublic.Should().BeFalse();
        }

        [Test]
        public void QueryHandler_EachClass_ShouldBeNonPublic()
        {
            var queryHandlers = _infrastructureAssembly.GetTypes().Where(p =>
                p.GetInterfaces().Any(a =>
                    a.IsGenericType && a.GetGenericTypeDefinition() == typeof(IQueryHandler<,>)));

            foreach (var queryHandler in queryHandlers)
                queryHandler.IsPublic.Should().BeFalse();
        }

        [Test]
        public void QueryHandler_EachClassNameShouldEndWith_HandlerPostFix()
        {
            var queryHandlers = _infrastructureAssembly.GetTypes().Where(p =>
                p.GetInterfaces().Any(a =>
                    a.IsGenericType && a.GetGenericTypeDefinition() == typeof(IQueryHandler<,>)));

            foreach (var queryHandler in queryHandlers)
                queryHandler.Name.Should().EndWith("Handler");
        }

        [Test]
        public void IntegrationHandler_ShouldHaveNameEndingWith_Handler()
        {
            var handlers = _applicationAssembly.GetTypes()
                .Where(p => p.Namespace == "BookLovers.Librarians.Application.Integration").Where(p =>
                    p.GetInterfaces().Any(a =>
                        a.IsGenericType && a.GetGenericTypeDefinition() == typeof(IIntegrationEventHandler<>)));

            foreach (var handler in handlers)
                handler.Name.Should().EndWith("Handler");
        }

        [Test]
        public void IntegrationHandler_EachClass_ShouldNotBePublic()
        {
            var handlers = _applicationAssembly.GetTypes()
                .Where(p => p.Namespace == "BookLovers.Librarians.Application.Integration").Where(p =>
                    p.GetInterfaces().Any(a =>
                        a.IsGenericType && a.GetGenericTypeDefinition() == typeof(IIntegrationEventHandler<>)));

            foreach (var handler in handlers)
                handler.IsPublic.Should().BeFalse();
        }

        [Test]
        public void Validator_ShouldHaveNameEndingWith_Validator()
        {
            var validators = _infrastructureAssembly.GetTypes()
                .Where(p => p.BaseType != (Type) null)
                .Where(p => p.BaseType.GenericTypeArguments.Length == 1).Where(p =>
                    p.IsSubclassOf(
                        typeof(AbstractValidator<>).MakeGenericType(p.BaseType.GenericTypeArguments.First())));

            foreach (var validator in validators)
                validator.Name.Should().EndWith("Validator");
        }

        [Test]
        public void Validator_EachClass_ShouldNotBePublic()
        {
            var validators = _infrastructureAssembly.GetTypes().Where(p => p.BaseType != null)
                .Where(p => p.BaseType.GenericTypeArguments.Length == 1)
                .Where(p =>
                    p.IsSubclassOf(
                        typeof(AbstractValidator<>).MakeGenericType(p.BaseType.GenericTypeArguments.First())));

            foreach (var validator in validators)
                validator.IsPublic.Should().BeFalse();
        }

        [Test]
        public void BusinessRule_Should_NotBePublic()
        {
            var rules = _domainAssembly.GetTypes()
                .Where(p => p.IsSubclassOf(typeof(BaseBusinessRule)))
                .Where(p => typeof(IBusinessRule).IsAssignableFrom(p));

            foreach (var rule in rules)
                rule.IsPublic.Should().BeFalse();
        }

        [Test]
        public void BusinessRule_AllShouldImplement_IBusinessRuleInterface()
        {
            var rules = _domainAssembly.GetTypes()
                .Where(p => typeof(IBusinessRule).IsAssignableFrom(p));

            foreach (var subject in rules)
                subject.Should().Implement<IBusinessRule>();
        }

        [Test]
        public void AggregateRoot_Should_BeImmutable()
        {
            var roots = _domainAssembly.GetTypes()
                .Where(p => p.IsSubclassOf(typeof(AggregateRoot))).ToList();

            foreach (var root in roots)
                root.Should().BeImmutable();
        }

        [Test]
        public void AggregateRoot_ShouldNotHave_PublicParameterlessConstructor()
        {
            var roots = _domainAssembly.GetTypes()
                .Where(p => p.IsSubclassOf(typeof(AggregateRoot))).ToList();

            foreach (var root in roots)
                root.Should().NotHavePublicParameterlessConstructor();
        }

        [Test]
        public void Entity_ShouldNotHaveReferenceTo_OtherOrOwnAggregateRoot()
        {
            var entities = _domainAssembly.GetTypes()
                .Where(p => p.IsSubclassOf(typeof(IEntityObject))).ToList();

            foreach (var entity in entities)
                entity.Should().NotHaveReferenceTo(typeof(AggregateRoot));
        }

        [Test]
        public void Entity_ShouldHave_PrivateParameterlessConstructor()
        {
            var entities = _domainAssembly.GetTypes()
                .Where(p => p.IsSubclassOf(typeof(IEntityObject))).ToList();

            foreach (var entity in entities)
                entity.Should().HaveNonPublicParameterlessConstructor();
        }

        [Test]
        public void ValueObject_ShouldBe_Immutable()
        {
            var valueObjects = _domainAssembly.GetTypes()
                .Where(p => p.BaseType != null)
                .Where(p => p.BaseType.GenericTypeArguments.Length > 0).Where(p =>
                    p.IsSubclassOf(
                        typeof(BookLovers.Base.Domain.ValueObject.ValueObject<>)
                            .MakeGenericType(
                                p.BaseType.GenericTypeArguments.First()))).ToList();

            foreach (var valueObject in valueObjects)
                valueObject.Should().BeImmutable();
        }

        [Test]
        public void ValueObject_ShouldHave_PrivateParameterlessConstructor()
        {
            var valueObjects = _domainAssembly.GetTypes()
                .Where(p => p.BaseType != null)
                .Where(p => p.BaseType.GenericTypeArguments.Length > 0).Where(p =>
                    p.IsSubclassOf(
                        typeof(BookLovers.Base.Domain.ValueObject.ValueObject<>).MakeGenericType(
                            p.BaseType.GenericTypeArguments.First()))).ToList();

            foreach (var valueObject in valueObjects)
                valueObject.Should().HaveNonPublicParameterlessConstructor();
        }

        [Test]
        public void DomainEvent_ShouldBe_Immutable()
        {
            var @events = _domainAssembly.GetTypes().Where(p =>
                typeof(IEvent).IsAssignableFrom(p) || typeof(IStateEvent).IsAssignableFrom(p));

            foreach (var subject in @events)
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
            _applicationAssembly.GetReferencedAssemblies().Should()
                .NotContain(p => p.FullName == _infrastructureAssembly.FullName);
        }

        [SetUp]
        public void SetUp()
        {
            _domainAssembly = Assembly.Load("BookLovers.Librarians");
            _applicationAssembly = Assembly.Load("BookLovers.Librarians.Application");
            _infrastructureAssembly = Assembly.Load("BookLovers.Librarians.Infrastructure");
        }
    }
}