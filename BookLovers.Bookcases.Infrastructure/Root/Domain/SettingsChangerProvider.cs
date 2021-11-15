using System;
using System.Collections.Generic;
using System.Linq;
using BookLovers.Base.Infrastructure.Ioc;
using BookLovers.Bookcases.Domain.Settings;
using Ninject;
using Ninject.Activation;

namespace BookLovers.Bookcases.Infrastructure.Root.Domain
{
    public class SettingsChangerProvider :
        BaseProvider<IDictionary<BookcaseOptionType, ISettingsChanger>>
    {
        public override Type Type
        {
            get { return typeof(IDictionary<BookcaseOptionType, ISettingsChanger>); }
        }

        protected override IDictionary<BookcaseOptionType, ISettingsChanger> CreateInstance(
            IContext context)
        {
            return context.Kernel.GetAll<ISettingsChanger>()
                .ToDictionary(k => k.OptionType, v => v);
        }
    }
}