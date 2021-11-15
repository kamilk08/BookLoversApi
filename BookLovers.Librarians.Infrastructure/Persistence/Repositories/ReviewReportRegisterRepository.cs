using AutoMapper;
using BookLovers.Base.Infrastructure;
using BookLovers.Librarians.Domain.ReviewReportRegisters;
using BookLovers.Librarians.Infrastructure.Persistence.ReadModels;
using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Threading.Tasks;

namespace BookLovers.Librarians.Infrastructure.Persistence.Repositories
{
    public class ReviewReportRegisterRepository :
        IReviewReportRegisterRepository,
        IRepository<ReviewReportRegister>
    {
        private readonly LibrariansContext _context;
        private readonly IMapper _mapper;

        public ReviewReportRegisterRepository(LibrariansContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        public async Task<ReviewReportRegister> GetAsync(Guid aggregateGuid)
        {
            var reportRegister = await this._context.ReviewReports
                .Include(p => p.Reports)
                .SingleOrDefaultAsync(p => p.Guid == aggregateGuid);

            return this._mapper.Map<ReviewReportRegisterReadModel, ReviewReportRegister>(reportRegister);
        }

        public async Task<ReviewReportRegister> GetByReviewGuidAsync(
            Guid reviewGuid)
        {
            var reportRegister = await this._context.ReviewReports.Include(p => p.Reports)
                .SingleOrDefaultAsync(p => p.ReviewGuid == reviewGuid);

            return this._mapper.Map<ReviewReportRegisterReadModel, ReviewReportRegister>(reportRegister);
        }

        public async Task CommitChangesAsync(ReviewReportRegister aggregate)
        {
            var destination = await this._context.ReviewReports.Include(p => p.Reports)
                .SingleOrDefaultAsync(p => p.Guid == aggregate.Guid);

            var mapped = this._mapper.Map(aggregate, destination);

            this._context.ReviewReports.AddOrUpdate(p => p.Id, mapped);

            await this._context.SaveChangesAsync();
        }
    }
}