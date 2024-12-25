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

            SetCellAndView(newCell);
        }

        public void Clear()
        {
            Destroy(this);
        }

        public void SetCellAndView(CardCell cell)
        {
            SetCell(cell);
            SetCellView();
        }

        public void SetCell(CardCell cell)
        {
            UnbindUI();

            _playableCell = cell;

            BindUI();
        }

        public void SetCellView()
        {
            _playableCell.transform.SetParent(_playableCardTransform);
            _playableCell.transform.localPosition = Vector3.zero;
        }

        private void BindUI()
        {
            _clockwise.onClick.AddListener(RotateClockwise);
            _anticlockwise.onClick.AddListener(RotateAnticlockwise);
        }

        private void UnbindUI()
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