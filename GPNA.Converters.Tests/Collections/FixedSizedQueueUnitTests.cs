namespace GPNA.Common.Tests.Collections
{
    #region Using
    using GPNA.Common.Messages;
    using NUnit.Framework;
    using System;
    using System.Linq;
    #endregion Using

    public class FixedSizedQueueUnitTests
    {
        #region Fields
        private FixedSizedQueueTestable<MessageStatus> _queue;
        private const int LIMIT = 1;
        private readonly Func<DateTime> GetCurrentDatetime = () => DateTime.Now;
        #endregion Fields


        #region Initialization
        [SetUp]
        public void Initialization()
        {
            _queue = new FixedSizedQueueTestable<MessageStatus>(0);
        }
        #endregion Initialization


        #region Methods
        [Test]
        public void Clear_ResultMustBe_true()
        {
            string firstMessage = "First",
               secondMessage = "Second";
            _queue.Limit = LIMIT;
            _queue.Enqueue(new MessageStatus()
            {
                DateTime = GetCurrentDatetime(),
                Message = firstMessage
            });
            _queue.Enqueue(new MessageStatus()
            {
                DateTime = GetCurrentDatetime(),
                Message = secondMessage
            });
            _queue.Clear();
            Assert.IsTrue(_queue.First == default
                && _queue.Last == default
                && _queue.AsEnumerable().Count() == 0);
        }

        [Test]
        public void Enqueue_ResultMustBe_true()
        {
            string firstMessage = "First",
                secondMessage = "Second";
            _queue.Limit = LIMIT;
            _queue.Enqueue(new MessageStatus()
            {
                DateTime = GetCurrentDatetime(),
                Message = firstMessage
            });
            _queue.Enqueue(new MessageStatus()
            {
                DateTime = GetCurrentDatetime(),
                Message = secondMessage
            });
            Assert.IsTrue(_queue.First?.Message == secondMessage
                && _queue.Last?.Message == secondMessage
                && _queue.AsEnumerable().Count() == LIMIT);
        }
        #endregion Methods
    }
}
