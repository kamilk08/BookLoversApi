using System.Collections.Generic;
using System.Linq;
using BookLovers.Base.Infrastructure.Ioc;
using BookLovers.Readers.Domain.Profiles;
using Ninject;
using Ninject.Activation;

namespace BookLovers.Readers.Infrastructure.Root.Domain
{
    public class FavouriteRemoverProvider : BaseProvider<IDictionary<FavouriteType, IFavouriteRemover>>
    {
        protected override IDictionary<FavouriteType, IFavouriteRemover> CreateInstance(
            IContext context)
        {
            return context.Kernel.GetAll<IFavouriteRemover>()
                .ToDictionary(k => k.FavouriteType, v => v);
        }
    }
}