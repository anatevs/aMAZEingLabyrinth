using DG.Tweening;
using UnityEngine;

namespace GameCore
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class PathMarkerView : MonoBehaviour
    {
        public float FadeDuration => _fadeDuration;

        [SerializeField]
        private float _fadeDuration = 0.2f;

        private SpriteRenderer _spriteRenderer;

        private readonly float _invisFade = 0;
        private readonly float _visFade = 1f;

        private void OnEnable()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();

            var clr = _spriteRenderer.color;
            clr.a = _invisFade;
            _spriteRenderer.color = clr;
        }

        public Tween Show(float fadeDuration)
        {
            return _spriteRenderer.DOFade(_visFade, fadeDuration)
                .Pause();
        }

        public Tween Hide(float fadeDuration)
        {
            return _spriteRenderer.DOFade(_invisFade, fadeDuration)
                .Pause();
        }
    }
}