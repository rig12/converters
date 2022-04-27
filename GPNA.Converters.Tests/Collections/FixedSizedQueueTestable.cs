namespace GPNA.Common.Tests.Collections
{
    #region Using
    using Common.Collections;
    #endregion Using

    public class FixedSizedQueueTestable<TEntity> : FixedSizedQueue<TEntity> where TEntity : class
    {
        public FixedSizedQueueTestable(int limit = 100) : base(limit)
        { }
    }
}
