namespace BehaviourTree.Core
{
    public class BT_Inverter : BT_Node
    {
        public BT_Inverter(string name)
        {
            this.name = name;
        }

        public override BT_Status Process()
        {
            BT_Status childStatus = childs[currentChild].Process();
            if (childStatus == BT_Status.Running)
                return BT_Status.Running;
            if (childStatus == BT_Status.Failure) return BT_Status.Success;
         
            return BT_Status.Failure;
        }
    }
}