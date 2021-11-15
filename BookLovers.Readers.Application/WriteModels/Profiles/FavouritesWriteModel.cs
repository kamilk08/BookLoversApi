using System;
using System.Collections.Generic;

namespace BookLovers.Readers.Application.WriteModels.Profiles
{
    public class FavouritesWriteModel
    {
        public IList<Guid> Authors { get; set; }

        public IList<Guid> Books { get; set; }

        public IList<byte> SubCategories { get; set; }
    }
}