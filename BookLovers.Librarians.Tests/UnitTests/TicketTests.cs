using System;
using System.Threading.Tasks;
using AutoFixture;
using BookLovers.Base.Domain.Rules;
using BookLovers.Base.Infrastructure.Services;
using BookLovers.Librarians.Domain.Librarians;
using BookLovers.Librarians.Domain.TicketOwners;
using BookLovers.Librarians.Domain.Tickets;
using BookLovers.Librarians.Domain.Tickets.Services;
using BookLovers.Librarians.Domain.Tickets.Services.Factories;
using BookLovers.Librarians.Events.Tickets;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace BookLovers.Librarians.Tests.UnitTests
{
    [TestFixture]
    public class TicketTests
    {
        private Ticket _ticket;
        private Fixture _fixture;
        private TicketFactory _ticketFactory;
        private Mock<ITicketConcernProvider> _tickerConcernProviderMock;
        private Mock<ITicketOwnerRepository> _ticketOwnerRepositoryMock;
        private Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private Mock<ITicketConcernChecker> _concernCheckerMock;

        [Test]
        public void SolveTicket_WhenCalled_ShouldResolveTicket()
        {
            var librarian = new Librarian(_fixture.Create<Guid>(), _fixture.Create<Guid>());

            _ticket.SolveTicket(librarian, Decision.Approve);

            var @events = _ticket.GetUncommittedEvents();

            _ticket.Should().NotBeNull();
            _ticket.Decision.Should().Be(Decision.Approve);
            _ticket.SolvedBy.HasBeenSolved().Should().BeTrue();
            _ticket.SolvedBy.LibrarianGuid.Should().Be(librarian.Guid);
            _ticket.TicketState.Should().Be(TicketState.Solved);

            @events.Should().HaveCount(1);
            @events.Should().ContainSingle(p => p.GetType() == typeof(TicketSolved));
        }

        [Test]
        public void SolveTicket_WhenCalledAndTicketIsNotActive_ShouldThrowBusinessRuleNotMeetException()
        {
            _ticket.ArchiveAggregate();

            var librarian = new Librarian(_fixture.Create<Guid>(), _fixture.Create<Guid>());

            Action act = () => _ticket.SolveTicket(librarian, Decision.Approve);

            act.Should().Throw<BusinessRuleNotMetException>();
        }

        [Test]
        public void SolveTicket_WhenCalledAndIsAlreadySolved_ShouldThrowBusinessRuleNotMeetException()
        {
            var librarian = new Librarian(_fixture.Create<Guid>(), _fixture.Create<Guid>());

            _ticket.SolveTicket(librarian, Decision.Approve);

            Action act = () => _ticket.SolveTicket(librarian, Decision.Decline);

            act.Should().Throw<BusinessRuleNotMetException>()
                .WithMessage("Ticket cannot be solved by anyone.");
        }

        [Test]
        public void IsSolved_WhenCalledAndTicketIsSolved_ShouldReturnTrue()
        {
            var librarian = new Librarian(_fixture.Create<Guid>(), _fixture.Create<Guid>());

            _ticket.SolveTicket(librarian, Decision.Approve);

            var result = _ticket.IsSolved();

            result.Should().BeTrue();
        }

        [Test]
        public void IsSolved_WhenCalledAndTicketIsNotSolved_ShouldReturnFalse()
        {
            var result = _ticket.IsSolved();

            result.Should().BeFalse();
        }

        [Test]
        public void IsSolvedBy_WhenCalledWithLibrarianThatSolvedTicket_ShouldReturnTrue()
        {
            var librarian = new Librarian(_fixture.Create<Guid>(), _fixture.Create<Guid>());

            _ticket.SolveTicket(librarian, Decision.Approve);

            var result = _ticket.IsSolvedBy(librarian);

            result.Should().BeTrue();
        }

        [Test]
        public void IsSolvedBy_WhenCalledAndTicketIsNotSolved_ShouldReturnFalse()
        {
            var librarian = new Librarian(_fixture.Create<Guid>(), _fixture.Create<Guid>());

            var result = _ticket.IsSolvedBy(librarian);

            result.Should().BeFalse();
        }

        [Test]
        public void IsTicketAbout_WhenCalled_ShouldReturnTrue()
        {
            var result = _ticket.IsTicketAbout(TicketConcern.Book);

            result.Should().BeTrue();
        }

        [SetUp]
        public Task SetUp()
        {
            _fixture = new Fixture();
            _tickerConcernProviderMock = new Mock<ITicketConcernProvider>();
            _ticketOwnerRepositoryMock = new Mock<ITicketOwnerRepository>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _concernCheckerMock = new Mock<ITicketConcernChecker>();

            var ticketOwnerGuid = _fixture.Create<Guid>();
            var readerId = _fixture.Create<int>();

            _tickerConcernProviderMock.Setup(p => p.GetTicketConcern(TicketConcern.Book.Value))
                .Returns(TicketConcern.Book);

            _ticketOwnerRepositoryMock.Setup(p => p.GetOwnerByReaderGuid(ticketOwnerGuid))
                .Returns(new TicketOwner(_fixture.Create<Guid>(), ticketOwnerGuid, readerId));

            _httpContextAccessorMock.Setup(s => s.UserGuid).Returns(ticketOwnerGuid);

            _concernCheckerMock.Setup(s => s.IsConcernValid(It.IsAny<int>()))
                .Returns(true);

            _ticketFactory =
                new TicketFactorySetup(new TicketFactory(), _ticketOwnerRepositoryMock.Object,
                        _tickerConcernProviderMock.Object, _httpContextAccessorMock.Object, _concernCheckerMock.Object)
                    .GetFactory(TicketFactoryData.Initialize().WithGuid(_fixture.Create<Guid>())
                        .WithTicketObject(_fixture.Create<Guid>())
                        .WithContent(new TicketContentData(_fixture.Create<string>(), _fixture.Create<string>()))
                        .WithDetails(new TicketDetailsData(_fixture.Create<DateTime>(), TicketConcern.Book.Value,
                            _fixture.Create<string>())));

            _ticket = _ticketFactory.Create();

            _ticket.CommitEvents();

            return Task.CompletedTask;
        }
    }
}