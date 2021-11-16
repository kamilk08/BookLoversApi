using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using BaseTests;
using BaseTests.EndToEndHelpers;
using BookLovers.Base.Domain.Rules;
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
using BookLovers.Shared.Likes;
using BookLovers.Shared.SharedSexes;
using FluentAssertions;
using Moq;
using Ninject;
using NUnit.Framework;

namespace BookLovers.Publication.Tests.IntegrationTests.UnLikeQuote
{
    public class UnLikeQuoteTests : IntegrationTest<PublicationModule, UnLikeQuoteCommand>
    {
        private Mock<IHttpContextAccessor> _mock = new Mock<IHttpContextAccessor>();
        private Guid _readerGuid;
        private Quote _authorQuote;
        private int _readerId;

        [Test]
        public async Task UnLikeQuote_WhenCalled_ShouldRemoveQuoteLike()
        {
            var queryResult =
                await Module.ExecuteQueryAsync<PaginatedUserQuotesQuery, PaginatedResult<QuoteDto>>(
                    new PaginatedUserQuotesQuery(_readerId, PaginatedResult.DefaultPage,
                        PaginatedResult.DefaultItemsPerPage, QuotesOrder.ByLikes.Value, true));

            queryResult.Value.Items.First().QuoteLikes.Count.Should().Be(1);

            var validationResult = await Module.SendCommandAsync(Command);

            validationResult.HasErrors.Should().BeFalse();

            queryResult =
                await Module.ExecuteQueryAsync<PaginatedUserQuotesQuery, PaginatedResult<QuoteDto>>(
                    new PaginatedUserQuotesQuery(_readerId, PaginatedResult.DefaultPage,
                        PaginatedResult.DefaultItemsPerPage, QuotesOrder.ByLikes.Value, true));

            queryResult.Value.Items.First().QuoteLikes.Should().BeEmpty();
        }

        [Test]
        public async Task UnLikeQuote_WhenCalledAndCommandIsNotValid_ShouldReturnValidationResultWithErrors()
        {
            Command = null;

            var validationResult = await Module.SendCommandAsync(Command);

            validationResult.HasErrors.Should().BeTrue();
        }

        [Test]
        public async Task UnLikeQuote_WhenCalledAndCommandHasInvalidData_ShouldReturnValidationResultWithErrors()
        {
            Command = new UnLikeQuoteCommand(Guid.Empty);

            var validationResult = await Module.SendCommandAsync(Command);

            validationResult.HasErrors.Should().BeTrue();
            validationResult.Errors.Should().Contain(p => p.ErrorProperty == "QuoteGuid");
        }

        [Test]
        public async Task UnLikeQuote_WhenCalledAndQuoteIsNotActive_ShouldThrowBusinessRuleNotMeetException()
        {
            var quote = await UnitOfWork.GetAsync<Quote>(_authorQuote.Guid);

            quote.ApplyChange(new QuoteArchived(quote.Guid, quote.QuoteContent.QuotedGuid,
                _authorQuote.QuoteDetails.AddedByGuid));

            await UnitOfWork.CommitAsync(quote);

            Func<Task> act = async () => await Module.SendCommandAsync(Command);

            act.Should().Throw<BusinessRuleNotMetException>();
        }

        [Test]
        public void
            UnLikeQuote_WhenCalledAndQuoteDoesNotHaveQuoteFromSelectedReader_ShouldThrowBusinessRuleNotMeetException()
        {
            _mock.Setup(s => s.UserGuid).Returns(Fixture.Create<Guid>());

            Func<Task> act = async () => await Module.SendCommandAsync(Command);

            act.Should().Throw<BusinessRuleNotMetException>()
                .WithMessage("Quote must contain selected like.");
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
            _readerId = Fixture.Create<int>();
            var bookReader = new BookReader(Fixture.Create<Guid>(), _readerGuid, _readerId);

            await UnitOfWork.CommitAsync(bookReader);

            var authorFactory = CompositionRoot.Kernel.Get<AuthorFactory>();

            var factoryData = this.CreateAuthorFactoryData();

            var author = authorFactory.CreateAuthor(factoryData);

            await UnitOfWork.CommitAsync(author);

            _authorQuote = new Quote(Fixture.Create<Guid>(), new QuoteContent(Fixture.Create<string>(), author.Guid),
                new QuoteDetails(_readerGuid, Fixture.Create<DateTime>(), QuoteType.AuthorQuote));

            var @event = AuthorQuoteAdded.Initialize()
                .WithAggregate(_authorQuote.Guid)
                .WithAuthor(_authorQuote.QuoteContent.QuotedGuid)
                .WithQuote(_authorQuote.QuoteContent.Content, _authorQuote.QuoteDetails.AddedAt)
                .WithAddedBy(_authorQuote.QuoteDetails.AddedByGuid);

            _authorQuote.ApplyChange(@event);

            _authorQuote.AddLike(Like.NewLike(_readerGuid));

            await UnitOfWork.CommitAsync(_authorQuote);

            Command = new UnLikeQuoteCommand(_authorQuote.Guid);
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