using UnityEngine;

namespace Test_Simulation.Locations
{
    public class IronOre : Location
    {
        public int ironAmount;
        public override bool IsAvailable()
        {
            return ironAmount>0;
        }

        public void Mine()
        {
            ironAmount--;
        }
        
        
    }
}