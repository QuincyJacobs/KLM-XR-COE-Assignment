using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;
using UnityEngine.Events;

namespace KLM.Airplanes.NavMesh
{
    public class NavMeshLoitering : MonoBehaviour
    {
        public UnityEvent NavMeshReachedFinalDestination;

        [SerializeField] private NavMeshAgent _agent;

        [SerializeField] private float _loiterRadius = 5.0f;
        [SerializeField] private float _maxBreakAfterArrivalInSeconds = 2.0f;
        [SerializeField] private float _minBreakAfterArrivalInSeconds = 0.5f;

        private float _breakAfterArrivalInSeconds = 0.0f;
        private float _timeSinceArrival = 0.0f;
        private bool _isFinalDestination;
        private bool _isActive = true;

        private void Start()
        {
            Assert.IsFalse(_minBreakAfterArrivalInSeconds > _maxBreakAfterArrivalInSeconds, $"Min break after arrival should be equal to or lower than than Max break after arrival.\r\n Min: {_minBreakAfterArrivalInSeconds}, Max: {_maxBreakAfterArrivalInSeconds}");
        }

        private void Update()
        {
            if (!_isActive)
            {
                return;
            }

            // Check if we are very close to the destination
            if (!_agent.pathPending && _agent.remainingDistance < 0.5f)
            {
                // Notify users if the final destination is reached and disable Update
                if (_isFinalDestination)
                {
                    _isActive = false;
                    if(NavMeshReachedFinalDestination != null)
                    {
                        NavMeshReachedFinalDestination.Invoke();
                    }
                    return;
                }

                // Wait for an x amount of time to make the movement appear more "random".
                _timeSinceArrival += Time.deltaTime;
                if(_timeSinceArrival >= _breakAfterArrivalInSeconds)
                {
                    SelectRandomDestination();
                    
                    // Reset time and get a new random break amount
                    _timeSinceArrival = 0.0f;
                    _breakAfterArrivalInSeconds = Random.Range(_minBreakAfterArrivalInSeconds, _maxBreakAfterArrivalInSeconds);
                }
            }
        }

        /// <summary>
        /// Sets the final destination for the airplane.
        /// </summary>
        /// <param name="targetPosition"></param>
        public void SetFinalPosition(Vector3 targetPosition)
        {
            _agent.SetDestination(targetPosition);
            _isFinalDestination = true;
        }

        public void Reset()
        {
            _isFinalDestination = false;
            _isActive = true;
        }

        /// <summary>
        /// Selects a random destination in a max radius around the <see cref="NavMeshAgent"/>.
        /// </summary>
        private void SelectRandomDestination()
        {
            Vector3 targetPosition = transform.position;
            targetPosition.x += 2 * _loiterRadius * (Random.value - 0.5f);
            targetPosition.z += 2 * _loiterRadius * (Random.value - 0.5f);

            _agent.SetDestination(targetPosition);
        }
    }
}
