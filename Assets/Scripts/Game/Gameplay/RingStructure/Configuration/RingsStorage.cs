using System.Collections.Generic;
using UnityEngine;

namespace Game.Gameplay.RingStructure.Configuration
{
    [CreateAssetMenu(fileName = "RingsStorage", menuName = "Storages/RingsStorage")]
    public class RingsStorage : ScriptableObject
    {
        public List<Ring> RingsPrefabs = new List<Ring>();

        public List<Ring> GetRingsPrefabs()
        {
            var rings = new List<Ring>(RingsPrefabs);
            rings.Sort((x, y) => x.SizeIndex.CompareTo(y.SizeIndex));

            return rings;
        }
    }
}