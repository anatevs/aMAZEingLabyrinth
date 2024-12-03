using UnityEngine;

namespace GameModules
{
    public interface IInput
    {
        public Vector3 GetMousePos();

        public bool IsMouseClicked();
    }
}