using System.Collections.Generic;
using System.Linq;
using BookLovers.Base.Infrastructure.Ioc;
using BookLovers.Publication.Domain.Books.Services.Editors;
using Ninject;
using Ninject.Activation;

namespace BookLovers.Publication.Infrastructure.Root.Domain.Providers
{
    internal class BookEditorsProvider : BaseProvider<List<IBookEditor>>
    {
        protected override List<IBookEditor> CreateInstance(IContext context)
        {
            return context.Kernel.GetAll<IBookEditor>().ToList();
        }
    }
}