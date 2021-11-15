using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture;
using BaseTests;
using BaseTests.DataCreationHelpers;
using BaseTests.EndToEndHelpers;
using BookLovers.Base.Infrastructure.Extensions;
using BookLovers.Base.Infrastructure.Services;
using BookLovers.Publication.Application.Commands.Authors;
using BookLovers.Publication.Application.WriteModels.Author;
using BookLovers.Publication.Domain.BookReaders;
using BookLovers.Publication.Infrastructure.Dtos.Publications;
using BookLovers.Publication.Infrastructure.Persistence;
using BookLovers.Publication.Infrastructure.Queries.Authors;
using BookLovers.Publication.Infrastructure.Root;
using BookLovers.Publication.Store.Persistence;
using BookLovers.Shared.Categories;
using BookLovers.Shared.SharedSexes;
using FluentAssertions;
using Moq;
using Ninject;
using NUnit.Framework;

namespace BookLovers.Publication.Tests.IntegrationTests
{
    public class CreateAuthorTests : IntegrationTest<PublicationModule, CreateAuthorCommand>
    {
        private Mock<IHttpContextAccessor> _mock = new Mock<IHttpContextAccessor>();
        private Guid _readerGuid;
        private CreateAuthorWriteModel _writeModel;

        [Test]
        public async Task CreateAuthor_WhenCalled_ShouldCreateAuthor()
        {
            var validationResult = await Module.SendCommandAsync(Command);
            validationResult.HasErrors.Should().BeFalse();

            var queryResult =
                await Module.ExecuteQueryAsync<AuthorByGuidQuery, AuthorDto>(
                    new AuthorByGuidQuery(_writeModel.AuthorWriteModel.AuthorGuid));

            queryResult.Value.Should().NotBeNull();
        }

        [Test]
        public async Task CreateAuthor_WhenCalledAndCommandIsNull_ShouldReturnValidationResultWithErrors()
        {
            Command = null;

            var validationResult = await Module.SendCommandAsync(Command);
            validationResult.HasErrors.Should().BeTrue();
        }

        [Test]
        public async Task CreateAuthor_WhenCalledAndCommandHasInvalidData_ShouldReturnValidationResultWithErrors()
        {
            _writeModel.AuthorWriteModel.ReaderGuid = Guid.Empty;
            _writeModel.AuthorWriteModel.AuthorGuid = Guid.Empty;
            _writeModel.AuthorWriteModel.Basics.SecondName = string.Empty;

            Command = new CreateAuthorCommand(_writeModel);

            var validationResult = await this.Module.SendCommandAsync(Command);
            validationResult.HasErrors.Should().BeTrue();
            validationResult.Errors.Should().Contain(p => p.ErrorProperty == "ReaderGuid");
            validationResult.Errors.Should().Contain(p => p.ErrorProperty == "AuthorGuid");
            validationResult.Errors.Should().Contain(p => p.ErrorProperty == "SecondName");
        }

        protected override void InitializeRoot()
        {
            _readerGuid = Fixture.Create<Guid>();
            _mock.Setup(s => s.UserGuid).Returns(_readerGuid);

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

            PublicationModuleStartup.Initialize(_mock.Object, appManagerMock.Object, new FakeLogger().GetLogger());
        }

        protected override Task ClearStore()
        {
            this.CompositionRoot.Kernel.Get<PublicationsStoreContext>().ClearPublicationsStore();

            return Task.CompletedTask;
        }

        protected override Task ClearReadContext()
        {
            this.CompositionRoot.Kernel.Get<PublicationsContext>().CleanPublicationsContext();

            return Task.CompletedTask;
        }

        protected override Task PrepareData()
        {
            UnitOfWork.CommitAsync(new BookReader(Fixture.Create<Guid>(), _readerGuid, Fixture.Create<int>()));

            var dto = Fixture
                .Build<AuthorWriteModel>()
                .With(p => p.AuthorGuid)
                .WithBasics(Sex.Female)
                .WithBooks(new List<Guid>())
                .WithDescription(Fixture.Create<string>(), Fixture.Create<string>(),
                    Fixture.Create<string>())
                .WithDetails(Fixture.Create<DateTime>(), Fixture.Create<string>(),
                    Fixture.Create<DateTime>())
                .WithGenres(new List<int> { SubCategory.FictionSubCategory.Fantasy.Value })
                .With(p => p.ReaderGuid, _readerGuid)
                .Create<AuthorWriteModel>();

            _writeModel = new CreateAuthorWriteModel
            {
                AuthorWriteModel = dto,
                PictureWriteModel = new AuthorPictureWriteModel()
                {
                    AuthorImage = string.Empty,
                    FileName = string.Empty
                }
            };

            Command = new CreateAuthorCommand(_writeModel);

            return Task.CompletedTask;
        }

        protected override void SetCompositionRoot()
        {
            this.CompositionRoot = new CompositionRoot();
        }
    }
}