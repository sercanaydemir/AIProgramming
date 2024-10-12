using UnityEngine;

namespace Test_Simulation.Locations
{
    public class Fruit : Location
    {
        public int amount;

        public override bool IsAvailable()
        {
            return amount > 0;
        }
        
        public void Harvest()
        {
            amount--;
        }
    }
}