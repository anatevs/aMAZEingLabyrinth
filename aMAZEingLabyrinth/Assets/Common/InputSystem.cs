using Assets.Common;
using UnityEngine;

namespace GameCore
{
    public sealed class InputSystem : IInput
    {
        private readonly Camera _camera;

        public InputSystem()
        {
            _camera = Camera.main;
        }

        public Vector3 GetMousePos()
        {
            var point = Input.mousePosition;

            return _camera.ScreenToWorldPoint(
                new Vector3(point.x, point.y, _camera.nearClipPlane));
        }
    }
}