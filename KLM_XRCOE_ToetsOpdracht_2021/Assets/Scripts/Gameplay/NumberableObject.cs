using TMPro;
using UnityEngine;

namespace KLM.Airplanes.Gameplay
{
    public class NumberableObject : MonoBehaviour
    {
        [SerializeField] private TextMeshPro _numberTextMesh;

        public int Number { get; private set; }

        public void SetNumber(int number)
        {
            Number = number;
            _numberTextMesh.text = Number.ToString();
        }

        public void SetTextColor(Color color)
        {
            _numberTextMesh.color = color;
        }
    }
}
