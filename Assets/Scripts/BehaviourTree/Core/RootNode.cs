using System.Collections.Generic;
using BehaviourTree.Core;
using UnityEngine;

namespace Ravenholm.Tools.BehaviourTree.Examples
{
    public class RootNode : BT_Node
    {
        public RootNode()
        {
            name = "Root";
        }

        public RootNode(string name)
        {
            base.name = name;
        }
        
        public void ResetRoot()
        {
            childs.Clear();
            currentChild = 0;
            proccesForcedSuccess = false;
        }

        public override BT_Status Process()
        {
            if (childs.Count == 0) return BT_Status.Success;
            
            return childs[currentChild].Process();
        }

        struct NodeLevel
        {
            public BT_Node node;
            public int level;
        }

        public void PrintTree()
        {
            string printOut = "";
            Stack<NodeLevel> stack = new Stack<NodeLevel>();
            BT_Node currentNode = this;
            stack.Push(new NodeLevel {node = currentNode, level = 0});
            while (stack.Count != 0)
            {
                NodeLevel next = stack.Pop();
                printOut += new string('-', next.level) + next.node.name + "\n";

                for (int i = next.node.childs.Count-1; i>=0; i--)
                {
                    stack.Push(new NodeLevel {node = next.node.childs[i], level = next.level + 1});
                }
            }
            
            Debug.Log(printOut);
            
        }
    }
}