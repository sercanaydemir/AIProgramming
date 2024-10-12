using UnityEngine;

namespace Test_Simulation.Locations
{
    public class HuntingPoint : Location
    {
        public int mealAmount;
        public Transform[] animalLocations;
        public override bool IsAvailable()
        {
            return mealAmount > 0;
        }
        
        public Transform GetRandomLocation()
        {
            return animalLocations[Random.Range(0, animalLocations.Length)];
        } 
    }
}