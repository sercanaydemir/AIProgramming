using UnityEngine;

namespace Test_Simulation.Locations
{
    public class StoneOre : Location
    {
        public int stoneAmount;
        public Transform[] miningPoint;
        public override bool IsAvailable()
        {
            return stoneAmount > 0;
        }
        
        public Transform GetMiningPoint(Vector3 position)
        {
            Transform closestPoint = null;
            float minDistance = float.MaxValue;
            foreach (var point in miningPoint)
            {
                float distance = Vector3.Distance(position, point.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestPoint = point;
                }
            }

            return closestPoint;
        }
        
        public void MineStone()
        {
            stoneAmount--;
        }
    }
}