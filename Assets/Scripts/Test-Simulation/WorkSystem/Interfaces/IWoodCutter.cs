using Test_Simulation.Locations;
using UnityEngine;
using Tree = Test_Simulation.Locations.Tree;

namespace Test_Simulation.WorkSystem.Interfaces
{
    public interface IWoodCutter
    {
        public Tree TargetTree { get; set; }
        public int WoodAmount { get; set; }
        
        public bool CutWood();
        
        public Storage WoodStorage { get; set; }
        
    }
}