using System;
using NPCs.Interfaces;
using Ravenholm;
using UnityEngine;
using Utilities;

namespace NPCs
{
    [Serializable]
    public class LifeStats
    {
        public AgentStat Hungry;
        public AgentStat Fatigue;
        public AgentStat Social;
        public AgentStat Morale;
        public IHealth Health;
        public AgentState currentState;

        public void SetIHealth(IHealth health)
        {
            Health = health;
        }
        public void UpdateStats(AgentState state)
        {
            StateChanged(state);
            
            CalculateStats();
            
            GUIPrinter.LifeStatsChanged(this);
        }
        
        public void StateChanged(AgentState state)
        {
            if(state != currentState)
            {
                currentState = state;

                if (state != AgentState.None)
                {
                    CheckStarvationDamage();
                    CheckFatigueDamage();
                    CheckMoraleDamage();
                }
            }
        }
        
        public float CalculateHungryStatWithAllStats()
        {
            return Fatigue.GetStatValue() * 0.002f + Social.GetStatValue() * 0.001f + Morale.GetStatValue() * 0.003f;
        }

        public float CalculateFatigueStatWithAllStats()
        {
            return Hungry.GetStatValue() * 0.003f + Social.GetStatValue() * 0.001f + Morale.GetStatValue() * 0.002f;
        }

        public float CalculateSocialStatWithAllStats()
        {
            return Fatigue.GetStatValue() * 0.0005f + Hungry.GetStatValue() * 0.001f + Morale.GetStatValue() * 0.001f;
        }

        public float CalculateMoraleStatWithAllStats()
        {
            return Social.GetStatValue() * 0.0025f + Fatigue.GetStatValueForCurve()  + Hungry.GetStatValueForCurve();
        }
        
        public void Eating(float  multiplier = 1)
        {
            Hungry.DecreaseStat(0.025f*multiplier);
            Health.Heal(0.01f);
        }
        
        public void Resting(float multiplier = 1)
        {
            Fatigue.DecreaseStat(0.01f*multiplier);
            Health.Heal(0.01f);
        }
        
        public void Socializing(float  multiplier = 1)
        {
            Social.IncreaseStat(0.01f*multiplier);
        }
        
        
        void CalculateStats()
        {
            float fatigueImpact = CalculateFatigueStatWithAllStats();
            float hungryImpact = CalculateHungryStatWithAllStats();
            float socialImpact = CalculateSocialStatWithAllStats();
            float moraleImpact = CalculateMoraleStatWithAllStats();
            
            switch (currentState)
            {
                case AgentState.Eating:
                    Eating(2f);
                    Socializing(0.2f);
                    Fatigue.IncreaseStat(0.0025f+fatigueImpact);
                    break;
                case AgentState.Resting:
                    Resting(2.6f);
                    Hungry.IncreaseStat(0.005f + hungryImpact);
                    Social.DecreaseStat(0.001f + socialImpact);
                    break;
                case AgentState.Socializing:
                    Socializing(1.5f);
                    Fatigue.IncreaseStat(0.0045f + fatigueImpact);
                    Hungry.IncreaseStat(0.007f + hungryImpact);
                    break;
                case AgentState.Working:
                    Social.DecreaseStat(0.001f + socialImpact);
                    Hungry.IncreaseStat(0.0085f + hungryImpact);
                    Fatigue.IncreaseStat(0.008f + fatigueImpact);
                    break;
                case AgentState.None:
                    Hungry.IncreaseStat(0.003f + hungryImpact);
                    Fatigue.IncreaseStat(0.0015f + fatigueImpact);
                    Social.DecreaseStat(0.001f + socialImpact);
                    break;
            }
            
            Morale.SetStatValue(Social.GetStatValue()*0.75f - Fatigue.GetStatValueForCurve() - Hungry.GetStatValueForCurve());
            
        }

        public float HungryPenaltyForHealth()
        {
            return Hungry.HealthPenalty;
        }
        
        public float FatiguePenaltyForHealth()
        {
            return Fatigue.HealthPenalty;
        }
        
        public float MoralePenaltyForHealth()
        {
            return Morale.HealthPenalty;
        }

        public void CheckStarvationDamage()
        {
            if(Hungry.GetStatValue() > 0.7f)
                Health.TakeDamage(0.2f);
        }
        
        public void CheckFatigueDamage()
        {
            if(Fatigue.GetStatValue()>0.8f)
                Health.TakeDamage(0.1f);
        }
        
        public void CheckMoraleDamage()
        {
            if(Morale.GetStatValue() < 0.3f)
                Health.TakeDamage(0.025f);
        }
    }
}