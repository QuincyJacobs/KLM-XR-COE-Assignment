using KLM.Airplanes.Gameplay;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KLM.Airplanes
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private List<NumberableObject> _hangarNumbers;
        [SerializeField] private List<Airplane> _airplanes;

        private void Start()
        {
            var airplaneNumbers = new List<NumberableObject>();

            foreach (Airplane airplane in _airplanes)
            {
                airplaneNumbers.Add(airplane);
            }

            RandomizeNumbers(_hangarNumbers);
            RandomizeNumbers(airplaneNumbers);
        }

        private void OnEnable()
        {
            foreach (Airplane airplane in _airplanes)
            {
                airplane.AirplaneReachedFinalDestination.AddListener(OnAirplaneReachedFinalDestination);
            }
        }

        private void OnDisable()
        {
            foreach (Airplane airplane in _airplanes)
            {
                airplane.AirplaneReachedFinalDestination.RemoveListener(OnAirplaneReachedFinalDestination);
            }
        }

        /// <summary>
        /// Park airplanes in the hangar with matching number. 
        /// </summary>
        /// <remarks>Parks at 0,0,0 if there is no hangar found.</remarks>
        public void ParkAirplanes()
        {
            foreach (var airplane in _airplanes)
            {
                var number = airplane.Number;
                Vector3 targetPosition = _hangarNumbers.FirstOrDefault(x => x.Number == number)?.transform.position ?? Vector3.zero;

                airplane.SetFinalPosition(targetPosition);
            }
        }

        /// <summary>
        /// Toggle the lights of all the airplanes
        /// </summary>
        public void ToggleAirplaneLights()
        {
            foreach (var airplane in _airplanes)
            {
                airplane.ToggleLights();
            }
        }

        /// <summary>
        /// Shuffles the numbers on the hangars and resets the airplanes so they start loitering again.
        /// </summary>
        public void ShuffleHangerNumbersAndResetAirplanes()
        {
            RandomizeNumbers(_hangarNumbers);

            foreach (var hangar in _hangarNumbers)
            {
                hangar.SetTextColor(Color.red);
            }

            foreach(var airplane in _airplanes)
            {
                airplane.Reset();
            }
        }

        /// <summary>
        /// React to the airplane reaching its final destination by setting the hangar number color to green.
        /// </summary>
        /// <param name="airplane"></param>
        private void OnAirplaneReachedFinalDestination(Airplane airplane)
        {
            _hangarNumbers.FirstOrDefault(x => x.Number == airplane.Number)?.SetTextColor(Color.green);
        }

        /// <summary>
        /// Randomizes the numbers on the hangars
        /// </summary>
        private void RandomizeNumbers(IList<NumberableObject> numberableList)
        {
            // The poor-mans Fisher - Yates shuffle
            var rng = new System.Random();
            var intList = Enumerable.Range(1, numberableList.Count + 1).ToArray();
            var amount = numberableList.Count;
            while (amount > 0)
            {
                amount--;
                int k = rng.Next(amount + 1);
                int value = intList[k];
                intList[k] = intList[amount];
                intList[amount] = value;
                numberableList[amount].SetNumber(value);
            }
        }
    }
}
