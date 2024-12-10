using UnityEngine;

namespace GameModules
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

        public bool IsMouseClicked()
        {
            return Input.GetMouseButtonDown(0);
        }

#if UNITY_EDITOR
        public Vector3 MousePosition => Input.mousePosition;
#endif
    }
}