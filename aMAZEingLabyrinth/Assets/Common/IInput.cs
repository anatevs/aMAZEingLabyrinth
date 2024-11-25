using System.Collections;
using UnityEngine;

namespace Assets.Common
{
    public interface IInput
    {
        public Vector3 GetMousePos();

        public bool IsMouseClicked();
    }
}