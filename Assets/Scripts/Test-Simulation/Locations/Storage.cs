using UnityEngine;

namespace Test_Simulation.Locations
{
    public class Storage : MonoBehaviour
    {
        public ResourceType resourceType;
        public int resourceAmount;
        public int maxResourceAmount;
        
        public void AddResource(int amount)
        {
            resourceAmount += amount;
            if (resourceAmount > maxResourceAmount)
            {
                resourceAmount = maxResourceAmount;
            }
        }
        public bool RemoveResource(int amount)
        {
            if (resourceAmount >= amount)
            {
                resourceAmount -= amount;
                return true;
            }
            return false;
        }
        
        public bool IsFull()
        {
            return resourceAmount >= maxResourceAmount;
        }
        
        public bool IsEmpty()
        {
            return resourceAmount <= 0;
        }
    }

    public enum ResourceType
    {
        None = 0,
        Food = 1,
        Wood = 2,
        Stone = 3,
        Iron = 4
        
    }
}