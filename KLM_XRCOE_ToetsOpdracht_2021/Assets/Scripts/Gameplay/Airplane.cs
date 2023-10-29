using KLM.Airplanes.NavMesh;
using KLM.Airplanes.ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;

namespace KLM.Airplanes.Gameplay
{
    public class Airplane : NumberableObject
    {
        public UnityEvent<Airplane> AirplaneReachedFinalDestination;

        [SerializeField] private AirplaneScriptableObject airplaneValues;
        [SerializeField] private NavMeshLoitering _navMeshLoitering;
        [SerializeField] private Light _light;

        private void Start()
        {
            this.name = $"{airplaneValues.brand} {airplaneValues.type}";
        }

        private void OnEnable()
        {
            _navMeshLoitering.NavMeshReachedFinalDestination.AddListener(OnNavMeshReachedFinalDestination);
        }

        private void OnDisable()
        {
            _navMeshLoitering.NavMeshReachedFinalDestination.RemoveListener(OnNavMeshReachedFinalDestination);
        }

        public void SetFinalPosition(Vector3 targetPosition)
        {
            _navMeshLoitering.SetFinalPosition(targetPosition);
        }

        public void ToggleLights()
        {
            _light.enabled = !_light.enabled;
        }

        public void Reset()
        {
            _navMeshLoitering.Reset();
        }

        private void OnNavMeshReachedFinalDestination()
        {
            if (AirplaneReachedFinalDestination != null)
            {
                AirplaneReachedFinalDestination.Invoke(this);
            }
        }
    }
}
