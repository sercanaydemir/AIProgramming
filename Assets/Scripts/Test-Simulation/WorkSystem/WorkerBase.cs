using UnityEngine;

namespace Test_Simulation.WorkSystem
{
    public abstract class WorkerBase
    {
        public abstract void Work();
    }
    
    public enum WorkerType
    {
        Hunter,
        Miner,
        Chef,
        WoodCutter,
        Gatherer
    }
}