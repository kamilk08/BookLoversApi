using AutoMapper;
using BookLovers.Librarians.Domain.Librarians;
using BookLovers.Librarians.Infrastructure.Persistence.ReadModels;
using System.Linq;

namespace BookLovers.Librarians.Infrastructure.Persistence.Providers
{
    public class DecisionProvider : IDecisionProvider, IDecisionChecker
    {
        private readonly LibrariansContext _context;
        private readonly IMapper _mapper;

        public DecisionProvider(LibrariansContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        public Decision GetDecision(int decisionType)
        {
            var decision = this._context.Decisions.SingleOrDefault(p => p.Value == decisionType);

            return this._mapper.Map<DecisionReadModel, Decision>(decision);
        }

        public bool IsDecisionValid(int decisionId)
        {
            return this._context.Decisions.AsNoTracking().Any(a => a.Value == decisionId);
        }
    }
}