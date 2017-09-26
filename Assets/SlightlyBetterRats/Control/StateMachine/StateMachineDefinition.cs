using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "State Machine Definition")]
public class StateMachineDefinition : ScriptableObject {
    [Serializable]
    public struct StateDefinition {
        public string name;
        public string[] transitions;
    }

    public string defaultState;
    public StateDefinition[] states;
}
