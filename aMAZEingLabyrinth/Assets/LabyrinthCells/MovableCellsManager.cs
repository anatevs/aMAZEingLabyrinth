using SaveLoadNamespace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

namespace GameCore
{
    public class MovableCellsManager
    {
        public IEnumerable<CardCell> MovableCells => _movableCells;

        public CardCell PlayableCell => _playableCell;

        public CellsData InitCellsData => _initCellsData;

        private CellsData _initCellsData;

        private readonly List<CardCell> _movableCells = new();

        private CardCell _playableCell;

        public void AddMovableData(OneCellData cellData)
        {
            _initCellsData.AddMovableCell(cellData);
        }

        public void SetPlayableData(OneCellData cellData)
        {
            _initCellsData.SetPlayableCell(cellData);
        }

        public void SetPlayable(CardCell playableCell)
        {
            _playableCell = playableCell;
        }

        public void AddMovable(CardCell cardCell)
        {
            _movableCells.Add(cardCell);
        }

        public void ClearMovable()
        {
            _movableCells.Clear();
        }
    }
}