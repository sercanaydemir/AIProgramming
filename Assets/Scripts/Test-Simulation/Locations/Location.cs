using UnityEngine;

namespace Test_Simulation.Locations
{
    public abstract class Location : MonoBehaviour
    {
        public abstract bool IsAvailable();
    }
    
    public enum WorkLocation
    {
        None = 0,
        HuntingPoint = 1,
        IronMine = 2,
        StoneMine = 3,
        Kitchen = 4, 
        Storage = 5,
        Tree = 6,
        GatheringPoint = 7
        
    }
}