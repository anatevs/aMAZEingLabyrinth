using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore
{
    public class PathMarkersPool : MonoBehaviour
    {
        [SerializeField]
        private Transform _pathMarkersTransform;

        [SerializeField]
        private PathMarkerView _markerPrefab;

        [SerializeField]
        private PathMarkerView[] _initMarkers;

        [SerializeField]
        private float _fadeDuration = 0.1f;

        private readonly Queue<PathMarkerView> _pool = new();

        private readonly List<PathMarkerView> _spawnedMarkers = new();

        private void Start()
        {
            foreach (var marker in _initMarkers)
            {
                _pool.Enqueue(marker);
            }
        }
        public Tween Spawn(int xPos, int yPos)
        {
            var position = new Vector3(xPos, yPos,
                _markerPrefab.transform.position.z);


            if (!_pool.TryDequeue(out PathMarkerView marker))
            {
                marker = Instantiate(_markerPrefab, position,
                    Quaternion.identity, _pathMarkersTransform);
            }

            marker.transform.localPosition = position;

            _spawnedMarkers.Add(marker);

            return marker.Show(_fadeDuration);
        }

        public void UnspawnAll()
        {
            for (int i = 0; i < _spawnedMarkers.Count; i++)
            {
                _pool.Enqueue(_spawnedMarkers[i]);
                _spawnedMarkers[i].Hide(_fadeDuration).Play();
            }

            _spawnedMarkers.Clear();
        }
    }
}