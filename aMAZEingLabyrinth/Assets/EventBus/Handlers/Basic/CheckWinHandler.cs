using EventBusNamespace;
using System.Collections;
using UnityEngine;

namespace EventBusNamespace
{
    public sealed class CheckWinHandler : BaseHandler<CheckWinEvent>
    {
        public CheckWinHandler(EventBus eventBus) : base(eventBus)
        {
            
        }

        protected override void RaiseEvent(CheckWinEvent evnt)
        {

        }
    }
}