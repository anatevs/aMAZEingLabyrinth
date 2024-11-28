using System;
using UnityEngine;

namespace GamePipeline
{
    public abstract class Task
    {
        private event Action Callback;

        public void Run(Action FinishCallback)
        {
            Callback = FinishCallback;
            OnRun();
        }

        protected abstract void OnRun();

        public void Finish()
        {
            if (Callback != null)
            {
                Action CachedCallback = Callback;
                Callback = null;

                CachedCallback.Invoke();
            }

            OnFinished();
        }

        protected virtual void OnFinished()
        {
        }
    }
}