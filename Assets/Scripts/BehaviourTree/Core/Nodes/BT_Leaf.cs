
namespace BehaviourTree.Core
{
    public class BT_Leaf : BT_Node
    {
        public delegate BT_Status Tick();
        public Tick ProcessMethod;
        
        public delegate BT_Status TickM(int index);
        public TickM ProcessMethodM;
        
        private int _index;
        
        public BT_Leaf(string name, Tick processMethod)
        {
            base.name = name;
            ProcessMethod = processMethod;
        }
        public BT_Leaf(string name, int i, TickM processMethodM)
        {
            base.name = name;
            ProcessMethodM = processMethodM;
            _index = i;
        }
        
        public BT_Leaf(string name, Tick processMethod,int sortOrder)
        {
            base.name = name;
            ProcessMethod = processMethod;
            base.sortOrder = sortOrder;
        }
        
        public override BT_Status Process()
        {
            if(ProcessMethod != null)
                return ProcessMethod();
            else if(ProcessMethodM != null)
                return ProcessMethodM(_index);
            return BT_Status.Failure;
        }
    }
}