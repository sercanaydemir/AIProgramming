using Test_Simulation.Locations;
using UnityEngine;

namespace Test_Simulation.WorkSystem.Interfaces
{
    public interface IChef
    {
        public int FoodAmount { get; set; }
        public void Cook();
        public void AddFood(int amount);
        
        public Storage FoodStorage { get; set; }
        
        public Kitchen Kitchen { get; set; }
        
    }
}