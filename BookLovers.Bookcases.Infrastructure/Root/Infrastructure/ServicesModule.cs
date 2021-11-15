using System.Collections.Generic;
using AutoMapper;
using BookLovers.Base.Infrastructure;
using BookLovers.Bookcases.Infrastructure.Services;
using Ninject.Extensions.Conventions;
using Ninject.Modules;
using MapperConfiguration = BookLovers.Bookcases.Infrastructure.Mappings.MapperConfiguration;

namespace BookLovers.Bookcases.Infrastructure.Root.Infrastructure
{
    internal class ServicesModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<IMapper>()
                .ToMethod(m => MapperConfiguration.CreateMapper()).InSingletonScope();

            this.Bind<IModule<BookcaseModule>>().To<BookcaseModule>();

            this.Bind(x => x.FromThisAssembly()
                .IncludingNonPublicTypes()
                .SelectAllClasses()
                .InheritedFrom(typeof(IBookcaseCollectionSorter))
                .BindAllInterfaces());

            this.Bind<IDictionary<BookcaseCollectionSortType, IBookcaseCollectionSorter>>()
                .ToProvider<BookcaseCollectionSortingProvider>();
        }
    }
}