using System;

namespace BookLovers.Bookcases.Application.WriteModels
{
    public class RemoveFromShelfWriteModel
    {
        public Guid BookcaseGuid { get; set; }

        public Guid BookGuid { get; set; }

        public Guid ShelfGuid { get; set; }
    }
}