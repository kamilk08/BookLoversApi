using System;
using System.Collections.Generic;
using System.Linq;
using BookLovers.Base.Infrastructure.Ioc;
using BookLovers.Readers.Domain.Profiles;
using Ninject;
using Ninject.Activation;

namespace BookLovers.Readers.Infrastructure.Root.Domain
{
    public class FavouriteAdderProvider : BaseProvider<IDictionary<FavouriteType, IFavouriteAdder>>
    {
        public override Type Type => typeof(IDictionary<FavouriteType, IFavouriteAdder>);

        protected override IDictionary<FavouriteType, IFavouriteAdder> CreateInstance(
            IContext context)
        {
            return context.Kernel.GetAll<IFavouriteAdder>()
                .ToDictionary(k => k.FavouriteType, v => v);
        }
    }
}