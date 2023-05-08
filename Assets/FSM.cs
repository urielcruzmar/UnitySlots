using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class FSM : MonoBehaviour
{
    // State enum
    [SerializeField] public enum FSMState
    {
        Follow,
        Retreat,
        Autodestroy
    }
    
    // Exposing struct
    public struct FSMProbability
    {
        public FSMState state;
        public int weight;
    }
    public FSMProbability[] states;

    FSMState selectState()
    {
        var totalWeights = states.Sum(state => state.weight);
        var randomNumber = Random.Range(0, totalWeights);
        var i = 0;
        while (randomNumber >= 0)
        {
            var tempState = states[i];
            randomNumber -= tempState.weight;
            if (randomNumber <= 0)
            {
                return tempState.state;
            }
            i++;
        }
        throw new Exception("ERROR: Something wrong in selectState algorithm");
    }
    
    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Space)) return;
        var randomState = selectState();
        Debug.Log(randomState.ToString());
    }
}
