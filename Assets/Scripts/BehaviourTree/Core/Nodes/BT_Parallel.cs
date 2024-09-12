namespace BehaviourTree.Core
{
    public class BT_Parallel : BT_Node
    {
        public BT_Parallel(string name, int sortOrder = 0)
        {
            this.name = name;
            this.sortOrder = sortOrder;
        }
        
        public override BT_Status Process()
        {
            BT_Status childStatus = BT_Status.Running;
            for (int i = 0; i < childs.Count; i++)
            {
                childStatus = childs[i].Process();
            }
            return childStatus;
        }
    }
}