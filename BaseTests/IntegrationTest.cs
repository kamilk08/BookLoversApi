using System.Threading.Tasks;
using AutoFixture;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Ioc;
using Ninject;
using NUnit.Framework;

namespace BaseTests
{
    [TestFixture]
    public abstract class IntegrationTest<TModule, TCommand>
        where TCommand : class, ICommand
    {
        protected IModule<TModule> Module;
        protected TCommand Command;
        protected IUnitOfWork UnitOfWork;
        protected ICompositionRoot CompositionRoot;
        protected Fixture Fixture;

        protected abstract void InitializeRoot();

        protected abstract Task ClearStore();

        protected abstract Task ClearReadContext();

        protected abstract Task PrepareData();

        protected abstract void SetCompositionRoot();

        [SetUp]
        protected async Task BeforeEachTest()
        {
            Fixture = new Fixture();

            SetCompositionRoot();

            InitializeRoot();

            Module = CompositionRoot.Kernel.Get<IValidationDecorator<TModule>>();
            UnitOfWork = CompositionRoot.Kernel.Get<IUnitOfWork>();

            await ClearStore();
            await ClearReadContext();
            await PrepareData();
        }

        [TearDown]
        protected async Task AfterEachTest()
        {
            await ClearStore();
            await ClearReadContext();
        }
    }
}