using System;
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
        
        public AgentState currentState;
        public void UpdateStats(AgentState state)
        {
            currentState = state;
            float fatigueImpact = CalculateFatigueStatWithAllStats();
            float hungryImpact = CalculateHungryStatWithAllStats();
            float socialImpact = CalculateSocialStatWithAllStats();
            float moraleImpact = CalculateMoraleStatWithAllStats();
            
            switch (state)
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
            
            Debug.LogError("Hungry: " + Hungry.GetStatValueForCurve());
            Debug.LogError("Fatigue: " + Fatigue.GetStatValueForCurve());
            
            GUIPrinter.LifeStatsChanged(this);
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
        }
        
        public void Resting(float multiplier = 1)
        {
            Fatigue.DecreaseStat(0.01f*multiplier);
        }
        
        public void Socializing(float  multiplier = 1)
        {
            Social.IncreaseStat(0.01f*multiplier);
        }
        
        public void Working()
        {
            Fatigue.IncreaseStat(0.0035f);
            Hungry.IncreaseStat(0.0025f);
            Social.DecreaseStat(0.005f);
        }
        
        public void Sleeping()
        {
            Social.DecreaseStat(0.01f);
        }
        
        
    }
}