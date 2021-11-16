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
using BookLovers.Publication.Application.Commands.Books;
using BookLovers.Publication.Application.WriteModels.Author;
using BookLovers.Publication.Application.WriteModels.Books;
using BookLovers.Publication.Domain.BookReaders;
using BookLovers.Publication.Domain.Books.CoverTypes;
using BookLovers.Publication.Domain.Books.Languages;
using BookLovers.Publication.Domain.Publishers;
using BookLovers.Publication.Infrastructure.Dtos.Publications;
using BookLovers.Publication.Infrastructure.Persistence;
using BookLovers.Publication.Infrastructure.Queries.Books;
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
    public class CreateBookTests : IntegrationTest<PublicationModule, AddBookCommand>
    {
        private Mock<IHttpContextAccessor> _mock = new Mock<IHttpContextAccessor>();
        private Guid _readerGuid;
        private BookWriteModel _model;

        [Test]
        public async Task CreateBook_WhenCalled_ShouldCreateBook()
        {
            var validationResult = await this.Module.SendCommandAsync(Command);
            validationResult.HasErrors.Should().BeFalse();

            var book = Module.ExecuteQueryAsync<BookByGuidQuery, BookDto>(new BookByGuidQuery(_model.BookGuid));

            book.Should().NotBeNull();
        }

        [Test]
        public async Task CreateBook_WhenCalledAndCommandIsNull_ShouldReturnValidationResultWithErrors()
        {
            Command = null;

            var validationResult = await Module.SendCommandAsync(Command);
            validationResult.HasErrors.Should().BeTrue();
        }

        [Test]
        public async Task CreateBook_WhenCalledAndCommandHasInvalidData_ShouldReturnValidationResultWithErrors()
        {
            Command.WriteModel.BookWriteModel.Authors = null;
            Command.WriteModel.BookWriteModel.BookGuid = Guid.Empty;
            Command.WriteModel.BookWriteModel.Basics.Title = string.Empty;

            var validationResult = await Module.SendCommandAsync(Command);
            validationResult.HasErrors.Should().BeTrue();
            validationResult.Errors.Should().Contain(p => p.ErrorProperty == "Authors");
            validationResult.Errors.Should().Contain(p => p.ErrorProperty == "BookGuid");
            validationResult.Errors.Should().Contain(p => p.ErrorProperty == "Title");
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
            var bookReader = new BookReader(Fixture.Create<Guid>(), _readerGuid, Fixture.Create<int>());
            await UnitOfWork.CommitAsync(bookReader);

            var authorDto = CreateAuthorDto();
            await this.Module.SendCommandAsync(new CreateAuthorCommand(authorDto));

            var publisher = new Publisher(Fixture.Create<Guid>(), Fixture.Create<string>());
            await UnitOfWork.CommitAsync(publisher);

            _model = Fixture.Build<BookWriteModel>()
                .With(p => p.BookGuid)
                .With(p => p.Description)
                .WithBookBasics(Category.NonFiction, SubCategory.NonFictionSubCategory.Design, "9788375155280",
                    Fixture.Create<string>(), publisher.Guid).WithDetails(Fixture.Create<int>(), Language.English)
                .WithCover(CoverType.HardCover, false)
                .WithSeries(Guid.Empty, default(byte))
                .With(p => p.AddedBy, bookReader.ReaderGuid)
                .With(p => p.Authors, new List<Guid> { authorDto.AuthorWriteModel.AuthorGuid })
                .WithCycles(new List<Guid>())
                .Create<BookWriteModel>();

            Command = new AddBookCommand(new CreateBookWriteModel
            {
                BookWriteModel = _model,
                PictureWriteModel = new BookPictureWriteModel
                {
                    Cover = string.Empty,
                    FileName = string.Empty
                }
            });
        }

        protected override void SetCompositionRoot()
        {
            CompositionRoot = new CompositionRoot();
        }

        private CreateAuthorWriteModel CreateAuthorDto()
        {
            return new CreateAuthorWriteModel()
            {
                AuthorWriteModel = new AuthorWriteModel
                {
                    ReaderGuid = _readerGuid,
                    AuthorBooks = new List<Guid>(),
                    AuthorGenres = new List<int> { SubCategory.FictionSubCategory.Fantasy.Value },
                    AuthorGuid = Fixture.Create<Guid>(),
                    Basics = new AuthorBasicsWriteModel
                    {
                        FirstName = Fixture.Create<string>(),
                        SecondName = Fixture.Create<string>(),
                        Sex = Sex.Male.Value
                    },
                    Description = new AuthorDescriptionWriteModel
                    {
                        DescriptionSource = Fixture.Create<string>(),
                        AboutAuthor = Fixture.Create<string>(),
                        WebSite = Fixture.Create<string>()
                    },
                    Details = new AuthorDetailsWriteModel
                    {
                        BirthDate = Fixture.Create<DateTime>(),
                        BirthPlace = Fixture.Create<string>(),
                        DeathDate = Fixture.Create<DateTime>()
                    }
                },
                PictureWriteModel = new AuthorPictureWriteModel
                {
                    AuthorImage = string.Empty,
                    FileName = string.Empty
                }
            };
        }
    }
}