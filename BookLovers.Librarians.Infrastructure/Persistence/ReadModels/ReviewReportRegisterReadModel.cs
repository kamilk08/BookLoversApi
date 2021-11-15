using BookLovers.Base.Infrastructure.Queries;
using System;
using System.Collections.Generic;

namespace BookLovers.Librarians.Infrastructure.Persistence.ReadModels
{
    public class ReviewReportRegisterReadModel : IReadModel<ReviewReportRegisterReadModel>, IReadModel
    {
        public int Id { get; set; }

        public Guid Guid { get; set; }

        public Guid ReviewGuid { get; set; }

        public Guid? LibrarianGuid { get; set; }

        public IList<ReportRegisterItemReadModel> Reports { get; set; }

        public int Status { get; set; }
    }
}