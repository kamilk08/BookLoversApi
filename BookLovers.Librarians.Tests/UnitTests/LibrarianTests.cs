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
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace BookLovers.Librarians.Tests.UnitTests
{
    [TestFixture]
    public class LibrarianTests
    {
        private Librarian _librarian;
        private Fixture _fixture;
        private Ticket _ticket;
        private Mock<ITicketConcernProvider> _ticketConcernMock;
        private Mock<ITicketOwnerRepository> _ticketOwnerRepository;
        private Mock<ITicketConcernChecker> _concernCheckerMock;
        private Mock<IDecisionChecker> _decisionCheckerMock;

        [Test]
        public void ResolveTicket_WhenCalled_GivenTicketShouldBeResolved()
        {
            _librarian.ResolveTicket(_ticket, Decision.Approve,
                new DecisionJustification(_fixture.Create<string>(), _fixture.Create<DateTime>()),
                _decisionCheckerMock.Object);

            var ticket = _librarian.GetTicket(_ticket.Guid);

            _librarian.GetUncommittedEvents().Should().HaveCount(1);
            _librarian.Tickets.Should().HaveCount(1);
            ticket.Should().NotBeNull();
            ticket.Decision.Should().Be(Decision.Approve);
            ticket.TicketGuid.Should().Be(_ticket.Guid);
        }

        [Test]
        public void ResolveTicket_WhenCalledAndLibrarianIsNotActive_ShouldThrowBusinessRuleNotMeetException()
        {
            _librarian.ArchiveAggregate();

            var justification =
                new DecisionJustification(_fixture.Create<string>(), _fixture.Create<DateTime>());

            Action act = () => _librarian.ResolveTicket(_ticket, Decision.Approve, justification,
                _decisionCheckerMock.Object);

            act.Should().Throw<BusinessRuleNotMetException>();
        }

        [Test]
        public void ResolveTicket_WhenCalledAndTicketAlreadyResolved_ShouldThrowLibrarianAlreadyResolvedTicket()
        {
            var justification =
                new DecisionJustification(_fixture.Create<string>(), _fixture.Create<DateTime>());

            _librarian.ResolveTicket(_ticket, Decision.Approve, justification,
                _decisionCheckerMock.Object);

            Action act = () => _librarian.ResolveTicket(_ticket, Decision.Approve,
                new DecisionJustification(_fixture.Create<string>(), _fixture.Create<DateTime>()),
                _decisionCheckerMock.Object);

            act.Should().Throw<BusinessRuleNotMetException>()
                .WithMessage("Ticket can be resolved only once.");
        }

        [Test]
        public void GetTicket_WhenCalled_ShouldReturnResolvedTicket()
        {
            _librarian.ResolveTicket(_ticket, Decision.Decline,
                new DecisionJustification(_fixture.Create<string>(), _fixture.Create<DateTime>()),
                _decisionCheckerMock.Object);

            var ticket = _librarian.GetTicket(_ticket.Guid);

            ticket.Should().NotBeNull();
            ticket.TicketGuid.Should().Be(_ticket.Guid);
            ticket.Decision.Should().Be(Decision.Decline);
            ticket.Justification.Should().NotBeNull();
        }

        [Test]
        public void GetTicket_WhenCalledAndNotTicketHasBeenResolved_ShouldReturnNull()
        {
            _librarian.GetTicket(_ticket.Guid).Should().BeNull();
        }

        [Test]
        public void HasTicket_WhenCalledAndThereIsResolvedTicket_ShouldReturnTrue()
        {
            _librarian.ResolveTicket(_ticket, Decision.Decline,
                new DecisionJustification(_fixture.Create<string>(), _fixture.Create<DateTime>()),
                _decisionCheckerMock.Object);

            _librarian.HasResolvedTicket(_ticket.Guid).Should().BeTrue();
        }

        [Test]
        public void HasTicket_WhenCalledAndLibrarianHasNoResolvedTicket_ShouldReturnFalse()
        {
            _librarian.HasResolvedTicket(_ticket.Guid).Should().BeFalse();
        }

        [SetUp]
        public Task SetUp()
        {
            _fixture = new Fixture();
            _librarian = new Librarian(_fixture.Create<Guid>(), _fixture.Create<Guid>());
            _librarian.CommitEvents();
            _concernCheckerMock = new Mock<ITicketConcernChecker>();
            _ticketConcernMock = new Mock<ITicketConcernProvider>();
            _ticketOwnerRepository = new Mock<ITicketOwnerRepository>();
            _decisionCheckerMock = new Mock<IDecisionChecker>();

            var readerGuid = _fixture.Create<Guid>();
            var readerId = _fixture.Create<int>();

            _ticketConcernMock.Setup(p => p.GetTicketConcern(TicketConcern.Book.Value))
                .Returns(TicketConcern.Book);

            _ticketOwnerRepository.Setup(p => p.GetOwnerByReaderGuid(readerGuid))
                .Returns(new TicketOwner(_fixture.Create<Guid>(), readerGuid, readerId));

            _concernCheckerMock.Setup(s => s.IsConcernValid(It.IsAny<int>()))
                .Returns(true);

            _decisionCheckerMock.Setup(s => s.IsDecisionValid(It.IsAny<int>()))
                .Returns(true);

            var httpAccessorMock = new Mock<IHttpContextAccessor>();
            httpAccessorMock.Setup(s => s.UserGuid).Returns(readerGuid);

            var data = TicketFactoryData.Initialize().WithGuid(_fixture.Create<Guid>())
                .WithTicketObject(_fixture.Create<Guid>())
                .WithContent(new TicketContentData(_fixture.Create<string>(), _fixture.Create<string>()))
                .WithDetails(new TicketDetailsData(_fixture.Create<DateTime>(), TicketConcern.Book.Value,
                    _fixture.Create<string>()));

            _ticket = new TicketFactorySetup(new TicketFactory(), _ticketOwnerRepository.Object,
                    _ticketConcernMock.Object, httpAccessorMock.Object, _concernCheckerMock.Object)
                .GetFactory(data)
                .Create();

            return Task.CompletedTask;
        }
    }
}