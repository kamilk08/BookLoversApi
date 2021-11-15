namespace BookLovers.Librarians.Domain.Tickets.Services
{
    public interface ITicketConcernChecker
    {
        bool IsConcernValid(int concernType);
    }
}