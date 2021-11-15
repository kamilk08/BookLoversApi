using System;
using System.Linq;
using System.Reflection;
using BaseTests;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Base.Domain.Entity;
using BookLovers.Base.Domain.Events;
using BookLovers.Base.Domain.Rules;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;
using BookLovers.Base.Infrastructure.Queries;
using FluentAssertions;
using FluentValidation;
using NUnit.Framework;

namespace BookLovers.Bookcases.Tests.ArchitectureTests
{
    [TestFixture]
    public class ArchitectureTests
    {
        private Assembly _domainAssembly;
        private Assembly _applicationAssembly;
        private Assembly _infrastructureAssembly;

        [Test]
        public void Command_ShouldBe_Immutable()
        {
            var commands = _applicationAssembly.GetTypes()
                .Where(p => !p.Name.EndsWith("InternalCommand"))
                .Where(p => typeof(ICommand).IsAssignableFrom(p));

            foreach (var command in commands)
                command.Should().BeImmutable();
        }

        [Test]
        public void Command_EachClass_ShouldBePublic()
        {
            var commands = _applicationAssembly.GetTypes()
                .Where(p => !p.Name.EndsWith("InternalCommand"))
                .Where(p => typeof(ICommand).IsAssignableFrom(p));

            foreach (var command in commands)
                command.IsPublic.Should().BeTrue();
        }

        [Test]
        public void Command_EachClassNameShouldEndWith_CommandPostFix()
        {
            var commands = _applicationAssembly.GetTypes()
                .Where(p => !p.Name.EndsWith("InternalCommand"))
                .Where(p => typeof(ICommand).IsAssignableFrom(p));

            foreach (var command in commands)
                command.Name.Should().EndWith("Command");
        }

        [Test]
        public void InternalCommand_EachClassNameShouldEndWith_InternalCommandPostFix()
        {
            var internalCommands = _applicationAssembly.GetTypes().Where(
                p => p.Name.EndsWith("InternalCommand"));

            foreach (var command in internalCommands)
                command.Name.Should().EndWith("InternalCommand");
        }

        [Test]
        public void InternalCommand_EachClassShouldNotBePublic()
        {
            var internalCommands = _applicationAssembly.GetTypes().Where(
                p => p.Name.EndsWith("InternalCommand"));

            foreach (var internalCommand in internalCommands)
                internalCommand.IsPublic.Should().BeFalse();
        }

        [Test]
        public void Query_EachClass_ShouldBePublic()
        {
            var queries = _infrastructureAssembly.GetTypes().Where(
                p =>
                    p.GetInterfaces().Any(a =>
                        a.IsGenericType && a.GetGenericTypeDefinition() == typeof(IQuery<>)));

            foreach (var query in queries)
                query.IsPublic.Should().BeTrue();
        }

        [Test]
        public void Query_EachClassNameShouldEndWith_QueryPostFix()
        {
            var queries = _infrastructureAssembly.GetTypes().Where(
                p => p.GetInterfaces().Any(a =>
                    a.IsGenericType && a.GetGenericTypeDefinition() == typeof(IQuery<>)));

            foreach (var query in queries)
                query.Name.Should().EndWith("Query");
        }

        [Test]
        public void Dto_EachClassNameShouldEndWith_DtoPostFix()
        {
            var dtos = _infrastructureAssembly.GetTypes()
                .Where(p => p.Namespace != null).Where(
                    p => p.IsClass && p.Namespace == "BookLovers.Bookcases.Infrastructure.Dtos");

            foreach (var dto in dtos)
                dto.Name.Should().EndWith("Dto");
        }

        [Test]
        public void Dto_EachClass_ShouldBePublic()
        {
            var dtos = _infrastructureAssembly.GetTypes()
                .Where(p => p.Namespace != null).Where(
                    p => p.IsClass && p.Namespace == "BookLovers.Bookcases.Infrastructure.Dtos");

            foreach (var dto in dtos)
                dto.IsPublic.Should().BeTrue();
        }

        [Test]
        public void WriteModel_EachClass_ShouldBePublic()
        {
            var writeModels = _applicationAssembly.GetTypes()
                .Where(p => p.Namespace != null).Where(p =>
                    p.IsClass && p.Namespace == "BookLovers.Bookcases.Application.WriteModels");

            foreach (var writeModel in writeModels)
                writeModel.IsPublic.Should().BeTrue();
        }

        [Test]
        public void WriteModel_EachClassNameShouldEndWith_WriteModelPostFix()
        {
            var writeModels = _applicationAssembly.GetTypes()
                .Where(p => p.Namespace != null).Where(p =>
                    p.IsClass && p.Namespace == "BookLovers.Bookcases.Application.WriteModels");

            foreach (var memberInfo in writeModels)
                memberInfo.Name.Should().EndWith("WriteModel");
        }

        [Test]
        public void CommandHandler_EachClass_ShouldNotBePublic()
        {
            var commandHandlers = _applicationAssembly.GetTypes().Where(
                p => p.GetInterfaces().Any(a =>
                    a.IsGenericType && a.GetGenericTypeDefinition() == typeof(ICommandHandler<>)));

            foreach (var commandHandler in commandHandlers)
                commandHandler.IsPublic.Should().BeFalse();
        }

        [Test]
        public void CommandHandler_EachClassNameShouldEndWith_HandlerPostFix()
        {
            var commandHandlers = _applicationAssembly.GetTypes().Where(
                p => p.GetInterfaces().Any(a =>
                    a.IsGenericType && a.GetGenericTypeDefinition() == typeof(ICommandHandler<>)));

            foreach (var commandHandler in commandHandlers)
                commandHandler.Name.Should().EndWith("Handler");
        }

        [Test]
        public void QueryHandler_EachClass_ShouldBeNonPublic()
        {
            var queryHandlers = _infrastructureAssembly.GetTypes().Where(
                p => p.GetInterfaces().Any(a =>
                    a.IsGenericType && a.GetGenericTypeDefinition() == typeof(IQueryHandler<,>)));

            foreach (var queryHandler in queryHandlers)
                queryHandler.IsPublic.Should().BeFalse();
        }

        [Test]
        public void QueryHandler_EachClassNameShouldEndWith_HandlerPostFix()
        {
            var queryHandlers = _infrastructureAssembly.GetTypes().Where(
                p => p.GetInterfaces().Any(a =>
                    a.IsGenericType && a.GetGenericTypeDefinition() == typeof(IQueryHandler<,>)));

            foreach (var memberInfo in queryHandlers)
                memberInfo.Name.Should().EndWith("Handler");
        }

        [Test]
        public void IntegrationHandler_EachClassShould_NotBePublic()
        {
            var integrationHandlers = _applicationAssembly.GetTypes().Where(
                p => p.GetInterfaces().Any(a =>
                    a.IsGenericType && a.GetGenericTypeDefinition() == typeof(IIntegrationEventHandler<>)));

            foreach (var handler in integrationHandlers)
                handler.IsPublic.Should().BeFalse();
        }

        [Test]
        public void IntegrationHandler_EachClassNameShouldEndWith_HandlerPostFix()
        {
            var integrationHandlers = _applicationAssembly.GetTypes().Where(
                p => p.GetInterfaces().Any(a =>
                    a.IsGenericType && a.GetGenericTypeDefinition() == typeof(IIntegrationEventHandler<>)));

            foreach (var handler in integrationHandlers)
                handler.Name.Should().EndWith("Handler");
        }

        [Test]
        public void DomainEventHandler_EachClassNameShouldEndWith_EventHandlerPostFix()
        {
            var eventHandlers = _applicationAssembly.GetTypes().Where(
                p => p.GetInterfaces().Any(a =>
                    a.IsGenericType && a.GetGenericTypeDefinition() == typeof(IDomainEventHandler<>)));

            foreach (var handler in eventHandlers)
                handler.Name.Should().EndWith("EventHandler");
        }

        [Test]
        public void DomainEventHandler_EachClassShould_NotBePublic()
        {
            var eventHandlers = _applicationAssembly.GetTypes().Where(
                p => p.GetInterfaces().Any(a =>
                    a.IsGenericType && a.GetGenericTypeDefinition() == typeof(IDomainEventHandler<>)));

            foreach (var eventHandler in eventHandlers)
                eventHandler.IsPublic.Should().BeFalse();
        }

        [Test]
        public void Validator_EachClassNameShouldEndWith_ValidatorPostFix()
        {
            var validators = _infrastructureAssembly.GetTypes()
                .Where(p => p.BaseType != (Type) null)
                .Where(p => (uint) p.BaseType.GenericTypeArguments.Length > 0).Where(
                    p =>
                        p.IsSubclassOf(typeof(AbstractValidator<>).MakeGenericType(
                            p.BaseType.GenericTypeArguments.First())));

            foreach (var validator in validators)
                validator.Name.Should().EndWith("Validator");
        }

        [Test]
        public void Validator_EachClass_ShouldNotBePublic()
        {
            var validators = _infrastructureAssembly.GetTypes()
                .Where(p => p.BaseType != (Type) null)
                .Where(p => (uint) p.BaseType.GenericTypeArguments.Length > 0U).Where(
                    p =>
                        p.IsSubclassOf(typeof(AbstractValidator<>).MakeGenericType(
                            p.BaseType.GenericTypeArguments.First())));

            foreach (var validator in validators)
                validator.IsPublic.Should().BeFalse();
        }

        [Test]
        public void ProjectionHandler_EachClassShould_NotBePublic()
        {
            var projectionHandlers = _infrastructureAssembly.GetTypes().Where(
                p => p.GetInterfaces().Any(a =>
                    a.IsGenericType && a.GetGenericTypeDefinition() == typeof(IProjectionHandler<>)));

            foreach (var projectionHandler in projectionHandlers)
                projectionHandler.IsPublic.Should().BeFalse();
        }

        [Test]
        public void ProjectionHandler_EachClassNameShouldEndWith_ProjectionPostFix()
        {
            var projectionHandlers = _infrastructureAssembly.GetTypes().Where(
                p => p.GetInterfaces().Any(a =>
                    a.IsGenericType && a.GetGenericTypeDefinition() == typeof(IProjectionHandler<>)));

            foreach (var handler in projectionHandlers)
                handler.Name.Should().EndWith("Projection");
        }

        [Test]
        public void BusinessRule_EachClassShouldNot_BePublic()
        {
            var rules = _domainAssembly.GetTypes().Where(
                p => typeof(IBusinessRule).IsAssignableFrom(p));

            foreach (var type in rules)
                type.IsPublic.Should().BeFalse();
        }

        [Test]
        public void BusinessRule_AllShouldImplement_IBusinessRuleInterface()
        {
            var rules = _domainAssembly.GetTypes().Where(
                p => typeof(IBusinessRule).IsAssignableFrom(p));

            foreach (var rule in rules)
                rule.Should().Implement<IBusinessRule>();
        }

        [Test]
        public void AggregateRoot_EachRoot_ShouldBeImmutable()
        {
            var roots = _domainAssembly.GetTypes().Where(
                p => p.IsSubclassOf(typeof(EventSourcedAggregateRoot)));

            foreach (var root in roots)
                root.Should().BeImmutable();
        }

        [Test]
        public void AggregateRoot_ShouldNotHave_PublicParameterlessConstructor()
        {
            var roots = _domainAssembly.GetTypes().Where(
                p => p.IsSubclassOf(typeof(EventSourcedAggregateRoot)));

            foreach (var subject in roots)
                subject.Should().NotHavePublicParameterlessConstructor();
        }

        [Test]
        public void Entity_ShouldNotHave_ReferenceToOwnOrOtherAggregateRoot()
        {
            var roots = _domainAssembly.GetTypes().Where(
                p => p.IsSubclassOf(typeof(IEntityObject)));

            foreach (var root in roots)
                root.Should().NotHaveReferenceTo(typeof(EventSourcedAggregateRoot));
        }

        [Test]
        public void Entity_ShouldHave_PrivateParameterlessConstructor()
        {
            var entities = _domainAssembly.GetTypes().Where(
                p => p.IsSubclassOf(typeof(IEntityObject)));

            foreach (var entitiy in entities)
                entitiy.Should().HaveNonPublicParameterlessConstructor();
        }

        [Test]
        public void ValueObject_ShouldBe_Immutable()
        {
            var valueObjects = _domainAssembly.GetTypes()
                .Where(p => p.BaseType != null)
                .Where(p => p.BaseType.GenericTypeArguments.Length > 0).Where(
                    p =>
                        p.IsSubclassOf(
                            typeof(BookLovers.Base.Domain.ValueObject.ValueObject<>).MakeGenericType(
                                p.BaseType.GenericTypeArguments.First())));

            foreach (var valueObject in valueObjects)
                valueObject.Should().BeImmutable();
        }

        [Test]
        public void ValueObject_ShouldHave_PrivateParameterlessConstructor()
        {
            var valueObjects = _domainAssembly.GetTypes()
                .Where(p => p.BaseType != null)
                .Where(p => p.BaseType.GenericTypeArguments.Length > 0).Where(
                    p =>
                        p.IsSubclassOf(
                            typeof(BookLovers.Base.Domain.ValueObject.ValueObject<>).MakeGenericType(
                                p.BaseType.GenericTypeArguments.First())));

            foreach (var subject in valueObjects)
                subject.Should().HaveNonPublicParameterlessConstructor();
        }

        [Test]
        public void Event_EachClassShould_BeImmutable()
        {
            var @events = _domainAssembly.GetTypes().Where(
                p =>
                    typeof(IEvent).IsAssignableFrom(p) || typeof(IStateEvent).IsAssignableFrom(p));

            foreach (var @event in @events)
                @event.Should().BeImmutable();
        }

        [Test]
        public void DomainAssembly_ShouldNotHaveDependencyFrom_ApplicationAssembly()
        {
            _domainAssembly.GetReferencedAssemblies().Should()
                .NotContain(p => p.FullName == _applicationAssembly.FullName);
        }

        [Test]
        public void DomainAssembly_ShouldNotHaveDependencyFrom_InfrastructureAssembly()
        {
            _domainAssembly.GetReferencedAssemblies().Should()
                .NotContain(
                    p => p.FullName == _infrastructureAssembly.FullName);
        }

        [Test]
        public void ApplicationAssembly_ShouldNotHaveDependencyFrom_InfrastructureAssembly()
        {
            _applicationAssembly.GetReferencedAssemblies().Should()
                .NotContain(
                    p => p.FullName == _infrastructureAssembly.FullName);
        }

        [SetUp]
        public void SetUp()
        {
            _domainAssembly = Assembly.Load("BookLovers.Bookcases");
            _applicationAssembly = Assembly.Load("BookLovers.Bookcases.Application");
            _infrastructureAssembly = Assembly.Load("BookLovers.Bookcases.Infrastructure");
        }
    }
}