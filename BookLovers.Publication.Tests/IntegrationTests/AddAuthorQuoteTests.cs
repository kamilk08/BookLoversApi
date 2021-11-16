using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using BaseTests;
using BaseTests.EndToEndHelpers;
using BaseTests.EndToEndHelpers.Mocks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Extensions;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Base.Infrastructure.Services;
using BookLovers.Publication.Application.Commands.Quotes;
using BookLovers.Publication.Application.WriteModels.Quotes;
using BookLovers.Publication.Domain.Authors.Services;
using BookLovers.Publication.Domain.BookReaders;
using BookLovers.Publication.Infrastructure.Dtos.Publications;
using BookLovers.Publication.Infrastructure.Persistence;
using BookLovers.Publication.Infrastructure.Queries.Quotes;
using BookLovers.Publication.Infrastructure.Root;
using BookLovers.Publication.Store.Persistence;
using BookLovers.Shared;
using BookLovers.Shared.Categories;
using BookLovers.Shared.SharedSexes;
using FluentAssertions;
using Moq;
using Ninject;
using NUnit.Framework;

namespace BookLovers.Publication.Tests.IntegrationTests
{
    public class AddAuthorQuoteTests : IntegrationTest<PublicationModule, AddAuthorQuoteCommand>
    {
        private Guid _readerGuid;
        private int _readerId;

        [Test]
        public async Task AddAuthorQuote_WhenCalled_ShouldAddAuthorQuote()
        {
            var queryResult =
                await Module.ExecuteQueryAsync<PaginatedUserQuotesQuery, PaginatedResult<QuoteDto>>(
                    new PaginatedUserQuotesQuery(_readerId, PaginatedResult.DefaultPage,
                        PaginatedResult.DefaultItemsPerPage, QuotesOrder.ByLikes.Value, true));

            queryResult.Value.TotalItems.Should().Be(0);

            var validationResult = await Module.SendCommandAsync(Command);
            validationResult.HasErrors.Should().BeFalse();

            queryResult = await Module.ExecuteQueryAsync<PaginatedUserQuotesQuery, PaginatedResult<QuoteDto>>(
                new PaginatedUserQuotesQuery(_readerId, PaginatedResult.DefaultPage,
                    PaginatedResult.DefaultItemsPerPage, QuotesOrder.ByLikes.Value, true));

            queryResult.Value.TotalItems.Should().Be(1);
        }

        [Test]
        public async Task AddAuthorQuote_WhenCalledAndCommandIsNotValid_ShouldReturnFailure()
        {
            this.Module = CompositionRoot.Kernel.Get<IValidationDecorator<PublicationModule>>();

            Command = null;

            var validationResult = await Module.SendCommandAsync(Command);
            validationResult.HasErrors.Should().BeTrue();
            validationResult.Errors.First().ErrorMessage.Should().Be("Command cannot be null.");
        }

        [Test]
        public async Task AddAuthorQuote_WhenCalledAndCommandHasNotValidData_ShouldReturnFailureResult()
        {
            Module = CompositionRoot.Kernel.Get<IValidationDecorator<PublicationModule>>();

            Command = new AddAuthorQuoteCommand(new AddQuoteWriteModel
            {
                QuoteGuid = Guid.Empty,
                Quote = string.Empty,
                QuotedGuid = Guid.Empty,
                AddedAt = default(DateTime)
            });

            var result = await Module.SendCommandAsync(Command);
            result.HasErrors.Should().BeTrue();
            result.Errors.Count().Should().Be(4);
            result.Errors.Should().Contain(p => p.ErrorProperty == "QuoteGuid");
            result.Errors.Should().Contain(p => p.ErrorProperty == "Quote");
            result.Errors.Should().Contain(p => p.ErrorProperty == "QuotedGuid");
            result.Errors.Should().Contain(p => p.ErrorProperty == "AddedAt");
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
            _readerId = Fixture.Create<int>();
            var bookReader = new BookReader(Fixture.Create<Guid>(), _readerGuid, _readerId);

            await UnitOfWork.CommitAsync(bookReader);

            var authorName = Fixture.Create<string>();

            var authorFactory = CompositionRoot.Kernel.Get<AuthorFactory>();

            var data = CreateAuthorFactoryData();

            var author = authorFactory.CreateAuthor(data);

            await UnitOfWork.CommitAsync(author);

            var addAuthorDto = Fixture.Build<AddQuoteWriteModel>()
                .With(p => p.Quote)
                .With(p => p.AddedAt)
                .With(p => p.QuotedGuid, author.Guid)
                .Create<AddQuoteWriteModel>();

            Command = new AddAuthorQuoteCommand(addAuthorDto);
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
                .AddedBy(new BookReader(Fixture.Create<Guid>(), _readerGuid, _readerId))
                .WithBooks(new List<Guid>())
                .WithGenres(new List<int> { SubCategory.FictionSubCategory.Thriller.Value });
        }
    }
}