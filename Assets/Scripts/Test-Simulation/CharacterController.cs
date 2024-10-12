using System;
using NPCs;
using UnityEngine;
using Utilities;

namespace Test_Simulation
{
    [RequireComponent(typeof(AgentAI))]
    public class CharacterController : MonoBehaviour
    {
        AgentAI _agentAI;
        
        private void Awake()
        {
            _agentAI = GetComponent<AgentAI>();
        }

        private void OnAgentStateChanged(AgentState obj)
        {
            switch (obj)
            {
                case AgentState.None:
                    break;
                case AgentState.Eating:
                    Debug.Log("Eating...");
                    break;
                case AgentState.Resting:
                    Debug.Log("Resting...");
                    break;
                case AgentState.Socializing:
                    Debug.Log("Socializing...");
                    break;
                case AgentState.Working:
                    Debug.Log("Working...");
                    break;
            }
        }

        void Eat()
        {
            
        }
        
        void Rest()
        {
            
        }
        
        void Work()
        {
            
        }
        
        void Socialize()
        {
            
        }
        
        private void OnEnable()
        {
            _agentAI.OnAgentStateChanged += OnAgentStateChanged;
        }
        private void OnDisable()
        {
            _agentAI.OnAgentStateChanged -= OnAgentStateChanged;
        }

    }
}