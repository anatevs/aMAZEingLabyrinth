using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GameUI
{
    public class NoPathMessageWindow : MonoBehaviour
    {
        [SerializeField]
        private GameObject _window;

        [SerializeField]
        private float _activeTime = 0.5f;

        public async UniTask SetActiveTask()
        {
            _window.SetActive(true);

            await UniTask.WaitForSeconds(_activeTime);

            _window.SetActive(false);
        }
    }
}