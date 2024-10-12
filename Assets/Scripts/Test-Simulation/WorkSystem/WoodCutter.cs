using BehaviourTree.Core;
using NPCs;
using Test_Simulation.Locations;
using Test_Simulation.Managers;
using Test_Simulation.WorkSystem.Interfaces;
using UnityEngine;
using Tree = Test_Simulation.Locations.Tree;

namespace Test_Simulation.WorkSystem
{
    public class WoodCutter : AgentAI,IWoodCutter
    {   
        public int WoodAmount { get; set; }
        public Tree TargetTree { get; set; }
        public Storage WoodStorage { get; set; }
        
        public float cutSpeed = 1f;
        public float currentCutTime = 0f;
        
        protected override void Start()
        {
            base.Start();
            WoodStorage = LocationManager.Instance.woodStorage;

            currentCutTime = cutSpeed;
        }

        public bool CutWood()
        {
            
            if(currentCutTime>0)
            {
                currentCutTime -= 0.25f;
                return false;
            }
            
            TargetTree.CutWood();   
            WoodAmount++;
            currentCutTime = cutSpeed;
            
            if (WoodAmount >= 3)
            {
                return true;
            }
            return false;
        }
        
        protected override BT_Sequence BuildWorkSequence()
        {
            BT_Sequence sequence = new BT_Sequence("WorkSequence");
            BT_Leaf goToTree = new BT_Leaf("GoToTree", GoToTree);
            BT_Leaf cutWood = new BT_Leaf("CutWood", Work);
            BT_Leaf goToStorage = new BT_Leaf("GoToStorage", GoToStorage);
            
            sequence.AddChild(goToTree);
            sequence.AddChild(cutWood);
            sequence.AddChild(goToStorage);
            return sequence;
        }
        
        private BT_Status GoToTree()
        {
            TargetTree = GetClosestTree();
            if (TargetTree == null)
            {
                Debug.LogError("No tree found");
                return BT_Status.Running;
            }
            return GoToLocation(TargetTree.transform.position);
        }
        
        Tree GetClosestTree()
        {
            return LocationManager.Instance.GetTreeLocation(transform);
        }
        
        private BT_Status Work()
        {
            if (CutWood())
            {
                return BT_Status.Success;
            }
            
            return BT_Status.Running;
        }
        
        private BT_Status GoToStorage()
        {
            if (WoodAmount == 0)
            {
                return BT_Status.Failure;
            }
            if (WoodStorage == null)
            {
                Debug.Log("Storage location is not set");
                return BT_Status.Failure;
            }
            BT_Status status = GoToLocation(WoodStorage.transform.position);

            if (status == BT_Status.Success)
            {
                WoodStorage.AddResource(WoodAmount);
                WoodAmount = 0;
            }
            
            return status;
        }
    }
}