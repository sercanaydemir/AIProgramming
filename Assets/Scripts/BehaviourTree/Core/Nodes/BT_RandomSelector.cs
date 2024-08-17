using Ravenholm.Tools.BehaviourTree.Utils;
using UnityEngine;

namespace BehaviourTree.Core
{
    public class BT_RandomSelector : BT_Node
    {
        bool isShuffled = false;
        public BT_RandomSelector()
        {
            
        }
        public BT_RandomSelector(string n)
        {
            name = n;
        }
        
        public override BT_Status Process()
        {
            if (!isShuffled)
            {
                childs.Shuffle();
                isShuffled = true;
            }
            
            BT_Status childStatus = childs[currentChild].Process();
            
            if(childStatus == BT_Status.Running)
                return BT_Status.Running;
            if (childStatus == BT_Status.Success)
            {
                currentChild = 0;
                isShuffled = false;
                return childStatus;
            }
            
            if(childStatus == BT_Status.Failure)
            {
                currentChild++;
                if (currentChild >= childs.Count)
                {
                    isShuffled = false;
                    currentChild = 0;
                    return BT_Status.Failure;
                }
            }

            return BT_Status.Running;
        }
    }
}