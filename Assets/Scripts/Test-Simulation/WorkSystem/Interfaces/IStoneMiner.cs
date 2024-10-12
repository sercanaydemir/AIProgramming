using Test_Simulation.Locations;

namespace Test_Simulation.WorkSystem.Interfaces
{
    public interface IStoneMiner
    {
        public int StoneAmount { get; set; }
        
        public StoneOre StoneOre { get; set; }
        public Storage StoneStorage { get; set; }
        
        public bool MineStone();
    }
}