using UnityEngine;
using UnityEngine.UI;

namespace GameCore
{
    public class PlayableCell : MonoBehaviour
    {
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

            SetCell(newCell);
        }

        private void SetCell(CardCell cell)
        {
            UnbindUI();

            cell.transform.parent = _playableCardTransform;
            cell.transform.localPosition = Vector3.zero;

            _playableCell = cell;
            BindUI();
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