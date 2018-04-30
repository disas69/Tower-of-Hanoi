using UnityEngine;

namespace Game.Gameplay.TowerStructure.Configuration
{
    [CreateAssetMenu(fileName = "TowerSettings", menuName = "Settings/TowerSettings")]
    public class TowerSettings : ScriptableObject
    {
        public float RingAnchoringStep = 2.5f;
    }
}