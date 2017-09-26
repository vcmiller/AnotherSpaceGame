using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachineRunner {
    public StateMachineDefinition definition { get; private set; }
    public StateMachineBinding binding { get; private set; }
    public StateMachineInstance instance { get; private set; }

    public MonoBehaviour impl { get; private set; }
    public GameObject controlled { get; private set; }

    public StateMachineRunner(GameObject controlled) {
        this.controlled = controlled;
    }

    public void Update() {
        if (impl) {
            instance.Update();
        } else {
            Debug.LogError("Error: Trying to update state machine that was either destroyed or not initialized fully.");
        }
    }

    public void Run(StateMachineDefinition newDef) {
        if (newDef != definition && impl) {
            Stop();
        }
        definition = newDef;
        if (newDef != null) {
            Type implType = Type.GetType(definition.name + "_Impl");

            if (implType == null) {
                Debug.LogError("Error: Implementation class " + definition.name + "_Impl not found.");
                definition = null;
            } else if (!implType.IsSubclassOf(typeof(MonoBehaviour))) {
                Debug.LogError("Error: Implementation class " + definition.name + "_Impl must be a subclass of MonoBehaviour.");
                definition = null;
            } else {
                impl = (MonoBehaviour)controlled.AddComponent(implType);

                binding = new StateMachineBinding(definition);
                binding.bindToObject(impl);
                instance = new StateMachineInstance(binding);
            }
        }
    }

    public void Stop() {
        UnityEngine.Object.Destroy(impl);
        impl = null;
        binding = null;
        instance = null;
        definition = null;
    }
}
