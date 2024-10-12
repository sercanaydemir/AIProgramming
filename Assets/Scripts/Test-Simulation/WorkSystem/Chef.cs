using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using BehaviourTree.Core;
using NPCs;
using Ravenholm.Managers;
using Test_Simulation.Locations;
using Test_Simulation.Managers;
using Test_Simulation.WorkSystem.Interfaces;
using UnityEngine;
using Utilities;

namespace Test_Simulation.WorkSystem
{
    public class Chef : AgentAI,IChef
    {
        public Storage FoodStorage { get; set; }
        public Kitchen Kitchen { get; set; }
        public int FoodAmount { get; set; }

        protected override void Start()
        {
            base.Start();
            FoodStorage = LocationManager.Instance.foodStorage;
            Kitchen = LocationManager.Instance.cookLocations;
        }

        public bool IsFoodAvailable()
        {
            return FoodAmount > 0;
        }
        public void Cook()
        {
            FoodAmount--;
        }
        
        public void AddFood(int amount)
        {
            FoodAmount += amount;
        }

        protected override BT_Sequence BuildWorkSequence()
        {
            BT_Sequence sequence = new BT_Sequence("WorkSequence");
            BT_Leaf goToStorage = new BT_Leaf("GoToStorage", GoToStorage);
            BT_Leaf goToCookPoint = new BT_Leaf("GoToStorage", GoToCookPoint);
            BT_Leaf cookFood = new BT_Leaf("CookFood", CookFood);
            
            sequence.AddChild(goToStorage);
            sequence.AddChild(goToCookPoint);
            sequence.AddChild(cookFood);
            return sequence;
        }

        private BT_Status GoToStorage()
        {
            if (IsFoodAvailable())
            {
                return BT_Status.Success;
            }
            
            if (FoodStorage == null)
            {
                Debug.Log("Storage location is not set");
                return BT_Status.Failure;
            }
            BT_Status status = GoToLocation(FoodStorage.transform.position);

            if (status == BT_Status.Success)
            {
                AddFood(1);
                FoodStorage.RemoveResource(1);
            }
            
            return GoToLocation(FoodStorage.transform.position);
        }
        
        private BT_Status GoToCookPoint()
        {
            if (Kitchen == null)
            {
                Debug.Log("Cook point is not set");
                return BT_Status.Failure;
            }
            return GoToLocation(Kitchen.transform.position);
        }
        
        private BT_Status CookFood()
        {
            if (IsFoodAvailable())
            {
                if (Kitchen.Cooking())
                {
                    FoodAmount--;
                    CheckState();
                    Debug.Log("Food cooked");
                    return BT_Status.Success;
                }
                
                Debug.Log("Cooking...");
                
                return BT_Status.Running;
            }
            Debug.LogError("No food available");
            return BT_Status.Failure;
        }

        private void CheckState()
        {
            if(_state == AgentState.Working)
            {
                tree.AddChild(BuildWorkSequence());
            }
            
        }
    }
}