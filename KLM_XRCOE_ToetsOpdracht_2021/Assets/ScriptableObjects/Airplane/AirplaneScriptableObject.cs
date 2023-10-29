using UnityEngine;

namespace KLM.Airplanes.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Scriptable Object/Airplane")]
    public class AirplaneScriptableObject : ScriptableObject
    {
        public string type;
        public string brand;
    }
}
