using System;
using System.Linq;
using System.Reflection;
using BaseTests;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Base.Domain.Entity;
using BookLovers.Base.Domain.Rules;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;
using BookLovers.Base.Infrastructure.Queries;
using FluentAssertions;
using FluentValidation;
using NUnit.Framework;

namespace BookLovers.Ratings.Tests.ArchitectureTests
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
            var commands = this._applicationAssembly.GetTypes()
                .Where(p => !p.IsInterface && typeof(ICommand).IsAssignableFrom(p))
                .ToList();

            foreach (var command in commands)
                command.Should().BeImmutable();
        }

        [Test]
        public void Command_ShouldHaveNameEndingWith_Command()
        {
            var commands = this._applicationAssembly.GetTypes().Where(
                p => typeof(ICommand).IsAssignableFrom(p));

            foreach (var command in commands)
                command.Name.Should().EndWith("Command");
        }

        [Test]
        public void Command_EachClass_ShouldBePublic()
        {
            var commands = this._applicationAssembly.GetTypes()
                .Where(p => !p.Name.EndsWith("InternalCommand"))
                .Where(p => typeof(ICommand).IsAssignableFrom(p));

            foreach (var command in commands)
                command.IsPublic.Should().BeTrue();
        }

        [Test]
        public void InternalCommand_EachClassNameShouldEndWith_InternalCommandPostFix()
        {
            var commands = this._applicationAssembly.GetTypes()
                .Where(p => p.Name.EndsWith("InternalCommand"))
                .Where(p => typeof(IInternalCommand).IsAssignableFrom(p));

            foreach (var command in commands)
                command.Name.Should().EndWith("InternalCommand");
        }

        [Test]
        public void InternalCommand_EachClassShouldNotBePublic()
        {
            var commands = this._applicationAssembly.GetTypes()
                .Where(p => typeof(IInternalCommand).IsAssignableFrom(p))
                .Where(p => p.Name.EndsWith("InternalCommand"));

            foreach (var command in commands)
                command.IsPublic.Should().BeFalse();
        }

        [Test]
        public void Query_ShouldHaveNameEndingWith_Query()
        {
            var queries = this._infrastructureAssembly.GetTypes().Where(
                p =>
                    p.GetInterfaces().Any(a =>
                        a.IsGenericType && a.GetGenericTypeDefinition() == typeof(IQuery<>)));

            foreach (var query in queries)
                query.Name.Should().EndWith("Query");
        }

        [Test]
        public void Query_EachClass_ShouldBePublic()
        {
            var queries = this._infrastructureAssembly.GetTypes().Where(
                p =>
                    p.GetInterfaces().Any(a =>
                        a.IsGenericType && a.GetGenericTypeDefinition() == typeof(IQuery<>)));

            foreach (var type in queries)
                type.IsPublic.Should().BeTrue();
        }

        [Test]
        public void Dto_EachClass_ShouldBePublic()
        {
            var dtos = this._infrastructureAssembly.GetTypes()
                .Where(p => p.Namespace != null)
                .Where(p => p.Namespace.Contains("BookLovers.Ratings.Infrastructure.Dtos"));

            foreach (var dto in dtos)
                dto.IsPublic.Should().BeTrue();
        }

        [Test]
        public void Dto_EachClassShouldHave_DtoPostFix()
        {
            var dtos = this._infrastructureAssembly
                .GetTypes()
                .Where(p => p.Namespace != null).Where(
                    p => p.Namespace.Contains("BookLovers.Ratings.Infrastructure.Dtos"));

            foreach (var dto in dtos)
                dto.Name.Should().EndWith("Dto");
        }

        [Test]
        public void Dto_EachClassShould_NotBeImmutable()
        {
            var dtos = this._infrastructureAssembly.GetTypes()
                .Where(p => p.Namespace != null).Where(
                    p => p.Namespace.Contains("BookLovers.Ratings.Infrastructure.Dtos"));

            foreach (var dto in dtos)
                dto.Should().NotBeImmutable();
        }

        [Test]
        public void WriteModel_EachClass_ShouldBePublic()
        {
            var writeModels = this._applicationAssembly.GetTypes()
                .Where(p => p.Namespace != null).Where(p =>
                    p.IsClass && p.Namespace == "BookLovers.Ratings.Application.WriteModels");

            foreach (var writeModel in writeModels)
                writeModel.IsPublic.Should().BeTrue();
        }

        [Test]
        public void WriteModel_EachClassNameShouldEndWith_WriteModelPostFix()
        {
            var writeModels = this._applicationAssembly.GetTypes()
                .Where(p => p.Namespace != null).Where(p =>
                    p.IsClass && p.Namespace == "BookLovers.Ratings.Application.WriteModels");

            foreach (var writeModel in writeModels)
                writeModel.Name.Should().EndWith("WriteModel");
        }

        [Test]
        public void CommandHandler_ShouldHaveNameEndingWith_Handler()
        {
            var commandHandlers = this._applicationAssembly.GetTypes().Where(
                p => p.GetInterfaces().Any(a =>
                    a.IsGenericType && a.GetGenericTypeDefinition() == typeof(ICommandHandler<>)));

            foreach (var commandHandler in commandHandlers)
                commandHandler.Name.Should().EndWith("Handler");
        }

        [Test]
        public void CommandHandler_EachClass_ShouldNotBePublic()
        {
            var commandHandlers = this._applicationAssembly.GetTypes().Where(
                p => p.GetInterfaces().Any(a =>
                    a.IsGenericType && a.GetGenericTypeDefinition() == typeof(ICommandHandler<>)));

            foreach (var commandHandler in commandHandlers)
                commandHandler.IsPublic.Should().BeFalse();
        }

        [Test]
        public void IntegrationHandler_ShouldHaveNameEndingWith_Handler()
        {
            var integrationHandlers = this._applicationAssembly.GetTypes()
                .Where(p => p.Namespace == "BookLovers.Ratings.Application.Integration")
                .Where(p =>
                    p.GetInterfaces().Any(a =>
                        a.IsGenericType && a.GetGenericTypeDefinition() == typeof(IIntegrationEventHandler<>)));

            foreach (var handler in integrationHandlers)
                handler.Name.Should().EndWith("Handler");
        }

        [Test]
        public void IntegrationHandler_EachClass_ShouldNotBePublic()
        {
            var integrationHandlers = this._applicationAssembly.GetTypes().Where(
                p => p.GetInterfaces().Any(a =>
                    a.IsGenericType && a.GetGenericTypeDefinition() == typeof(IIntegrationEventHandler<>)));

            foreach (var handler in integrationHandlers)
                handler.IsPublic.Should().BeFalse();
        }

        [Test]
        public void QueryHandler_EachClass_ShouldBeNonPublic()
        {
            var queryHandlers = this._infrastructureAssembly.GetTypes().Where(
                p => p.GetInterfaces().Any(a =>
                    a.IsGenericType && a.GetGenericTypeDefinition() == typeof(IQueryHandler<,>)));

            foreach (var queryHandler in queryHandlers)
                queryHandler.IsPublic.Should().BeFalse();
        }

        [Test]
        public void QueryHandler_EachClassNameShouldEndWith_HandlerPostFix()
        {
            var queryHandlers = this._infrastructureAssembly.GetTypes().Where(
                p => p.GetInterfaces().Any(a =>
                    a.IsGenericType && a.GetGenericTypeDefinition() == typeof(IQueryHandler<,>)));

            foreach (var handler in queryHandlers)
                handler.Name.Should().EndWith("Handler");
        }

        [Test]
        public void Validator_ShouldHaveNameEndingWith_Validator()
        {
            var validators = this._infrastructureAssembly.GetTypes()
                .Where(p => p.BaseType != (Type) null)
                .Where(p => p.BaseType.GenericTypeArguments.Length == 1).Where(
                    p =>
                        p.IsSubclassOf(typeof(AbstractValidator<>).MakeGenericType(
                            p.BaseType.GenericTypeArguments.First())));

            foreach (var validator in validators)
                validator.Name.Should().EndWith("Validator");
        }

        [Test]
        public void DomainEventHandler_EachClassNameShouldEndWith_EventHandlerPostFix()
        {
            var validators = this._applicationAssembly.GetTypes().Where(
                p => p.GetInterfaces().Any(a =>
                    a.IsGenericType && a.GetGenericTypeDefinition() == typeof(IDomainEventHandler<>)));

            foreach (var validator in validators)
                validator.Name.Should().EndWith("EventHandler");
        }

        [Test]
        public void DomainEventHandler_EachClassShould_NotBePublic()
        {
            var eventHandlers = this._applicationAssembly.GetTypes().Where(
                p => p.GetInterfaces().Any(a =>
                    a.IsGenericType && a.GetGenericTypeDefinition() == typeof(IDomainEventHandler<>)));

            foreach (var handler in eventHandlers)
                handler.IsPublic.Should().BeFalse();
        }

        [Test]
        public void Validator_EachClass_ShouldNotBePublic()
        {
            var validators = this._infrastructureAssembly.GetTypes()
                .Where(p => p.BaseType != null)
                .Where(p => p.BaseType.GenericTypeArguments.Length == 1).Where(
                    p =>
                        p.IsSubclassOf(typeof(AbstractValidator<>).MakeGenericType(
                            p.BaseType.GenericTypeArguments.First())));

            foreach (var validator in validators)
                validator.IsPublic.Should().BeFalse();
        }

        [Test]
        public void BusinessRule_Should_NotBePublic()
        {
            var rules = this._domainAssembly.GetTypes()
                .Where(p => p.IsSubclassOf(typeof(BaseBusinessRule)))
                .Where(p => typeof(IBusinessRule).IsAssignableFrom(p));

            foreach (var rule in rules)
                rule.IsPublic.Should().BeFalse();
        }

        [Test]
        public void AggregateRoot_Should_BeImmutable()
        {
            var roots = this._domainAssembly.GetTypes()
                .Where(p => p.IsSubclassOf(typeof(AggregateRoot))).ToList();

            foreach (var root in roots)
                root.Should().BeImmutable();
        }

        [Test]
        public void AggregateRoot_ShouldNotHave_PublicParameterlessConstructor()
        {
            var roots = this._domainAssembly.GetTypes()
                .Where(p => p.IsSubclassOf(typeof(AggregateRoot))).ToList();

            foreach (var root in roots)
                root.Should().NotHavePublicParameterlessConstructor();
        }

        [Test]
        public void Entity_ShouldNotHaveReferenceTo_OtherOrOwnAggregateRoot()
        {
            var entities = this._domainAssembly.GetTypes()
                .Where(p => p.IsSubclassOf(typeof(IEntityObject))).ToList();

            foreach (var entitiy in entities)
                entitiy.Should().NotHaveReferenceTo(typeof(AggregateRoot));
        }

        [Test]
        public void Entity_ShouldHave_PrivateParameterlessConstructor()
        {
            var entities = this._domainAssembly.GetTypes()
                .Where(p => p.IsSubclassOf(typeof(IEntityObject))).ToList();

            foreach (var entitiy in entities)
                entitiy.Should().HaveNonPublicParameterlessConstructor();
        }

        [Test]
        public void ValueObject_ShouldBe_Immutable()
        {
            var valueObjects = this._domainAssembly.GetTypes()
                .Where(p => p.BaseType != null)
                .Where(p => p.BaseType.GenericTypeArguments.Length > 0).Where(
                    p =>
                        p.IsSubclassOf(
                            typeof(BookLovers.Base.Domain.ValueObject.ValueObject<>).MakeGenericType(
                                p.BaseType.GenericTypeArguments.First()))).ToList();

            foreach (var subject in valueObjects)
                subject.Should().BeImmutable();
        }

        [Test]
        public void ValueObject_ShouldHave_PrivateParameterlessConstructor()
        {
            var valueObjects = this._domainAssembly.GetTypes()
                .Where(p => p.BaseType != null)
                .Where(p => p.BaseType.GenericTypeArguments.Length > 0).Where(
                    p =>
                        p.IsSubclassOf(
                            typeof(BookLovers.Base.Domain.ValueObject.ValueObject<>).MakeGenericType(
                                p.BaseType.GenericTypeArguments.First()))).ToList();

            foreach (var subject in valueObjects)
                subject.Should().HaveNonPublicParameterlessConstructor();
        }

        [Test]
        public void DomainAssembly_ShouldNot_HaveDependencyFromApplicationAssembly()
        {
            this._domainAssembly.GetReferencedAssemblies().Should()
                .NotContain(p => p.FullName == this._applicationAssembly.FullName);
        }

        [Test]
        public void DomainAssembly_ShouldNot_HaveDependencyFromInfrastructureAssembly()
        {
            this._domainAssembly.GetReferencedAssemblies().Should()
                .NotContain(
                    p => p.FullName == this._infrastructureAssembly.FullName);
        }

        public void ApplicationAssembly_ShouldNot_HaveDependencyFromInfrastructureAssembly()
        {
            this._applicationAssembly.GetReferencedAssemblies().Should()
                .NotContain(
                    p => p.FullName == this._infrastructureAssembly.FullName);
        }

        [NUnit.Framework.SetUp]
        public void SetUp()
        {
            this._domainAssembly = Assembly.Load("BookLovers.Ratings");
            this._applicationAssembly = Assembly.Load("BookLovers.Ratings.Application");
            this._infrastructureAssembly = Assembly.Load("BookLovers.Ratings.Infrastructure");
        }
    }
}