using System;
using UnityEditor.UIElements;
using UnityEngine;

namespace NPCs
{
    [Serializable]
    public struct AgentStat
    {
        [Range(0, 1)] public float StatValue;
        public AnimationCurve StatCurve;

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
    }
}