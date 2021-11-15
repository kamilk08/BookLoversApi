using System;
using System.Collections.Generic;

namespace BookLovers.Bookcases.Application.WriteModels
{
    public class ShelfWriteModel
    {
        public Guid ShelfGuid { get; set; }

        public Guid BookCaseGuid { get; set; }

        public string ShelfName { get; set; }

        public byte ShelfCategory { get; set; }

        public ICollection<Guid> BookIds { get; set; }
    }
}