using System.Collections.Generic;
using BookLovers.Base.Domain.Builders;
using BookLovers.Base.Domain.Services;
using BookLovers.Publication.Domain.Authors.Services;
using BookLovers.Publication.Domain.Authors.Services.Editors;
using BookLovers.Publication.Domain.BookReaders;
using BookLovers.Publication.Domain.Books;
using BookLovers.Publication.Domain.Books.IsbnValidation;
using BookLovers.Publication.Domain.Books.Services;
using BookLovers.Publication.Domain.Books.Services.Editors;
using BookLovers.Publication.Domain.PublisherCycles;
using BookLovers.Publication.Domain.Publishers.Services;
using BookLovers.Publication.Domain.Quotes;
using BookLovers.Publication.Domain.Quotes.Services;
using BookLovers.Publication.Domain.SeriesCycle;
using BookLovers.Publication.Domain.SeriesCycle.Services;
using BookLovers.Publication.Infrastructure.Root.Domain.Providers;
using BookLovers.Publication.Infrastructure.Services;
using Ninject.Extensions.Conventions;
using Ninject.Modules;

namespace BookLovers.Publication.Infrastructure.Root.Domain
{
    internal class DomainModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind(x =>
                x.FromAssemblyContaining(typeof(Book))
                    .SelectAllClasses()
                    .InheritedFrom(typeof(IBuilder<>))
                    .BindAllInterfaces());

            this.Bind(x =>
                x.FromAssemblyContaining(typeof(Book))
                    .SelectAllClasses()
                    .InheritedFrom(typeof(IIsbnValidator))
                    .BindAllInterfaces());

            Bind<IDictionary<IsbnType, IIsbnValidator>>()
                .ToProvider<IsbnValidatorProvider>();

            Bind<IsbnValidatorFactory>().ToSelf();

            this.Bind(x =>
                x.FromAssemblyContaining(typeof(Book)).SelectAllClasses()
                    .InheritedFrom(typeof(IDomainService<>))
                    .BindAllInterfaces());

            this.Bind(x =>
                x.FromAssemblyContaining(typeof(Quote))
                    .IncludingNonPublicTypes().SelectAllClasses()
                    .InheritedFrom(typeof(IQuoteFactory))
                    .BindAllInterfaces());

            Bind<IDictionary<QuoteType, IQuoteFactory>>()
                .ToProvider<QuoteFactoryProvider>();

            this.Bind(x =>
                x.FromAssemblyContaining(typeof(IAuthorEditor))
                    .IncludingNonPublicTypes().SelectAllClasses()
                    .InheritedFrom(typeof(IAuthorEditor))
                    .BindAllInterfaces());

            Bind<IList<IAuthorEditor>>().ToProvider<AuthorEditorsProvider>();

            this.Bind(x =>
                x.FromAssemblyContaining(typeof(Book))
                    .IncludingNonPublicTypes()
                    .SelectAllClasses()
                    .InheritedFrom(typeof(IBookEditor))
                    .BindAllInterfaces());

            Bind<IList<IBookEditor>>().ToProvider<BookEditorsProvider>();

            this.Bind(x =>
                x.FromAssemblyContaining(typeof(Book))
                    .IncludingNonPublicTypes()
                    .SelectAllClasses()
                    .InheritedFrom(typeof(IAggregateManager<>))
                    .BindAllInterfaces());

            Bind<BookFactory>().ToSelf();

            Bind<AuthorFactory>().ToSelf();

            Bind<PublisherCycleFactory>().ToSelf();

            Bind<PublisherFactory>().ToSelf();

            Bind<SeriesFactory>().ToSelf();

            Bind<ITitleUniquenessChecker>().To<TitleUniquenessChecker>();

            Bind<IIsbnUniquenessChecker>().To<IsbnUniquenessChecker>();

            Bind<IPublisherUniquenessChecker>().To<PublisherUniquenessChecker>();

            Bind<IAuthorUniquenessChecker>().To<AuthorUniquenessChecker>();

            Bind<IPublisherCycleUniquenessChecker>().To<PublisherCycleUniquenessChecker>();

            Bind<ISeriesUniquenessChecker>().To<SeriesUniquenessChecker>();

            Bind<IQuoteUniquenessChecker>().To<QuoteUniquenessChecker>();

            Bind<IUnknownAuthorService>().To<UnknownAuthorService>();

            Bind<ISelfPublisherService>().To<SelfPublisherService>();

            Bind<IBookReaderAccessor>().To<BookReaderAccessor>();
        }
    }
}