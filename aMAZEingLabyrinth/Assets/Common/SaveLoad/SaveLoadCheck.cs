using GameCore;
using Newtonsoft.Json;
using System.Collections;
using UnityEngine;
using VContainer;

namespace SaveLoadNamespace
{
    public class SaveLoadCheck : MonoBehaviour
    {

        private MovableCellsManager _mngr;
        private MovingCellsSaveLoad _svld;

        [Inject]
        public void Construct(MovableCellsManager mngr, MovingCellsSaveLoad svld)
        {
            _mngr = mngr;
            _svld = svld;
        }

        private void Awake()
        {
            _svld.TestLoadDefault();
        }


    }
}