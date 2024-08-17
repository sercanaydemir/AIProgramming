
using UnityEngine;

namespace BehaviourTree.Core
{
    public class BT_Sequence : BT_Node
    {
        public BT_Sequence(string name,int sortOrder = 0)
        {
            this.name = name;
            this.sortOrder = sortOrder;
        }

        public override BT_Status Process()
        {
            BT_Status childStatus = childs[currentChild].Process();

            if (childStatus == BT_Status.Running)
                return BT_Status.Running;
            if (childStatus == BT_Status.Failure) return childStatus;

            currentChild++;
            if (currentChild >= childs.Count)
            {
                currentChild = 0;
                return BT_Status.Success;
            }
            
            return BT_Status.Running;
        }        
    }
}