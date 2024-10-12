using BehaviourTree.Core;
using NPCs;
using Test_Simulation.Locations;
using Test_Simulation.Managers;
using Test_Simulation.WorkSystem.Interfaces;
using UnityEngine;

namespace Test_Simulation.WorkSystem
{
    public class Miner : AgentAI, IStoneMiner
    {
        public int StoneAmount { get; set; }
        public StoneOre StoneOre { get; set; }
        public Storage StoneStorage { get; set; }
        public Transform MiningPoint { get; set; }
        
        public float MiningSpeed = 1f;  
        public float currentMiningTime = 0f;

        protected override void Start()
        {
            base.Start();
            StoneOre = LocationManager.Instance.stoneOre;
            StoneStorage = LocationManager.Instance.stoneStorage;
            currentMiningTime = MiningSpeed;
        }
        
        public bool MineStone()
        {
            if (currentMiningTime > 0)
            {
                currentMiningTime -= 0.25f;
                return false;
            }
            
            if (StoneAmount >= 3)
            {
                return true;
            }
            
            StoneAmount++;
            StoneOre.MineStone();
            currentMiningTime = MiningSpeed;
            return false;
            
        }
        
        protected override BT_Sequence BuildWorkSequence()
        {
            BT_Sequence sequence = new BT_Sequence("WorkSequence");
            BT_Leaf goToStoneOre = new BT_Leaf("GoToStoneOre", GoToStoneOre);
            BT_Leaf mineStone = new BT_Leaf("MineStone", Work);
            BT_Leaf goToStorage = new BT_Leaf("GoToStorage", GoToStorage);
            sequence.AddChild(goToStoneOre);
            sequence.AddChild(mineStone);
            sequence.AddChild(goToStorage);
            return sequence;
        }
        
        private BT_Status GoToStoneOre()
        {
            MiningPoint = StoneOre.GetMiningPoint(transform.position);
            if (MiningPoint == null)
            {
                return BT_Status.Failure;
            }
            return GoToLocation(MiningPoint.position);
        }
        
        private BT_Status Work()
        {
            Debug.LogError("Mining");
            if (MineStone())
            {
                Debug.LogError("Stone mined");
                return BT_Status.Success;
            }
            
            return BT_Status.Running;
        }
        
        private BT_Status GoToStorage()
        {
            if (StoneStorage == null)
            {
                Debug.Log("Storage location is not set");
                return BT_Status.Failure;
            }
            
            Debug.LogError("Going to storage");
            BT_Status status = GoToLocation(StoneStorage.transform.position);

            if (status == BT_Status.Success)
            {
                StoneStorage.AddResource(StoneAmount);
                StoneAmount = 0;
            }
            
            return status;
        }
        
    }
}