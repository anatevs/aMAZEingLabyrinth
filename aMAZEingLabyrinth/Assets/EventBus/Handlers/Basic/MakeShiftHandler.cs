using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EventBusNamespace
{
    public class MakeShiftHandler : BaseHandler<MakeShiftEvent>
    {
        public MakeShiftHandler(EventBus eventBus) : base(eventBus)
        {
        }

        protected override void RaiseEvent(MakeShiftEvent evnt)
        {
            evnt.CellsLabyrinth.MakeShift(evnt.Row, evnt.Col, out var oppositeIndex);

            evnt.ShiftArrowsService.ChangeDisabledArrow(oppositeIndex);

            evnt.ShiftArrowsService.DisableAllArrows();
        }
    }
}