using System;
using System.Collections.Generic;
using System.Linq;
using BookLovers.Base.Infrastructure.Ioc;
using BookLovers.Publication.Domain.Quotes;
using BookLovers.Publication.Domain.Quotes.Services;
using Ninject;
using Ninject.Activation;

namespace BookLovers.Publication.Infrastructure.Root.Domain.Providers
{
    public class QuoteFactoryProvider : BaseProvider<IDictionary<QuoteType, IQuoteFactory>>
    {
        public override Type Type => typeof(IDictionary<QuoteType, IQuoteFactory>);

        protected override IDictionary<QuoteType, IQuoteFactory> CreateInstance(
            IContext context)
        {
            return context.Kernel.GetAll<IQuoteFactory>()
                .ToDictionary(k => k.QuoteType, v => v);
        }
    }
}