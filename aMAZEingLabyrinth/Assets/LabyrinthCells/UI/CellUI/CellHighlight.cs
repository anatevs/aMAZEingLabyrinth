using GamePipeline;
using UnityEngine;
using VContainer;

namespace GameCore
{
    public sealed class CellHighlight : MonoBehaviour
    {
        public Vector3 CurrentPosition => _currentPos;

        [SerializeField]
        private CellsLabyrinth _labyrinth;

        [SerializeField]
        CellHighlightView _view;

        private bool _isActive;

        private Vector3 _currentPos;

        private Vector3 _outPos = new(-1, -1, 0);

        private TurnPipeline _turnPipeline;

        [Inject]
        public void Construct(TurnPipeline turnPipeline)
        {
            _turnPipeline = turnPipeline;
        }

        private void OnEnable()
        {
            _currentPos = _outPos;

            _view.OnMouseEnter += SetHighlight;

            _view.OnMouseExit += UnhighlightReward;

            _view.OnCellClicked += ClickOnCell;
        }

        private void OnDisable()
        {
            _view.OnMouseEnter -= SetHighlight;

            _view.OnMouseExit -= UnhighlightReward;

            _view.OnCellClicked -= ClickOnCell;
        }

        public void SetActive(bool isActive)
        {
            _isActive = isActive;
            _view.SetActive(isActive);
        }

        private void SetHighlight(Vector3 mousePos)
        {
            if (_isActive)
            {
                var pos = _labyrinth.GetCellCenterCoordinates(mousePos);

                if (pos != _currentPos)
                {
                    if (_currentPos != _outPos)
                    {
                        _labyrinth.LayerUpRewardSprite(_currentPos, false);
                    }
                    _labyrinth.LayerUpRewardSprite(pos, true);
                }

                _currentPos = pos;

                _view.SetHighlight(pos);
            }
        }

        private void UnhighlightReward()
        {
            _labyrinth.LayerUpRewardSprite(_currentPos, false);
            _currentPos = _outPos;
        }

        private void ClickOnCell()
        {
            if (_isActive)
            {
                _turnPipeline.Run();
            }
        }
    }
}