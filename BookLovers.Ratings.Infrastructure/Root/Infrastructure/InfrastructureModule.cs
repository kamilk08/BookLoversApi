using AutoMapper;
using BookLovers.Base.Infrastructure;
using BookLovers.Ratings.Infrastructure.Mappings;
using Ninject.Modules;

namespace BookLovers.Ratings.Infrastructure.Root.Infrastructure
{
    internal class InfrastructureModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IMapper>()
                .ToMethod(cfg => RatingsMappingConfiguration.ConfigureMapper())
                .InSingletonScope();

            Bind<IModule<RatingsModule>>().To<RatingsModule>();
        }
    }
}