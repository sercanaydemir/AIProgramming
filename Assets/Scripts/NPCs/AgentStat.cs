using System;
using UnityEditor.UIElements;
using UnityEngine;

namespace NPCs
{
    [Serializable]
    public struct AgentStat
    {
        [Range(0, 1)] public float StatValue;
        [field: SerializeField]public AnimationCurve StatCurve { get; private set; }
        public float HealthPenalty => _healthPenalty.GetPenalty(StatValue);
        [SerializeField] private HealthPenalty _healthPenalty;
        public float GetStatValue()
        {
            return StatValue;
        }

        public void SetStatValue(float value)
        {
            StatValue = value;
            StatValue = Mathf.Clamp(StatValue, 0, 1);
        }

        public void IncreaseStat(float value)
        {
            StatValue += value;
            StatValue = Mathf.Clamp(StatValue, 0, 1);
        }

        public void DecreaseStat(float value)
        {
            StatValue -= value;
            StatValue = Mathf.Clamp(StatValue, 0, 1);
        }
        
        public float GetStatValueForCurve()
        {
            return StatCurve.Evaluate(StatValue);
        }
    }
    
    [Serializable]
    public struct HealthPenalty
    {
        [Range(1,2)] public float penalty;
        public float penaltyLimit;
        public bool isPenaltyActive;
        public bool isBiggerLimit;
        
        public float GetPenalty(float value)
        {
            if (!isPenaltyActive) return 1;
            
            if (isBiggerLimit)
            {
                if(penaltyLimit<value)
                {
                    Debug.LogError("penalty applied: " + penaltyLimit);
                    return penalty;
                }
                else
                {
                    return 1;
                }
            }
            else
            {
                if(penaltyLimit>value)
                {
                    Debug.LogError("penalty applied: " + penaltyLimit);
                    return penalty;
                }
                else
                {
                    return 1;
                }
            }
        }
    }
}