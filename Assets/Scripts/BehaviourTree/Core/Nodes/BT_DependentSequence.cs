using Ravenholm.Tools.BehaviourTree.Examples;
using UnityEngine;
using UnityEngine.AI;

namespace BehaviourTree.Core
{
    public class BT_DependentSequence : BT_Node
    {
        RootNode dependantTree;
        NavMeshAgent agent;
        public BT_DependentSequence(string name,RootNode dependantTree,NavMeshAgent agent)
        {
            this.name = name;
            this.dependantTree = dependantTree;
            this.agent = agent;
        }
        public override BT_Status Process()
        {
            if (dependantTree.Process() == BT_Status.Failure)
            {
                agent.ResetPath();
                childs.ForEach(x => x.Reset());
                
                return BT_Status.Failure;
            }
            
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