using System;

namespace BookLovers.Bookcases.Application.WriteModels
{
    public class ChangeShelfWriteModel
    {
        public Guid BookcaseGuid { get; set; }

        public Guid NewShelfGuid { get; set; }

        public Guid OldShelfGuid { get; set; }

        public Guid BookGuid { get; set; }
    }
}