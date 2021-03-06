using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using BaseTests;
using BaseTests.EndToEndHelpers;
using BaseTests.EndToEndHelpers.Mocks;
using BookLovers.Base.Domain.Rules;
using BookLovers.Base.Infrastructure.Extensions;
using BookLovers.Base.Infrastructure.Services;
using BookLovers.Publication.Application.Commands.Authors;
using BookLovers.Publication.Domain.Authors;
using BookLovers.Publication.Domain.Authors.Services;
using BookLovers.Publication.Domain.BookReaders;
using BookLovers.Publication.Infrastructure.Dtos.Publications;
using BookLovers.Publication.Infrastructure.Persistence;
using BookLovers.Publication.Infrastructure.Queries.Authors;
using BookLovers.Publication.Infrastructure.Root;
using BookLovers.Publication.Store.Persistence;
using BookLovers.Shared;
using BookLovers.Shared.Categories;
using BookLovers.Shared.SharedSexes;
using FluentAssertions;
using Moq;
using Ninject;
using NUnit.Framework;

namespace BookLovers.Publication.Tests.IntegrationTests.FollowAuthor
{
    public class FollowAuthorTests : IntegrationTest<PublicationModule, FollowAuthorCommand>
    {
        private Guid _readerGuid;
        private Guid _followerGuid;
        private Author _author;

        [Test]
        public async Task FollowAuthor_WhenCalled_ShouldAddNewFollowerToAuthor()
        {
            var validationResult = await Module.SendCommandAsync(Command);
            validationResult.HasErrors.Should().BeFalse();

            var queryResult = await Module.ExecuteQueryAsync<AuthorByGuidQuery, AuthorDto>(
                new AuthorByGuidQuery(_author.Guid));

            queryResult.Value.AuthorFollowers.Should().HaveCount(1);
        }

        [Test]
        public async Task FollowAuthor_WhenCalledAndCommandIsNotValid_ShouldReturnFailureResult()
        {
            Command = null;

            var validationResult = await Module.SendCommandAsync(Command);
            validationResult.HasErrors.Should().BeTrue();
            validationResult.Errors.First().ErrorMessage.Should().Be("Command cannot be null.");
        }

        [Test]
        public async Task FollowAuthor_WhenCalledAndCommandHasInvalidData_ShouldReturnFailureResult()
        {
            Command = new FollowAuthorCommand(Guid.Empty);

            var validationResult = await Module.SendCommandAsync(Command);
            validationResult.HasErrors.Should().BeTrue();
            validationResult.Errors.Should().Contain(p => p.ErrorProperty == "AuthorGuid");
        }

        [Test]
        public async Task
            FollowAuthor_WhenCalledAndThereIsAlreadySuchAFollower_ShouldThrowBusinessRuleNotMeetException()
        {
            await Module.SendCommandAsync(Command);

            Func<Task> act = async () => await Module.SendCommandAsync(Command);

            act.Should().Throw<BusinessRuleNotMetException>()
                .WithMessage("Author cannot have duplicated followers.");
        }

        protected override void InitializeRoot()
        {
            _readerGuid = Fixture.Create<Guid>();

            var appManagerMock = new Mock<IAppManager>();

            var publicationsConnectionString =
                Environment.GetEnvironmentVariable(PublicationsContext.ConnectionStringKey);
            if (publicationsConnectionString.IsEmpty())
                publicationsConnectionString = E2EConstants.PublicationsConnectionString;

            var publicationsStoreConnectionString =
                Environment.GetEnvironmentVariable(PublicationsStoreContext.ConnectionStringKey);
            if (publicationsStoreConnectionString.IsEmpty())
                publicationsStoreConnectionString = E2EConstants.PublicationsStoreConnectionString;

            appManagerMock.Setup(s => s.GetConfigValue(PublicationsContext.ConnectionStringKey))
                .Returns(publicationsConnectionString);
            appManagerMock.Setup(s => s.GetConfigValue(PublicationsStoreContext.ConnectionStringKey)).Returns(
                publicationsStoreConnectionString);

            PublicationModuleStartup.Initialize(new FakeHttpContextAccessor(_readerGuid, true), appManagerMock.Object,
                new FakeLogger().GetLogger());
        }

        protected override Task ClearStore()
        {
            CompositionRoot.Kernel.Get<PublicationsStoreContext>().ClearPublicationsStore();

            return Task.CompletedTask;
        }

        protected override Task ClearReadContext()
        {
            CompositionRoot.Kernel.Get<PublicationsContext>().CleanPublicationsContext();

            return Task.CompletedTask;
        }

        protected override async Task PrepareData()
        {
            var readerId = Fixture.Create<int>();
            var bookReader = new BookReader(Fixture.Create<Guid>(), _readerGuid, readerId);

            await UnitOfWork.CommitAsync(bookReader);

            _followerGuid = Fixture.Create<Guid>();
            var followerId = Fixture.Create<int>();
            var followerReader = new BookReader(Fixture.Create<Guid>(), _followerGuid, followerId);

            await UnitOfWork.CommitAsync(followerReader);

            var authorName = Fixture.Create<string>();

            var authorFactory = CompositionRoot.Kernel.Get<AuthorFactory>();

            var authorFactoryData = this.CreateAuthorFactoryData();

            _author = authorFactory.CreateAuthor(authorFactoryData);

            await UnitOfWork.CommitAsync(_author);

            Command = new FollowAuthorCommand(_author.Guid);
        }

        protected override void SetCompositionRoot()
        {
            CompositionRoot = new CompositionRoot();
        }

        private AuthorData CreateAuthorFactoryData()
        {
            return AuthorData.Initialize()
                .WithBasics(
                    new FullName(Fixture.Create<string>(), Fixture.Create<string>()),
                    Sex.Male)
                .WithDetails(
                    new LifeLength(Fixture.Create<DateTime>(), Fixture.Create<DateTime>()),
                    Fixture.Create<string>())
                .WithDescription(
                    Fixture.Create<string>(),
                    Fixture.Create<string>(), Fixture.Create<string>())
                .WithGuid(Fixture.Create<Guid>())
                .AddedBy(new BookReader(Fixture.Create<Guid>(), _readerGuid, Fixture.Create<int>()))
                .WithBooks(new List<Guid>())
                .WithGenres(new List<int> { SubCategory.FictionSubCategory.Thriller.Value });
        }
    }
}