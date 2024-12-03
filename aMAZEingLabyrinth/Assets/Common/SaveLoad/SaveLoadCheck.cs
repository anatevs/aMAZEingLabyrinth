using GameCore;
using Newtonsoft.Json;
using System.Collections;
using UnityEngine;
using VContainer;

namespace SaveLoadNamespace
{
    public class SaveLoadCheck : MonoBehaviour
    {
        private MovingCellsSaveLoad _svld;

        //[Inject]
        public void Construct(MovingCellsSaveLoad svld)
        {
            _svld = svld;

            _svld.TestLoadDefault();
        }
    }
}