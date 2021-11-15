using AutoMapper;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Bookcases.Events.Bookcases;
using BookLovers.Bookcases.Infrastructure.Persistence;
using BookLovers.Bookcases.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Bookcases.Infrastructure.Projections.Bookcases
{
    internal class BookcaseCreatedProjection : IProjectionHandler<BookcaseCreated>, IProjectionHandler
    {
        private readonly BookcaseContext _context;
        private readonly IMapper _mapper;

        public BookcaseCreatedProjection(BookcaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public void Handle(BookcaseCreated @event)
        {
            var bookcase = _mapper.Map<BookcaseReadModel>(@event);

            _context.Bookcases.Add(bookcase);
            _context.SaveChanges();
        }
    }
}