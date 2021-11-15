using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using BaseTests;
using BaseTests.EndToEndHelpers;
using BaseTests.EndToEndHelpers.Mocks;
using BookLovers.Base.Infrastructure.Extensions;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Base.Infrastructure.Services;
using BookLovers.Publication.Application.Commands.Quotes;
using BookLovers.Publication.Domain.Authors.Services;
using BookLovers.Publication.Domain.BookReaders;
using BookLovers.Publication.Domain.Quotes;
using BookLovers.Publication.Events.Quotes;
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

namespace BookLovers.Publication.Tests.IntegrationTests.LikeQuote
{
    public class LikeQuoteTestsWithEventStoreSnapshot :
        IntegrationTest<PublicationModule, LikeQuoteCommand>
    {
        private Guid _readerGuid;
        private Quote _aggregate;
        private int _readerId;

        [Test]
        public async Task LikeQuote_WhenCalled_ShouldAddQuoteLike()
        {
            var validationResult = await Module.SendCommandAsync(Command);
            validationResult.HasErrors.Should().BeFalse();

            var queryResult =
                await Module.ExecuteQueryAsync<PaginatedUserQuotesQuery, PaginatedResult<QuoteDto>>(
                    new PaginatedUserQuotesQuery(_readerId, PaginatedResult.DefaultPage,
                        PaginatedResult.DefaultItemsPerPage, QuotesOrder.ByLikes.Value, true));

            queryResult.Value.TotalItems.Should().Be(1);
            queryResult.Value.Items.First().QuoteLikes.Count.Should().Be(1);
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
            var factoryData = this.CreateAuthorFactoryData();

            var author = authorFactory.CreateAuthor(factoryData);

            await UnitOfWork.CommitAsync(author);

            _aggregate = new Quote(Fixture.Create<Guid>(), new QuoteContent(Fixture.Create<string>(), author.Guid),
                new QuoteDetails(_readerGuid, Fixture.Create<DateTime>(), QuoteType.AuthorQuote));

            var @event = AuthorQuoteAdded.Initialize()
                .WithAggregate(_aggregate.Guid)
                .WithAuthor(_aggregate.QuoteContent.QuotedGuid)
                .WithQuote(_aggregate.QuoteContent.Content, _aggregate.QuoteDetails.AddedAt)
                .WithAddedBy(_aggregate.QuoteDetails.AddedByGuid);

            _aggregate.ApplyChange(@event);

            await UnitOfWork.CommitAsync(_aggregate);

            await Enumerable.Range(0, 30).ForEach(async i =>
            {
                await this.Module.SendCommandAsync(new LikeQuoteCommand(_aggregate.Guid));
                await this.Module.SendCommandAsync(new UnLikeQuoteCommand(_aggregate.Guid));
            });

            Command = new LikeQuoteCommand(_aggregate.Guid);
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