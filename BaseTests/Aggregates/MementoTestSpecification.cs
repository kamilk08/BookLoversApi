using System.Threading.Tasks;
using AutoFixture;
using BookLovers.Base.Domain;
using BookLovers.Base.Infrastructure.Ioc;
using BookLovers.Base.Infrastructure.Persistence;
using Ninject;
using NUnit.Framework;

namespace BaseTests.Aggregates
{
    [TestFixture]
    public abstract class MementoTestSpecification<TAggregate, TMemento>
        where TMemento : IMemento<TAggregate>
        where TAggregate : class
    {
        protected TAggregate Aggregate;

        protected ISnapshotMaker SnapshotMaker;
        protected ICompositionRoot CompositionRoot;
        protected Fixture Fixture;

        protected abstract void InitializeRoot();

        protected abstract Task PrepareData();

        protected abstract void SetCompositionRoot();

        [SetUp]
        protected async Task BeforeEachTest()
        {
            Fixture = new Fixture();

            SetCompositionRoot();
            InitializeRoot();

            SnapshotMaker = CompositionRoot.Kernel.Get<ISnapshotMaker>();

            await PrepareData();
        }

        [TearDown]
        protected void AfterEachTest()
        {
        }
    }
}