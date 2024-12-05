using SaveLoadNamespace;
using System;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

namespace GameCore
{
    public sealed class ShiftArrowsService : MonoBehaviour
    {
        public event Action<int, int> OnClick;

        [SerializeField]
        private ShiftArrow[] _arrows;

        private (int, int) _invalidIndex;

        private (int, int) _disabledIndex;

        private bool _areActive;

        private readonly Dictionary<(int, int), ShiftArrow> _indexArrows = new();

        private ShiftArrowsDataConnector _dataConnector;

        [Inject]
        public void Construct(ShiftArrowsDataConnector dataConnector)
        {
            _dataConnector = dataConnector;

            _dataConnector.OnArrowsInfoRequested += SendArrowsToConnector;
        }

        private void Start()
        {
            foreach (ShiftArrow arrow in _arrows)
            {
                _indexArrows.Add(arrow.Index, arrow);

                arrow.OnShift += Click;
            }

            //StartGameInitArrows();

            InitArrows(_dataConnector.Data);
        }

        private void OnDisable()
        {
            foreach (ShiftArrow arrow in _arrows)
            {
                arrow.OnShift -= Click;
            }

            _dataConnector.OnArrowsInfoRequested -= SendArrowsToConnector;
        }

        //private void InitArrows()
        //{
        //    _disabledIndex = _initIndex;
        //    DisableAllArrows();
        //}

        private void InitArrows(ShiftArrowsData data)
        {
            _disabledIndex = data.DisabledIndex;

            _invalidIndex = data.InvalidIndex;

            _areActive = data.AreArrowsActive;

            if (!_areActive)
            {
                DisableAllArrows();
            }
            else
            {
                EnableAllActiveArrows();
            }
        }


        private void Click(int row, int column)
        {
            OnClick?.Invoke(row, column);
        }

        public void ChangeDisabledArrow((int, int) index)
        {
            var newDisabled = _indexArrows[index];

            if (_disabledIndex != _invalidIndex)
            {
                var prevDisabled = _indexArrows[_disabledIndex];
                prevDisabled.ActivateButton();
            }

            newDisabled.DisactivateButton();

            _disabledIndex = index;
        }

        public void DisableAllArrows()
        {
            foreach(var arrow in _arrows)
            {
                arrow.DisactivateButton();
            }

            _areActive = false;
        }

        public void EnableAllActiveArrows()
        {
            foreach (var index in _indexArrows.Keys)
            {
                if (index != _disabledIndex)
                {
                    _indexArrows[index].ActivateButton();
                }
                else
                {
                    _indexArrows[index].DisactivateButton();
                }
            }

            _areActive = true;
        }

        private void SendArrowsToConnector()
        {
            _dataConnector.SetArrows(_disabledIndex, _areActive);
        }
    }
}