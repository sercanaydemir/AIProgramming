using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree.Core
{
    // Dynamic Priority Selector
    public class BT_DynamicPSelector : BT_Node
    {
        BT_Node[] array;
        public BT_DynamicPSelector()
        {
            
        }
        public BT_DynamicPSelector(string n)
        {
            name = n;
        }
        
        void OrderNodes()
        {
            array = childs.ToArray();
            SortNodes(array, 0, childs.Count - 1);
            childs = new List<BT_Node>(array); 
        }
        public override BT_Status Process()
        {
            OrderNodes();
            BT_Status childStatus = childs[currentChild].Process();
            Debug.LogError("child name: " + childs[currentChild].name + " sort order: " + childs[currentChild].sortOrder );
            if(childStatus == BT_Status.Running)
                return BT_Status.Running;
            if (childStatus == BT_Status.Success)
            {
                // difference from BT_PSelector
                childs[currentChild].sortOrder = 1;
                //---------------------------
                currentChild = 0;
                return childStatus;
            }
            // difference from BT_PSelector
            else
                childs[currentChild].sortOrder = 10;
            //---------------------------
            
            
            if(childStatus == BT_Status.Failure)
            {
                Debug.LogError("Selector Failure " + name + " " + childs[currentChild].name + " ");
                currentChild++;
                if (currentChild >= childs.Count)
                {
                    currentChild = 0;
                    return BT_Status.Failure;
                }
            }

            return BT_Status.Running;
        }
        
        int Partition(BT_Node[] array, int low, int high)
        {
            BT_Node pivot = array[high];
            int i = (low - 1);
            for (int j = low; j < high; j++)
            {
                if (array[j].sortOrder <= pivot.sortOrder)
                {
                    i++;
                    BT_Node temp = array[i];
                    array[i] = array[j];
                    array[j] = temp;
                }
            }
            BT_Node temp1 = array[i + 1];
            array[i + 1] = array[high];
            array[high] = temp1;
            return i + 1;
        }
        
        void SortNodes(BT_Node[] array, int low, int high)
        {
            if (low < high)
            {
                int pi = Partition(array, low, high);
                SortNodes(array, low, pi - 1);
                SortNodes(array, pi + 1, high);
            }
        }
    }
}