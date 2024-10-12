using System;
using NPCs;
using Test_Simulation.WorkSystem;
using Test_Simulation.WorkSystem.Interfaces;
using UnityEngine;

namespace Test_Simulation.Locations
{
    public class CookPoint : MonoBehaviour
    {
        public Kitchen kitchen;
        public bool IsAvailable = true;
        // private void OnTriggerEnter(Collider other)
        // {
        //     if(other.TryGetComponent(out IChef chef))
        //     {
        //         if (IsAvailable)
        //         {
        //             IsAvailable = false;
        //             chef.Cook();
        //         }
        //     }
        // }
        //
        // private void OnTriggerExit(Collider other)
        // {
        //     if(other.TryGetComponent(out AgentAI agent))
        //     {
        //         if (agent.workerType == WorkerType.Chef)
        //         {
        //             IsAvailable = true;
        //         }
        //     }
        // }
    }
}