namespace GPNA.Common.Tests.Services
{
    #region Using
    using NUnit.Framework;
    using GPNA.Common.Interfaces;
    using Moq;
    using System.Linq;
    #endregion Using

    public class ObservableModuleUnitTests
    {
        #region Fields
        private ObservableModuleTestable _observableModuleTestable;
        #endregion Fields


        #region Initialization
        [SetUp]
        public void Initialization()
        {
            _observableModuleTestable = new ObservableModuleTestable();
        }
        #endregion Initialization


        #region Methods
        [Test]
        public void Subscrube_ResultMustBe_true()
        {
            var module = new Mock<IObserverModule>();
            _observableModuleTestable.Subscrube<object>(module.Object);
            var modules = _observableModuleTestable.Types.ContainsKey(typeof(object))
                ? _observableModuleTestable.Types[typeof(object)] : Enumerable.Empty<IObserverModule>();
            Assert.IsTrue(modules.FirstOrDefault(x => x == module.Object) != default);
        }

        [Test]
        public void Unsubscrube_ResultMustBe_true()
        {
            var module = new Mock<IObserverModule>();
            _observableModuleTestable.Subscrube<object>(module.Object);
            var modules = _observableModuleTestable.Types.ContainsKey(typeof(object))
                ? _observableModuleTestable.Types[typeof(object)] : Enumerable.Empty<IObserverModule>();
            _observableModuleTestable.Unsubscrube(module.Object);
            Assert.IsTrue(modules.FirstOrDefault(x => x == module.Object) == default);
        }

        [Test]
        public void Subscrube_on_two_types_ResultMustBe_more_than_one_types()
        {
            var module = new Mock<IObserverModule>();
            _observableModuleTestable.Subscrube<object>(module.Object);
            _observableModuleTestable.Subscrube<string>(module.Object);
            Assert.IsTrue(_observableModuleTestable.Types.Count > 1);
        }

        [Test]
        public void Subscrube_on_two_types_and_unsubscribe_ResultMustBe_more_than_one_types()
        {
            var module = new Mock<IObserverModule>();
            _observableModuleTestable.Subscrube<object>(module.Object);
            _observableModuleTestable.Subscrube<string>(module.Object);
            _observableModuleTestable.Unsubscrube(module.Object);
            Assert.IsTrue(_observableModuleTestable.Types.Count > 1);
        }

        [Test]
        public void Subscrube_on_two_types_ResultMustBe_more_than_one_types_with_modules()
        {
            var module = new Mock<IObserverModule>();
            _observableModuleTestable.Subscrube<object>(module.Object);
            _observableModuleTestable.Subscrube<string>(module.Object);
            Assert.IsTrue(_observableModuleTestable.Types
                .Where(x => x.Value.Count() > 0).Count() > 1);
        }

        [Test]
        public void Notify_ResultMustBe_Execute_add_method_once()
        {
            var obj = new object();
            var module = new Mock<IObserverModule>();
            _observableModuleTestable.Subscrube<object>(module.Object);
            _observableModuleTestable.Notify(obj);
            module.Verify(c => c.Add(obj), Times.Once());
        }

        [Test]
        public void Notify_ResultMustBe_Execute_add_method_never()
        {
            var str = "test";
            var module = new Mock<IObserverModule>();
            _observableModuleTestable.Subscrube<object>(module.Object);
            _observableModuleTestable.Notify(str);
            module.Verify(c => c.Add(str), Times.Never());
        }
        #endregion Methods
    }
}
