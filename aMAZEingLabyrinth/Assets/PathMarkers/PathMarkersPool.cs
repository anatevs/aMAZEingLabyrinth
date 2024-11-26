using System.Collections.Generic;
using UnityEngine;

namespace GameCore
{
    public class PathMarkersPool : MonoBehaviour
    {
        [SerializeField]
        private Transform _pathMarkersTransform;

        [SerializeField]
        private GameObject _pathMarker;

        [SerializeField]
        private GameObject[] _initMarkers;

        private readonly Queue<GameObject> _pool = new();

        private readonly List<GameObject> _spawnedMarkers = new();

        private void Start()
        {
            foreach (var marker in _initMarkers)
            {
                _pool.Enqueue(marker);
            }
        }
        public GameObject Spawn(int xPos, int yPos)
        {
            var position = new Vector3(xPos, yPos,
                _pathMarker.transform.position.z);

            GameObject marker;

            if (_pool.TryDequeue(out marker))
            {
                marker.SetActive(true);
            }
            else
            {
                marker = Instantiate(_pathMarker, position,
                    Quaternion.identity, _pathMarkersTransform);
            }

            marker.transform.localPosition = position;

            _spawnedMarkers.Add(marker);

            return marker;
        }

        public void UnspawnAll()
        {
            for (int i = 0; i < _spawnedMarkers.Count; i++)
            {
                _pool.Enqueue(_spawnedMarkers[i]);
                _spawnedMarkers[i].SetActive(false);
            }

            _spawnedMarkers.Clear();
        }
    }
}