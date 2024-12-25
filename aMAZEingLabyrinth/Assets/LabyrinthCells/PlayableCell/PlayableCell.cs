using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace GameCore
{
    public class PlayableCell : MonoBehaviour
    {
        public CardCell CardCell => _playableCell;

        [SerializeField]
        private Transform _playableCardTransform;

        [SerializeField]
        private Button _clockwise;

        [SerializeField]
        private Button _anticlockwise;

        private CardCell _playableCell;

        public void ReplacePlayableCell(CardCell newCell, out CardCell oldCell)
        {
            oldCell = _playableCell;

            SetValuesAndView(newCell);
        }

        private void SetValuesAndView(CardCell cell)
        {
            SetCellValues(cell);
            SetupView();
        }

        public void SetCellValues(CardCell cell)
        {
            UnsubscribeUI();

            _playableCell = cell;

            SubscribeUI();
        }

        private void SetupView()
        {
            _playableCell.transform.SetParent(_playableCardTransform);
            _playableCell.transform.localPosition = Vector3.zero;
        }

        public Tween PrepareViewSet(float duration)
        {
            var tween = _playableCell.transform.DOMove(_playableCardTransform.position, duration)
                .OnPlay(SetParentTransform)
                .Pause();

            return tween;
        }

        private void SetParentTransform()
        {
            _playableCell.transform.SetParent(_playableCardTransform);
        }

        private void SubscribeUI()
        {
            _clockwise.onClick.AddListener(RotateClockwise);
            _anticlockwise.onClick.AddListener(RotateAnticlockwise);
        }

        private void UnsubscribeUI()
        {
            _clockwise.onClick.RemoveListener(RotateClockwise);
            _anticlockwise.onClick.RemoveListener(RotateAnticlockwise);
        }

        private void RotateClockwise()
        {
            _playableCell.CellValues.Rotate(-90);
        }

        private void RotateAnticlockwise()
        {
            _playableCell.CellValues.Rotate(90);
        }
    }
}