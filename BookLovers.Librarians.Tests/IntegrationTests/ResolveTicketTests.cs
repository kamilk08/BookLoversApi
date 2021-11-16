using System;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using BaseTests;
using BaseTests.EndToEndHelpers;
using BookLovers.Base.Domain.Rules;
using BookLovers.Base.Infrastructure.Extensions;
using BookLovers.Base.Infrastructure.Persistence;
using BookLovers.Base.Infrastructure.Services;
using BookLovers.Librarians.Application.Commands.Tickets;
using BookLovers.Librarians.Application.WriteModels;
using BookLovers.Librarians.Domain.Librarians;
using BookLovers.Librarians.Domain.TicketOwners;
using BookLovers.Librarians.Domain.Tickets;
using BookLovers.Librarians.Domain.Tickets.Services.Factories;
using BookLovers.Librarians.Infrastructure.Dtos;
using BookLovers.Librarians.Infrastructure.Persistence;
using BookLovers.Librarians.Infrastructure.Queries.Tickets;
using BookLovers.Librarians.Infrastructure.Root;
using FluentAssertions;
using Moq;
using Ninject;
using NUnit.Framework;

namespace BookLovers.Librarians.Tests.IntegrationTests
{
    public class ResolveTicketTests : IntegrationTest<LibrarianModule, ResolveTicketCommand>
    {
        private Mock<IHttpContextAccessor> _httpAccessorMock = new Mock<IHttpContextAccessor>();
        private Guid _librarianGuid;
        private Guid _ticketGuid;
        private Guid _ticketOwnerGuid;
        private Mock<IAppManager> _managerMock;

        [Test]
        public async Task ResolveTicket_WhenCalled_LibrarianShouldResolveSelectedTicket()
        {
            var validationResult = await Module.SendCommandAsync(Command);
            validationResult.HasErrors.Should().BeFalse();

            var queryResult =
                await Module.ExecuteQueryAsync<TicketByGuidQuery, TicketDto>(new TicketByGuidQuery(_ticketGuid));

            queryResult.Should().NotBeNull();
            queryResult.Value.Guid.Should().Be(_ticketGuid);
            queryResult.Value.TicketDecision.Should().Be(Decision.Approve.Value);
            queryResult.Value.TicketState.Should().Be(TicketState.Solved.Value);
        }

        [Test]
        public async Task ResolveTicket_WhenCalledAndCommandIsNotValid_ShouldReturnFailureResult()
        {
            Command = null;

            var validationResult = await Module.SendCommandAsync(Command);
            validationResult.HasErrors.Should().BeTrue();
            validationResult.Errors.First().ErrorMessage.Should().Be("Command cannot be null.");
        }

        [Test]
        public async Task ResolveTicket_WhenCalledAndCommandHasInvalidData_ShouldReturnFailureResult()
        {
            var dto = Fixture.Build<ResolveTicketWriteModel>()
                .With(p => p.TicketGuid, Guid.Empty)
                .Create();

            Command = new ResolveTicketCommand(dto);

            var validationResult = await Module.SendCommandAsync(Command);
            validationResult.HasErrors.Should().BeTrue();
            validationResult.Errors.Should().Contain(p => p.ErrorProperty == "TicketGuid");
        }

        [Test]
        public async Task ResolveTicket_WhenCalledAndTicketIsAlreadyResolved_ShouldThrowBusinessRuleNotMeetException()
        {
            await Module.SendCommandAsync(Command);

            Func<Task> act = async () => await Module.SendCommandAsync(new ResolveTicketCommand(Command.WriteModel));

            act.Should().Throw<BusinessRuleNotMetException>()
                .WithMessage("Ticket can be resolved only once.");
        }

        protected override void InitializeRoot()
        {
            _librarianGuid = Fixture.Create<Guid>();

            _httpAccessorMock.Setup(s => s.UserGuid).Returns(_librarianGuid);

            _managerMock = new Mock<IAppManager>();

            var librariansConnectionString = Environment.GetEnvironmentVariable(LibrariansContext.ConnectionStringKey);
            if (librariansConnectionString.IsEmpty())
                librariansConnectionString = E2EConstants.LibrariansConnectionString;

            _managerMock.Setup(s => s.GetConfigValue(LibrariansContext.ConnectionStringKey))
                .Returns(librariansConnectionString);

            LibrarianModuleStartup.Initialize(_httpAccessorMock.Object, _managerMock.Object,
                new FakeLogger().GetLogger(), PersistenceSettings.Default());
        }

        protected override Task ClearStore()
        {
            return Task.CompletedTask;
        }

        protected override Task ClearReadContext()
        {
            CompositionRoot.Kernel.Get<LibrariansContext>().CleanLibrarianContext();

            return Task.CompletedTask;
        }

        protected override async Task PrepareData()
        {
            var readerGuid = this.Fixture.Create<Guid>();
            var librarian = new Librarian(this._librarianGuid, readerGuid);

            await this.UnitOfWork.CommitAsync(librarian, false);

            this._ticketOwnerGuid = this.Fixture.Create<Guid>();

            var readerId = this.Fixture.Create<int>();
            var ticketOwner = new TicketOwner(
                this.Fixture.Create<Guid>(),
                this._ticketOwnerGuid, readerId);
            await this.UnitOfWork.CommitAsync(ticketOwner);

            this._httpAccessorMock.Setup(s => s.UserGuid)
                .Returns(this._ticketOwnerGuid);

            LibrarianModuleStartup.Initialize(
                this._httpAccessorMock.Object,
                this._managerMock.Object, new FakeLogger().GetLogger(),
                PersistenceSettings.DoNotCleanContext());

            var data = TicketFactoryData.Initialize()
                .WithGuid(this.Fixture.Create<Guid>())
                .WithTicketObject(this.Fixture.Create<Guid>())
                .WithContent(new TicketContentData(
                    this.Fixture.Create<string>(),
                    this.Fixture.Create<string>())).WithDetails(
                    new TicketDetailsData(
                        this.Fixture.Create<DateTime>(),
                        TicketConcern.Book.Value,
                        this.Fixture.Create<string>()));

            var ticket = this.CompositionRoot.Kernel.Get<TicketFactorySetup>().GetFactory(data).Create();

            await this.UnitOfWork.CommitAsync(ticket);

            this._ticketGuid = ticket.Guid;

            var writeModel = this.Fixture.Build<ResolveTicketWriteModel>()
                .With(p => p.Date)
                .With(p => p.DecisionJustification)
                .With(p => p.LibrarianGuid, this._librarianGuid)
                .With(p => p.DecisionType, Decision.Approve.Value)
                .With(p => p.TicketConcern, TicketConcern.Book.Value)
                .With(p => p.TicketGuid, this._ticketGuid)
                .Create();

            this.Command = new ResolveTicketCommand(writeModel);
        }

        protected override void SetCompositionRoot()
        {
            CompositionRoot = new CompositionRoot();
        }
    }
}