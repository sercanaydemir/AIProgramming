﻿using System;
using System.Collections.Generic;
using BehaviourTree.Core;
using NPCs.Interfaces;
using Ravenholm.Managers;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.Serialization;
using Utilities;

namespace NPCs
{
    public class AgentAI : BT_Agent,IHealth
    {
        public Transform workPoint;
        public Transform eatPoint;
        public Transform restPoint;
        public Transform socializePoint;
        
        public LifeStats lifeStats;
        private AgentState _state;
        
        [field: SerializeField] public float Health { get; set; }

        // Agent Life Stats Data Collection
        CSVWriter csvWriter;
        private void Awake()
        {
            csvWriter = new CSVWriter();
            
            lifeStats.SetIHealth(this);
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
            _state = AgentState.Eating;

            if (lifeStats.Hungry.GetStatValue() < 0.2f)
            {
                Debug.LogError("Hungry is full!");
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
            Debug.LogError("Resting...");
            _state = AgentState.Resting;
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
            Debug.LogError("Going to socialize point...");
            return GoToLocation(socializePoint.position);
        }
        
        public BT_Status Socialize()
        {
            Debug.LogError("Socializing...");
            _state = AgentState.Socializing;
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
            _state = AgentState.Working;
            Debug.LogWarning("Working is done! Worker can go to next");
            return BT_Status.Running;
        }
        
        #endregion
        //-----------------------------------------------------------------------------------
        
        private void OnMinuteChanged()
        {
            lifeStats.UpdateStats(_state);
        }
        private void OnTimeOfDayChanged(TimeOfDay obj)
        {
            //Debug.LogError("Time of day changed! " + obj );
            if(obj == TimeOfDay.Noon) return;
            
            tree.ResetRoot();
            switch (obj)
            {
                case TimeOfDay.BeforeMorning:
                    tree.AddChild(BuildEatSequence());
                    break;
                case TimeOfDay.Morning:
                    tree.AddChild(BuildWorkSequence());
                    break;
                case TimeOfDay.Afternoon:
                    tree.AddChild(BuildWorkSequence());
                    break;
                case TimeOfDay.Evening:
                    BT_Sequence eveningSequence = new BT_Sequence("Evening Sequence");
                    eveningSequence.AddChild(BuildEatSequence());
                    eveningSequence.AddChild(BuildSocializeSequence());
                    tree.AddChild(eveningSequence);
                    break;
                case TimeOfDay.Night:
                    tree.AddChild(BuildRestSequence());
                    break;
            }
            
            //Debug.Break();
            
            _state = AgentState.None;
            csvWriter.AddData(TimeManager.Instance.GetCurrentDate().Day,obj.ToString(),lifeStats.Hungry.GetStatValue()
                ,lifeStats.Fatigue.GetStatValue(),lifeStats.Social.GetStatValue(),lifeStats.Morale.GetStatValue(),Health);
            tree.PrintTree();
        }

        #region IHealth Implementation
        public void TakeDamage(float damage)
        {
            Health -= CalculateHealthPenaltyForDamage(damage);
            if (IsDead())
            {
                AgentDeath();
                Health = Mathf.Clamp(Health, 0, 1);
            }
        }

        public void Heal(float amount)
        {
            Health += amount;
            Health = Mathf.Clamp(Health, 0, 1);
        }

        public bool IsDead()
        {
            if (Health <= 0)
            {
                //TODO: Implement agent death actions here
                
                Debug.LogError("Agent is dead! " + this.name,transform);
                return true;
            }

            return false;
        }
        private void OnAgentDeath(AgentAI obj)
        {
            
        }
        
        private float CalculateHealthPenaltyForDamage(float damage)
        {
            string takeDamage = "Before taken penalty impact:" + damage.ToString();
            
            damage *= lifeStats.MoralePenaltyForHealth();
            damage *= lifeStats.FatiguePenaltyForHealth();
            damage *= lifeStats.HungryPenaltyForHealth();
            
            takeDamage += " After taken penalty impact:" + damage.ToString() + " MoralePenalty " + lifeStats.MoralePenaltyForHealth() 
                          + " FatiguePenalty " + lifeStats.FatiguePenaltyForHealth() + " HungryPenalty " + lifeStats.HungryPenaltyForHealth();
            
            Debug.LogError(takeDamage);
            return damage;
        }
        
        #endregion
        
        //-----------------------------------------------------------------------------------
        private void OnEnable()
        {
            TimeManager.OnMinuteChanged += OnMinuteChanged;
            TimeManager.OnTimeOfDayChanged += OnTimeOfDayChanged;
            AgentDeathEvent += OnAgentDeath;
        }
        private void OnDisable()
        {
            TimeManager.OnMinuteChanged -= OnMinuteChanged;
            TimeManager.OnTimeOfDayChanged -= OnTimeOfDayChanged;
            AgentDeathEvent -= OnAgentDeath;
        }
        
        //-----------------------------------------------------------------------------------

        #region events

        public static event Action<AgentAI> AgentDeathEvent;
        
        private void AgentDeath()
        {
            AgentDeathEvent?.Invoke(this);
        }

        #endregion
    }
    
    
}
