using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EventBusNamespace
{
    public sealed class MoveThroughPathVisualHandler : BaseHandler<MoveThroughPathEvent>
    {
        public MoveThroughPathVisualHandler(EventBus eventBus) : base(eventBus)
        {
        }

        protected override void RaiseEvent(MoveThroughPathEvent evnt)
        {
            //async evnt.Player.MovePath add to visual pipeline
        }
    }
}