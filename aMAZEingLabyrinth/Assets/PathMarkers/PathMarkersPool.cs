using System.Collections.Generic;
using UnityEngine;

namespace GameCore
{
    public class PathMarkersPool : MonoBehaviour
    {
        public float SpawnedTime
        {
            get => _spawnedTime;
            set => _spawnedTime = value;
        }

        [SerializeField]
        private Transform _pathMarkersTransform;

        [SerializeField]
        private GameObject _pathMarker;

        [SerializeField]
        private GameObject[] _initMarkers;

        [SerializeField]
        private float _chillTime;

        private float _spawnedTime;

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


            if (_pool.TryDequeue(out GameObject marker))
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

        private void Update()
        {
            if (_spawnedTime > 0)
            {
                if (Time.time - _spawnedTime > _chillTime)
                {
                    UnspawnAll();
                }
            }
        }

        private void UnspawnAll()
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