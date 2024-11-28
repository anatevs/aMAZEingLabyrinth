using System;
using System.Collections.Generic;
using UnityEngine;
using EventBusNamespace;

namespace EventBusNamespace
{
    public sealed class EventBus
    {
        private readonly Dictionary<Type, IEventBusHandlersList> _events = new();

        private bool _isRunning;

        private readonly Queue<IEvent> _eventsQueue = new();

        public void Subscribe<T>(Action<T> handler)
        {
            Type type = typeof(T);

            if (!_events.ContainsKey(type))
            {
                _events.Add(type, new EventBusHandlersList<T>());
            }

            _events[type].Subscribe(handler);
        }

        public void Unsubscribe<T>(Action<T> handler)
        {
            Type type = typeof(T);

            if (_events.ContainsKey(type))
            {
                _events[type].Unsubscribe(handler);
            }
        }

        public void RaiseEvent<T>(T evnt) where T : IEvent
        {
            if (_isRunning)
            {
                _eventsQueue.Enqueue(evnt);
                return;
            }

            _isRunning = true;

            Type type = evnt.GetType();

            if (!_events.ContainsKey(type))
            {
                Debug.Log($"no subscribers to event {type}");
                _isRunning = false;
                return;
            }

            _events[type].RaiseEvent(evnt);

            _isRunning = false;

            if (_eventsQueue.TryDequeue(out IEvent eventOnQueue))
            {
                RaiseEvent(eventOnQueue);
            }
        }


        private interface IEventBusHandlersList
        {
            public void Subscribe(Delegate handler);

            public void Unsubscribe(Delegate handler);

            public void RaiseEvent<T>(T evnt);
        }


        private sealed class EventBusHandlersList<T> : IEventBusHandlersList
        {
            private readonly List<Delegate> _handlers = new();

            private int _currentIndex = -1;

            public void Subscribe(Delegate handler)
            {
                _handlers.Add(handler);
            }

            public void Unsubscribe(Delegate handler)
            {
                int index = _handlers.IndexOf(handler);
                _handlers.RemoveAt(index);

                if (index <= _currentIndex)
                {
                    _currentIndex--;
                }
            }

            public void RaiseEvent<Tevnt>(Tevnt evnt)
            {
                if (evnt is not T concreteEvent)
                {
                    return;
                }

                for (_currentIndex = 0; _currentIndex < _handlers.Count; _currentIndex++)
                {
                    Action<T> handler = (Action<T>)_handlers[_currentIndex];
                    handler.Invoke(concreteEvent);
                }

                _currentIndex = -1;
            }
        }
    }
}