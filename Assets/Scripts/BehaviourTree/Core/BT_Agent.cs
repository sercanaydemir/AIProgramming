using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Ravenholm.Tools.BehaviourTree.Examples;
namespace BehaviourTree.Core
{
    public abstract class BT_Agent : MonoBehaviour
    {
        protected RootNode _tree;
        protected NavMeshAgent _agent;
        protected enum ActionState{ Idle, Working }
        protected ActionState actionState = ActionState.Idle;
        
        protected BT_Status _status = BT_Status.Running;
        private WaitForSeconds _waitForSeconds;
        private Vector3 rememberedLocation;
        protected virtual void Start()
        {
            _agent = GetComponent<NavMeshAgent>();
            _tree = new RootNode();
            _waitForSeconds = new WaitForSeconds(Random.Range(0.1f,1));
            StartCoroutine(Behave());
        }
        
        IEnumerator Behave()
        {
            while (true)
            { 
                _status = _tree.Process();
                yield return _waitForSeconds;
            }
        }
        
        protected BT_Status CanSee(Vector3 target, string tag, float distance,float maxAngle)
        {
            Vector3 direction = target - transform.position;
            float angle = Vector3.Angle(direction, transform.forward);
         
            RaycastHit hit;
            if (angle < maxAngle)
            {
                if (Physics.Raycast(transform.position, direction, out hit, distance))
                {
                    if (hit.collider.CompareTag(tag))
                    {
                        Debug.LogError("Can see " + tag);
                        return BT_Status.Success;
                    }
                }
            }
            Debug.LogError("Can't see " + tag);
            return BT_Status.Failure;
        }

        protected BT_Status Flee(Vector3 location, float distance)
        {
            if (actionState == ActionState.Idle)
            {
                Vector2 randomDirection = Random.insideUnitCircle * distance;
                Vector3 fleeLocation = new Vector3(location.x + randomDirection.x, location.y, location.z + randomDirection.y);
                rememberedLocation = fleeLocation;
                Debug.LogError("rememberedLocation " + rememberedLocation);
            }
            Debug.LogError("Flee" + rememberedLocation);
            return GoToLocation(rememberedLocation);
        }
        protected BT_Status GoToLocation(Vector3 destination)
        {
            float distance = Vector3.Distance(transform.position, destination);
            
            if(actionState == ActionState.Idle)
            {
                _agent.SetDestination(destination);
                actionState = ActionState.Working;
            }
            else if(Vector3.Distance(_agent.pathEndPosition, destination) >= 2f)
            {
                actionState = ActionState.Idle;
                return BT_Status.Failure;
            }
            else if(distance < 2f)
            {
                actionState = ActionState.Idle;
                return BT_Status.Success;
            }
            return BT_Status.Running;
        }
    }
}