using System.Collections;
using BehaviourTree.Core;
using Unity.VisualScripting;
using UnityEngine;

namespace Test_Simulation.Locations
{
    public class Kitchen : Location
    {
        public Transform[] cookPoints;
        public int foodAmount;
        public int cookedMealCount;
        private Coroutine cookingRoutine;
        public float cookTime = 5;
        
        public bool Cooking()
        {
            if(cookTime > 0)
            {
                cookTime -= 0.25f;
                return false;
            }
            else
            {
                foodAmount--;
                FinishCooking();
                cookTime = 5;
                return true;
            }
        }

        public void FinishCooking()
        {
            cookedMealCount++;
        }

        public void AddMeal(int meal)
        {
            foodAmount += meal;
        }
        
        public Transform GetAvailableCookPoint()
        {
            return cookPoints[0];
        }

        public override bool IsAvailable()
        {
            throw new System.NotImplementedException();
        }
    }
}