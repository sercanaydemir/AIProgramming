using System.Collections.Generic;

namespace BehaviourTree.Core
{
    public class BT_Node
    {
        public BT_Status Status { get; set; }
        public List<BT_Node> childs = new List<BT_Node>();
        public int currentChild = 0;
        public string name;
        public int sortOrder;
        public BT_Node(){ }
        
        public BT_Node(string name)
        {
            this.name = name;
        }
        
        public BT_Node (string name, int sortOrder)
        {
            this.name = name;
            this.sortOrder = sortOrder;
        }

        public void Reset()
        {
            childs.ForEach(x => x.Reset());
            currentChild = 0;
        }
        
        public void AddChild(BT_Node child)
        {
            childs.Add(child);
        }

        public virtual BT_Status Process()
        {
            return childs[currentChild].Process();
        }
        
    }
    
    public enum BT_Status
    {
        Success,
        Failure,
        Running
    }
}