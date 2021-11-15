using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using BaseTests;
using BaseTests.DataCreationHelpers;
using BaseTests.EndToEndHelpers;
using BaseTests.EndToEndHelpers.Mocks;
using BookLovers.Base.Domain.Rules;
using BookLovers.Base.Infrastructure.Extensions;
using BookLovers.Base.Infrastructure.Services;
using BookLovers.Publication.Application.Commands.Authors;
using BookLovers.Publication.Application.WriteModels.Author;
using BookLovers.Publication.Application.WriteModels.Books;
using BookLovers.Publication.Domain.Authors;
using BookLovers.Publication.Domain.Authors.Services;
using BookLovers.Publication.Domain.BookReaders;
using BookLovers.Publication.Events.Authors;
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

namespace BookLovers.Publication.Tests.IntegrationTests.EditAuthor
{
    public class EditAuthorTests : IntegrationTest<PublicationModule, EditAuthorCommand>
    {
        private Guid _readerGuid;
        private AuthorWriteModel _dto;
        private Author _aggregate;

        [Test]
        public async Task EditAuthor_WhenCalled_ShouldEditAuthor()
        {
            var validationResult = await Module.SendCommandAsync(Command);

            validationResult.HasErrors.Should().BeFalse();

            var queryResult =
                await Module.ExecuteQueryAsync<AuthorByGuidQuery, AuthorDto>(
                    new AuthorByGuidQuery(_dto.AuthorGuid));

            queryResult.Value.Guid.Should().Be(_dto.AuthorGuid);
            queryResult.Value.AboutAuthor.Should().Be(_dto.Description.AboutAuthor);
            queryResult.Value.BirthPlace.Should().Be(_dto.Details.BirthPlace);
            queryResult.Value.DescriptionSource.Should().Be(_dto.Description.DescriptionSource);
            queryResult.Value.AuthorWebSite.Should().Be(_dto.Description.WebSite);
            queryResult.Value.FirstName.Should().Be(_dto.Basics.FirstName);
            queryResult.Value.SecondName.Should().Be(_dto.Basics.SecondName);
            queryResult.Value.Sex.Should().Be(_dto.Basics.Sex);
        }

        [Test]
        public async Task EditAuthor_WhenCalledAndCommandIsNotValid_ShouldReturnFailureResult()
        {
            Command = null;

            var validationResult = await Module.SendCommandAsync(Command);

            validationResult.HasErrors.Should().BeTrue();
            validationResult.Errors.First().ErrorMessage.Should().Be("Command cannot be null.");
        }

        [Test]
        public async Task EditAuthor_WhenCalledAndCommandHasInvalidData_ShouldReturnFailureResult()
        {
            Command = new EditAuthorCommand(new EditAuthorWriteModel
            {
                AuthorWriteModel = new AuthorWriteModel()
                {
                    AuthorGuid = Guid.Empty
                }
            });

            var validationResult = await Module.SendCommandAsync(Command);
            validationResult.HasErrors.Should().BeTrue();
            validationResult.Errors.Should().Contain(p => p.ErrorProperty == "AuthorGuid");
        }

        [Test]
        public async Task EditAuthor_WhenCalledAndAuthorIsNotActive_ShouldThrowBusinessRuleNotMeetException()
        {
            _aggregate.ApplyChange(new AuthorArchived(
                _aggregate.Guid,
                _aggregate.Books.Select(s => s.BookGuid).ToList(),
                _aggregate.AuthorQuotes.Select(s => s.QuoteGuid)));

            await UnitOfWork.CommitAsync(_aggregate);

            Func<Task> act = async () => await Module.SendCommandAsync(Command);

            act.Should().Throw<BusinessRuleNotMetException>();
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

            var authorFactory = CompositionRoot.Kernel.Get<AuthorFactory>();

            var factoryData = this.CreateAuthorFactoryData();

            _aggregate = authorFactory.CreateAuthor(factoryData);

            await UnitOfWork.CommitAsync(_aggregate);

            _dto = Fixture
                .Build<AuthorWriteModel>()
                .With(p => p.AuthorGuid, _aggregate.Guid)
                .WithBasics(Sex.Female)
                .WithBooks(new List<Guid>()).WithDescription(Fixture.Create<string>(), Fixture.Create<string>(),
                    Fixture.Create<string>()).WithDetails(Fixture.Create<DateTime>(), Fixture.Create<string>(),
                    Fixture.Create<DateTime>())
                .WithGenres(new List<int> { SubCategory.FictionSubCategory.Fantasy.Value })
                .Create<AuthorWriteModel>();

            Command = new EditAuthorCommand(new EditAuthorWriteModel
            {
                AuthorWriteModel = _dto,
                PictureWriteModel = new AuthorPictureWriteModel
                {
                    AuthorImage = string.Empty,
                    FileName = string.Empty
                }
            });
        }

        protected override void SetCompositionRoot()
        {
            this.CompositionRoot = new CompositionRoot();
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