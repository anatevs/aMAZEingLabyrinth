using System;
using VContainer.Unity;

namespace EventBusNamespace
{
    public abstract class BaseHandler<T> : IInitializable, IDisposable
    {
        public EventBus EventBus => _eventBus;

        private readonly EventBus _eventBus;

        protected BaseHandler(EventBus eventBus)
        {
            _eventBus = eventBus;
        }

        void IInitializable.Initialize()
        {
            _eventBus.Subscribe<T>(RaiseEvent);
        }

        void IDisposable.Dispose()
        {
            _eventBus.Unsubscribe<T>(RaiseEvent);
        }

        protected abstract void RaiseEvent(T evnt);
    }
}