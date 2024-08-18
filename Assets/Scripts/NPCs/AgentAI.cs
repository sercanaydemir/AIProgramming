using BehaviourTree.Core;
using UnityEngine;
using UnityEngine.Serialization;

namespace NPCs
{
    public class AgentAI : BT_Agent
    {
        public Transform workPoint;
        public Transform eatPoint;
        public Transform restPoint;
        public Transform socializePoint;
        
        public float workTime = 10f;
        public float remainingWorkTime = 10f;

        public float hungry;
        public float fatigue;
        public float social;
        public float morale;
        
        public float hungryMultiplier = 1f;
        public float fatigueMultiplier = 1f;
        public float socialMultiplier = 1f;
        public float moraleMultiplier = 1f;

        protected override void Start()
        {
            base.Start();
            BuildDayCycle();
        }

        void BuildDayCycle()
        {
            BT_Sequence dayCycle = new BT_Sequence("Day Cycle");
            dayCycle.AddChild(BuildEatSequence());
            dayCycle.AddChild(BuildWorkSequence());
            dayCycle.AddChild(BuildEatSequence());
            dayCycle.AddChild(BuildWorkSequence());
            dayCycle.AddChild(BuildSocializeSequence());
            dayCycle.AddChild(BuildEatSequence());
            dayCycle.AddChild(BuildRestSequence());
            
            tree.AddChild(dayCycle);
            tree.PrintTree();

        }


        #region Eat 

        BT_Sequence BuildEatSequence()
        {
            BT_Sequence eatingSequence = new BT_Sequence("Eating Sequence");
            BT_Leaf goToEatPoint = new BT_Leaf("Go To Eat Point", GoToEatPoint);
            BT_Leaf collectFood = new BT_Leaf("Collect Food",CollectingFood);
            BT_Leaf eat = new BT_Leaf("Eat", Eat);
            
            eatingSequence.AddChild(goToEatPoint);
            eatingSequence.AddChild(collectFood);
            eatingSequence.AddChild(eat);
            return eatingSequence;

        }
        
        BT_Status GoToEatPoint()
        {
            return GoToLocation(eatPoint.position);
        }

        BT_Status Eat()
        {
            Debug.LogError("Eating...");
            CalculateStatsEatTime();
            if(hungry<=0)
            {
                Debug.LogWarning("Eating is done! Worker can go to work");
                return BT_Status.Success;
            }
            return BT_Status.Running;
        }

        BT_Status CollectingFood()
        {
            // integrate food resources system and check here if there is food to collectable
            
            Debug.LogError("Collecting food...");
            return BT_Status.Success;
        }
        #endregion
        
        //-----------------------------------------------------------------------------------

        #region Resting Sequence

        BT_Sequence BuildRestSequence()
        {
            BT_Sequence restSequence = new BT_Sequence("Rest Sequence");
            BT_Leaf goToRestPoint = new BT_Leaf("Go To Rest Point", GoToRestPoint);
            BT_Leaf rest = new BT_Leaf("Rest", Resting);
            
            restSequence.AddChild(goToRestPoint);
            restSequence.AddChild(rest);
            return restSequence;
        }
        
        BT_Status GoToRestPoint()
        {
            return GoToLocation(restPoint.position);
        }
        
        public BT_Status Resting()
        {
            CalculateStatsRestTime();
            Debug.LogError("Resting...");
            if (fatigue <= 0)
            {
                Debug.LogWarning("Resting is done! Worker can go to work");
                return BT_Status.Success;
            }
            return BT_Status.Running;
        }
        
        
        #endregion
        //-----------------------------------------------------------------------------------

        #region Socialize Sequence


        BT_Sequence BuildSocializeSequence()
        {
            BT_Sequence socializingSequence = new BT_Sequence("Socializing Sequence");
            BT_Leaf goToSocializePoint = new BT_Leaf("Go To Socialize Point", GoToSocializePoint);
            BT_Leaf socialize = new BT_Leaf("Socialize",Socialize);
            
            socializingSequence.AddChild(goToSocializePoint);
            socializingSequence.AddChild(socialize);
            return socializingSequence;
        }

        private BT_Status GoToSocializePoint()
        {
            return GoToLocation(socializePoint.position);
        }
        
        public BT_Status Socialize()
        {
            Debug.LogError("Socializing...");
            CalculateStatsSocializeTime();
            
            if(social >= 100)
            {
                Debug.LogWarning("Socializing is done! Worker can go to rest");
                return BT_Status.Success;
                
            }
            
            return BT_Status.Running;
        }
        
        #endregion
        
        //-----------------------------------------------------------------------------------

        #region Work Sequence
        
        BT_Sequence BuildWorkSequence()
        {
            BT_Sequence workingSequence = new BT_Sequence("Working Sequence");
            BT_Leaf goToWorkPoint = new BT_Leaf("Go To Work Point", GoToWorkPoint);
            BT_Leaf work = new BT_Leaf("Work", Work);
            
            workingSequence.AddChild(goToWorkPoint);
            workingSequence.AddChild(work);
            return workingSequence;
        }

        private BT_Status GoToWorkPoint()
        {
            return GoToLocation(workPoint.position);
        }
        
        private BT_Status Work()
        {
            
            if(remainingWorkTime > 0)
            {
                Debug.LogError("Working...");
                remainingWorkTime -= Time.deltaTime*50f;
                CalculateStatsWorkTime();
                return BT_Status.Running;
            }
            remainingWorkTime = workTime;
            Debug.LogWarning("Working is done! Worker can go to rest");
            return BT_Status.Success;
        }
        
        #endregion
        //-----------------------------------------------------------------------------------
        
        void CalculateStatsWorkTime()
        {
            hungry += Time.deltaTime*2;
            fatigue += Time.deltaTime*3;
            social -= Time.deltaTime;
            morale -= Time.deltaTime*2;
        }
        
        void CalculateStatsRestTime()
        {
            hungry += Time.deltaTime;
            fatigue -= Time.deltaTime*fatigueMultiplier;
            morale += Time.deltaTime;
        }
        
        void CalculateStatsSocializeTime()
        {
            hungry += Time.deltaTime;
            fatigue += Time.deltaTime;
            social += Time.deltaTime*socialMultiplier;
            morale += Time.deltaTime;
        }
        
        void CalculateStatsEatTime()
        {
            hungry -= Time.deltaTime*hungryMultiplier;
            fatigue += Time.deltaTime;
            social += Time.deltaTime;
            morale += Time.deltaTime;
        }
    }
}
