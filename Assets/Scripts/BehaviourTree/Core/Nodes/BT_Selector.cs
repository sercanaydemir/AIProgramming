using UnityEngine;

namespace BehaviourTree.Core
{
    public class BT_Selector : BT_Node
    {
        public BT_Selector()
        {
            
        }
        public BT_Selector(string n)
        {
            name = n;
        }
        public override BT_Status Process()
        {
            BT_Status childStatus = childs[currentChild].Process();
            
            if(childStatus == BT_Status.Running)
                return BT_Status.Running;
            if (childStatus == BT_Status.Success)
            {
                currentChild = 0;
                return childStatus;
            }
            
            if(childStatus == BT_Status.Failure)
            {
                currentChild++;
                if (currentChild >= childs.Count)
                {
                    currentChild = 0;
                    return BT_Status.Failure;
                }
            }

            return BT_Status.Running;
        }
    }
}