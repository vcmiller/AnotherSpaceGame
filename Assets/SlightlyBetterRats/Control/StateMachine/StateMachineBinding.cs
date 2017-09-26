using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class StateMachineBinding {
    public class State {
        public State(string name) {
            this.name = name;
        }

        public string name;
        public Notify enter = null;
        public Notify exit = null;
        public Notify during = null;

        public Dictionary<string, Transition> transitions = new Dictionary<string, Transition>();
    }

    public class Transition {
        public Transition(State target) {
            this.target = target;
        }

        public Notify notify = null;
        public Condition condition = null;
        public State target = null;
    }

    public delegate bool Condition();
    public delegate void Notify();

    public State defaultState { get; private set; }
    private Dictionary<string, State> states;

	public StateMachineBinding(StateMachineDefinition def) {
        states = new Dictionary<string, State>();

        foreach (var stateDef in def.states) {
            State state = new State(stateDef.name);
            states.Add(stateDef.name, state);
        }

        foreach (var stateDef in def.states) {
            State begin = states[stateDef.name];
            foreach (string transition in stateDef.transitions) {
                State end = states[transition];
                if (end != null) {
                    begin.transitions.Add(transition, new Transition(end));
                } else {
                    Debug.LogWarning("Warning: Transition to unknown state " + transition + " in file " + def.name);
                }
            }
        }

        if (!states.ContainsKey(def.defaultState)) {
            Debug.LogWarning("Warning: Default state " + def.defaultState + " not found.");
            defaultState = states[def.states[0].name];
        } else {
            defaultState = states[def.defaultState];
        }
    }

    public void bindToObject(object obj) {
        foreach (MethodInfo m in obj.GetType().GetMethods()) {
            if (m.Name.StartsWith("State_")) {
                var param = m.GetParameters();

                if (param.Length == 0) {
                    string state = m.Name.Substring(6);
                    BindStateDuring(state, (Notify)Delegate.CreateDelegate(typeof(Notify), obj, m));
                } else {
                    Debug.LogWarning("Warning: State event handler " + m.Name + " should take no arguments.");
                }
            } else if (m.Name.StartsWith("StateEnter_")) {
                var param = m.GetParameters();

                if (param.Length == 0) {
                    string state = m.Name.Substring(11);
                    BindStateEnter(state, (Notify)Delegate.CreateDelegate(typeof(Notify), obj, m));
                } else {
                    Debug.LogWarning("Warning: State event handler " + m.Name + " should take no arguments.");
                }
            } else if (m.Name.StartsWith("StateExit_")) {
                var param = m.GetParameters();

                if (param.Length == 0) {
                    string state = m.Name.Substring(10);
                    BindStateExit(state, (Notify)Delegate.CreateDelegate(typeof(Notify), obj, m));
                } else {
                    Debug.LogWarning("Warning: State event handler " + m.Name + " should take no arguments.");
                }
            } else if (m.Name.StartsWith("TransitionCond_")) {
                var param = m.GetParameters();

                if (param.Length == 0) {
                    int div = m.Name.LastIndexOf('_');
                    if (div < 0 || div == m.Name.Length - 1) {
                        Debug.LogWarning("Warning: Bad format in transition function name. Should be TransitionCond_State1_State2.");
                    }

                    string state1 = m.Name.Substring(15, div - 15);
                    string state2 = m.Name.Substring(div + 1);
                    BindTransitionCondition(state1, state2, (Condition)Delegate.CreateDelegate(typeof(Condition), obj, m));
                } else {
                    Debug.LogWarning("Warning: State transition function " + m.Name + " should take no arguments.");
                }
            } else if (m.Name.StartsWith("TransitionNotify_")) {
                var param = m.GetParameters();

                if (param.Length == 0) {
                    int div = m.Name.LastIndexOf('_');
                    if (div < 0 || div == m.Name.Length - 1) {
                        Debug.LogWarning("Warning: Bad format in transition function name. Should be TransitionCond_State1_State2.");
                    }

                    string state1 = m.Name.Substring(17, div - 17);
                    string state2 = m.Name.Substring(div + 1);
                    BindTransitionNotify(state1, state2, (Notify)Delegate.CreateDelegate(typeof(Notify), obj, m));
                } else {
                    Debug.LogWarning("Warning: State transition function " + m.Name + " should take no arguments.");
                }
            }
        }
    }

    public State getState(string stateName) {
        State state = states[stateName];
        if (state == null) {
            Debug.LogWarning("Warning: State " + stateName + " not found.");
        }
        return state;
    }

    public Transition getTransition(string beginName, string endName) {
        State begin = getState(beginName);
        State end = getState(endName);

        if (begin != null && end != null) {
            Transition transition = begin.transitions[endName];
            if (transition == null) {
                Debug.LogWarning("Warning: Transition from state " + beginName + " to " + endName + " not found.");
            } else {
                return transition;
            }
        }

        return null;
    }

    public void BindStateDuring(string stateName, Notify notify) {
        State state = getState(stateName);
        if (state != null) {
            state.during = notify;
        }
    }

    public void BindStateEnter(string stateName, Notify notify) {
        State state = getState(stateName);
        if (state != null) {
            state.enter = notify;
        }
    }

    public void BindStateExit(string stateName, Notify notify) {
        State state = getState(stateName);
        if (state != null) {
            state.exit = notify;
        }
    }
    
    public void BindTransitionCondition(string beginName, string endName, Condition cond) {
        Transition transition = getTransition(beginName, endName);
        if (transition != null) {
            transition.condition = cond;
        }
    }

    public void BindTransitionNotify(string beginName, string endName, Notify notify) {
        Transition transition = getTransition(beginName, endName);
        if (transition != null) {
            transition.notify = notify;
        }
    }

}
