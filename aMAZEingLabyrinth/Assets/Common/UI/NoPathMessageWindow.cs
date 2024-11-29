using UnityEngine;

namespace GameUI
{
    public class NoPathMessageWindow : MonoBehaviour
    {
        [SerializeField]
        private GameObject _window;

        [SerializeField]
        private float _activeTime;

        private float _counter = 0;

        public void SetActive()
        {
            _window.SetActive(true);
        }

        private void Update()
        {
            if (_window.activeSelf)
            {
                _counter += Time.deltaTime;

                if (_counter >= _activeTime)
                {
                    _counter = 0;
                    _window.SetActive(false);
                }
            }
        }
    }
}