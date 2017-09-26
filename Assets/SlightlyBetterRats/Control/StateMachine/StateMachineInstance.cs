using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachineInstance {
    public StateMachineBinding.State currentState { get; private set; }
    public int maxTransitionsPerTick = 16;

    public StateMachineInstance(StateMachineBinding binding) {
        currentState = binding.defaultState;
    }

    public void Update() {
        CallIfExists(currentState.during);

        var oldState = currentState;
        TryTransition(maxTransitionsPerTick);

        if (oldState != currentState) {
            CallIfExists(oldState.exit);
            CallIfExists(currentState.enter);
        }
    }

    private StateMachineBinding.State TryTransition(int maxRemaining) {
        if (maxRemaining > 0) {
            foreach (var t in currentState.transitions) {
                if (t.Value.condition == null) {
                    Debug.LogWarning("Warning: Transition from state " + currentState.name + " to " + t.Key + " does not have a condition.");
                } else if (t.Value.condition()) {
                    CallIfExists(t.Value.notify);

                    currentState = t.Value.target;
                    
                    return TryTransition(maxRemaining - 1);
                }
            }
        }

        return currentState;
    }

    void CallIfExists(StateMachineBinding.Notify notify) {
        if (notify != null) {
            notify();
        }
    }
}
