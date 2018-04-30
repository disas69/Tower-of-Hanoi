using UnityEngine;

namespace Game.Gameplay.RingStructure.Configuration
{
    [CreateAssetMenu(fileName = "RingSettings", menuName = "Settings/RingSettings")]
    public class RingSettings : ScriptableObject
    {
        public float MoveSmoothing = 0.1f;
    }
}