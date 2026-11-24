using UnityEngine;
using UnityEngine.EventSystems;

namespace GameModules
{
    public sealed class InputHandler : IInput
    {
        private readonly PlayerActions _inputActions;

        private readonly Camera _camera;

        public InputHandler()
        {
            _inputActions = new PlayerActions();

            _camera = Camera.main;

            _inputActions.Enable();
        }

        public void OnDisable()
        {
            _inputActions.Disable();
        }

        public Vector3 GetMousePos()
        {
            var point = _inputActions.Game.Position.ReadValue<Vector2>();

            return _camera.ScreenToWorldPoint(
                new Vector3(point.x, point.y, _camera.nearClipPlane));
        }

        public bool IsMouseClicked()
        {
            return _inputActions.Game.Click.WasPressedThisFrame();
        }

#if UNITY_EDITOR
        public Vector3 MousePosition => _inputActions.Game.Position.ReadValue<Vector2>();
#endif
    }
}