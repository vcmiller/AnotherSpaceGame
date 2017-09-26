using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class AIController : Controller {
    public StateMachineDefinition stateMachineDefinition;

    public StateMachineRunner stateMachine { get; private set; }

    protected virtual void Awake() {
        if (stateMachineDefinition == null) {
            Debug.LogWarning("Warning: No state machine specified.");
        } else {
            stateMachine = new StateMachineRunner(gameObject);
            stateMachine.Run(stateMachineDefinition);
        }
    }

    public override void GetInput() {
        if (enabled) {
            if (stateMachineDefinition != stateMachine.definition) {
                stateMachine.Run(stateMachineDefinition);
            }

            stateMachine.Update();
        }
    }
}
