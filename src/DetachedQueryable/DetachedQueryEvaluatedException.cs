using System;

namespace DetachedQueryable
{
    public class DetachedQueryEvaluatedException : InvalidOperationException
    {
        public DetachedQueryEvaluatedException()
            : base("Invalid attempt to evaluate a DetachedQuery before attaching a data store")
        { }
    }
}