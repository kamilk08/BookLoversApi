using System.Data.Entity.Core.Objects.DataClasses;
using System.Linq;
using System.Reflection;
using BaseTests;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Base.Domain.Events;
using BookLovers.Base.Domain.Rules;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;
using BookLovers.Base.Infrastructure.Queries;
using FluentAssertions;
using FluentValidation;
using NUnit.Framework;

namespace BookLovers.Publication.Tests.ArchitectureTests
{
    [TestFixture]
    public class ArchitectureTests
    {
        private Assembly _domainAssembly;
        private Assembly _applicationAssembly;
        private Assembly _infrastructureAssembly;

        [Test]
        public void Command_EachClassShould_BeImmutable()
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

            foreach (var memberInfo in commands)
                memberInfo.Name.Should().EndWith("Command");
        }

        [Test]
        public void InternalCommand_EachClassNameShouldEndWith_InternalCommandPostFix()
        {
            var internalCommands = _applicationAssembly.GetTypes().Where(
                p => p.Name.EndsWith("InternalCommand"));

            foreach (var internalCommand in internalCommands)
                internalCommand.Name.Should().EndWith("InternalCommand");
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
        public void Query_Should_BeImmutable()
        {
            var queries = _applicationAssembly.GetTypes().Where(
                p =>
                    p.GetInterfaces().Any(a =>
                        a.IsGenericType && a.GetGenericTypeDefinition() == typeof(IQuery<>)));

            foreach (var query in queries)
                query.Should().BeImmutable();
        }

        [Test]
        public void Query_EachClass_ShouldBePublic()
        {
            var queries = _applicationAssembly.GetTypes().Where(
                p =>
                    p.GetInterfaces().Any(a =>
                        a.IsGenericType && a.GetGenericTypeDefinition() == typeof(IQuery<>)));

            foreach (var type in queries)
                type.IsPublic.Should().BeTrue();
        }

        [Test]
        public void Query_EachClassNameShouldEndWith_QueryPostFix()
        {
            var queries = _applicationAssembly.GetTypes().Where(
                p =>
                    p.GetInterfaces().Any(a =>
                        a.IsGenericType && a.GetGenericTypeDefinition() == typeof(IQuery<>)));

            foreach (var query in queries)
                query.Name.Should().EndWith("Query");
        }

        [Test]
        public void Dto_ShouldNot_BeImmutable()
        {
            var dtos = _infrastructureAssembly.GetTypes()
                .Where(p => p.Namespace != null).Where(
                    p => p.Namespace.Contains("BookLovers.Readers.Infrastructure.Dtos"));

            foreach (var dto in dtos)
                dto.Should().NotBeImmutable();
        }

        [Test]
        public void Dto_EachClass_ShouldBePublic()
        {
            var dtos = _infrastructureAssembly.GetTypes()
                .Where(p => p.Namespace != null).Where(
                    p => p.Namespace.Contains("BookLovers.Readers.Infrastructure.Dtos"));

            foreach (var dto in dtos)
                dto.IsPublic.Should().BeTrue();
        }

        [Test]
        public void Dto_EachClass_ShouldHaveDtoPostFix()
        {
            var dtos = _infrastructureAssembly.GetTypes()
                .Where(p => p.Namespace != null).Where(
                    p => p.Namespace.Contains("BookLovers.Readers.Infrastructure.Dtos"));

            foreach (var dto in dtos)
                dto.Name.Should().EndWith("Dto");
        }

        [Test]
        public void WriteModel_EachClassShouldHave_WriteModelPostFix()
        {
            var writeModels = _applicationAssembly.GetTypes()
                .Where(p => p.Namespace != null).Where(
                    p => p.Namespace.Contains("BookLovers.Readers.Application.WriteModels"));

            foreach (var writeModel in writeModels)
                writeModel.Name.Should().EndWith("WriteModel");
        }

        [Test]
        public void WriteModel_EachClass_ShouldBePublic()
        {
            var writeModels = _applicationAssembly.GetTypes()
                .Where(p => p.Namespace != null).Where(
                    p => p.Namespace.Contains("BookLovers.Readers.Application.WriteModels"));

            foreach (var writeModel in writeModels)
                writeModel.IsPublic.Should().BeTrue();
        }

        [Test]
        public void WriteModel_EachClass_ShouldNotBeImmutable()
        {
            var writeModels = _applicationAssembly.GetTypes()
                .Where(p => p.Namespace != null).Where(
                    p => p.Namespace.Contains("BookLovers.Readers.Application.WriteModels"));

            foreach (var writeModel in writeModels)
                writeModel.Should().NotBeImmutable();
        }

        [Test]
        public void CommandHandler_EachClassShould_NotBePublic()
        {
            var writeModels = _applicationAssembly.GetTypes().Where(
                p => p.GetInterfaces().Any(a =>
                    a.IsGenericType && a.GetGenericTypeDefinition() == typeof(ICommandHandler<>)));

            foreach (var writeModel in writeModels)
                writeModel.IsPublic.Should().BeFalse();
        }

        [Test]
        public void CommandHandler_EachClassShouldEndWith_HandlerPostFix()
        {
            var commandHandlers = _applicationAssembly.GetTypes().Where(
                p => p.GetInterfaces().Any(a =>
                    a.IsGenericType && a.GetGenericTypeDefinition() == typeof(ICommandHandler<>)));

            foreach (var commandHandler in commandHandlers)
                commandHandler.Name.Should().EndWith("Handler");
        }

        [Test]
        public void QueryHandler_EachClassShouldEndWith_HandlerPostFix()
        {
            var queryHandlers = _infrastructureAssembly.GetTypes().Where(
                p => p.GetInterfaces().Any(a =>
                    a.IsGenericType && a.GetGenericTypeDefinition() == typeof(IQueryHandler<,>)));

            foreach (var handlers in queryHandlers)
                handlers.Name.Should().EndWith("Handler");
        }

        [Test]
        public void QueryHandler_EachClassShould_NotBePublic()
        {
            var queryHandlers = _infrastructureAssembly.GetTypes().Where(
                p => p.GetInterfaces().Any(a =>
                    a.IsGenericType && a.GetGenericTypeDefinition() == typeof(IQueryHandler<,>)));

            foreach (var handler in queryHandlers)
                handler.IsPublic.Should().BeFalse();
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
            var integrationHandlers = _applicationAssembly.GetTypes()
                .Where(p => p.Namespace != null)
                .Where(
                    p => p.Namespace.Contains("BookLovers.Readers.Application.Integration"))
                .Where(p =>
                    p.GetInterfaces().Any(a =>
                        a.IsGenericType && a.GetGenericTypeDefinition() == typeof(IIntegrationEventHandler<>)));

            foreach (var handler in integrationHandlers)
                handler.Name.Should().EndWith("Handler");
        }

        [Test]
        public void DomainEventHandler_EachClassShould_NotBePublic()
        {
            var @eventHandlers = _applicationAssembly.GetTypes().Where(
                p => p.GetInterfaces().Any(a =>
                    a.IsGenericType && a.GetGenericTypeDefinition() == typeof(IDomainEventHandler<>)));

            foreach (var handler in @eventHandlers)
                handler.IsPublic.Should().BeFalse();
        }

        [Test]
        public void DomainEventHandler_EachClassNameShouldEndWith_EventHandlerPostFix()
        {
            var @eventHandlers = _applicationAssembly.GetTypes().Where(
                p => p.GetInterfaces().Any(a =>
                    a.IsGenericType && a.GetGenericTypeDefinition() == typeof(IDomainEventHandler<>)));

            foreach (var handler in @eventHandlers)
                handler.Name.Should().EndWith("EventHandler");
        }

        [Test]
        public void Validator_EachClassShould_NotBePublic()
        {
            var validators = _infrastructureAssembly.GetTypes()
                .Where(p => p.BaseType != null)
                .Where(p => p.BaseType.GenericTypeArguments.Length > 0).Where(
                    p =>
                        p.IsSubclassOf(typeof(AbstractValidator<>).MakeGenericType(
                            p.BaseType.GenericTypeArguments.First())));

            foreach (var validator in validators)
                validator.IsPublic.Should().BeFalse();
        }

        [Test]
        public void Validator_EachClassNameShouldEndWith_ValidatorPostFix()
        {
            var validators = _infrastructureAssembly.GetTypes()
                .Where(p => p.BaseType != null)
                .Where(p => p.BaseType.GenericTypeArguments.Length > 0).Where(
                    p =>
                        p.IsSubclassOf(typeof(AbstractValidator<>).MakeGenericType(
                            p.BaseType.GenericTypeArguments.First())));

            foreach (var validator in validators)
                validator.Name.Should().EndWith("Validator");
        }

        [Test]
        public void ProjectionHandler_EachClassNameShouldEndWith_ProjectionPostFix()
        {
            var projections = _applicationAssembly.GetTypes().Where(
                p => p.GetInterfaces().Any(a =>
                    a.IsGenericType && a.GetGenericTypeDefinition() == typeof(IProjectionHandler<>)));

            foreach (var handler in projections)
                handler.Name.Should().EndWith("Projection");
        }

        [Test]
        public void ProjectionHandler_EachClassShould_NotBePublic()
        {
            var projections = _applicationAssembly.GetTypes().Where(
                p => p.GetInterfaces().Any(a =>
                    a.IsGenericType && a.GetGenericTypeDefinition() == typeof(IProjectionHandler<>)));

            foreach (var projection in projections)
                projection.IsPublic.Should().BeFalse();
        }

        [Test]
        public void BusinessRule_EachClass_ShouldNotBePublic()
        {
            var rules = _domainAssembly.GetTypes().Where(
                p => p.IsSubclassOf(typeof(BaseBusinessRule)));

            foreach (var rule in rules)
                rule.IsPublic.Should().BeFalse();
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
        public void AggregateRoot_ShouldBe_Immutable()
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

            foreach (var root in roots)
                root.Should().NotHavePublicParameterlessConstructor();
        }

        [Test]
        public void Entity_ShouldNot_HaveReferenceToOwnAggregateRootOrAnotherAggregateRoot()
        {
            var entities = _domainAssembly.GetTypes().Where(
                p => p.IsSubclassOf(typeof(EntityObject)));

            foreach (var entity in entities)
                entity.Should().NotHaveReferenceTo(typeof(EventSourcedAggregateRoot));
        }

        [Test]
        public void Entity_Should_HavePrivateParameterlessConstructor()
        {
            var entities = _domainAssembly.GetTypes().Where(
                p => p.IsSubclassOf(typeof(EntityObject)));

            foreach (var entity in entities)
                entity.Should().HaveNonPublicParameterlessConstructor();
        }

        [Test]
        public void ValueObject_Should_BeImmutable()
        {
            var valueObjects = _domainAssembly.GetTypes()
                .Where(p => p.BaseType != null)
                .Where(p => p.BaseType.GenericTypeArguments.Length > 0).Where(
                    p =>
                        p.IsSubclassOf(
                            typeof(BookLovers.Base.Domain.ValueObject.ValueObject<>).MakeGenericType(
                                p.BaseType.GenericTypeArguments.First())));

            foreach (var val in valueObjects)
                val.Should().BeImmutable();
        }

        [Test]
        public void ValueObject_ShouldHave_NonPublicParameterlessConstructor()
        {
            var valueObjects = _domainAssembly.GetTypes()
                .Where(p => p.BaseType != null)
                .Where(p => p.BaseType.GenericTypeArguments.Length > 0).Where(
                    p =>
                        p.IsSubclassOf(
                            typeof(BookLovers.Base.Domain.ValueObject.ValueObject<>).MakeGenericType(
                                p.BaseType.GenericTypeArguments.First())));

            foreach (var val in valueObjects)
                val.Should().HaveNonPublicParameterlessConstructor();
        }

        [Test]
        public void DomainEvent_ShouldBe_Immutable()
        {
            var values = _domainAssembly.GetTypes().Where(
                p =>
                    typeof(IEvent).IsAssignableFrom(p) || typeof(IStateEvent).IsAssignableFrom(p));

            foreach (var value in values)
                value.Should().BeImmutable();
        }

        [Test]
        public void DomainAssembly_ShouldNotHaveDependenciesFrom_ApplicationAssembly()
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

        [SetUp]
        public void SetUp()
        {
            _domainAssembly = Assembly.Load("BookLovers.Publication");
            _applicationAssembly = Assembly.Load("BookLovers.Publication.Application");
            _infrastructureAssembly = Assembly.Load("BookLovers.Publication.Infrastructure");
        }
    }
}