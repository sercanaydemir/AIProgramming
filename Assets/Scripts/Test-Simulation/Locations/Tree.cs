using Test_Simulation.Managers;
using UnityEngine;

namespace Test_Simulation.Locations
{
    public class Tree : Location
    {
        public int woodAmount;
        public override bool IsAvailable()
        {
            return woodAmount>0;
        }
        
        public void CutWood()
        {
            woodAmount--;
            
            if(woodAmount == 0)
            {
                gameObject.SetActive(false);
                LocationManager.Instance.RemoveTree(this);
            }
        }
        
         
    }
}