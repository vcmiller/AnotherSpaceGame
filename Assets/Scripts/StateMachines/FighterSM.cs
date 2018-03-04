using UnityEngine;
using SBR;

public abstract class FighterSM : SBR.StateMachine {
    public enum StateID {
        Pursue, Evade, Reversal, AvoidWall
    }

    private class State {
        public StateID id;

        public Notify enter;
        public Notify during;
        public Notify exit;

        public Transition[] transitions;
    }

    private class Transition {
        public State from;
        public State to;

        public Notify notify;
        public Condition cond;
    }

    private State[] states;
    private State currentState;

    public FighterSM() {
        states = new State[4];

        State statePursue = new State();
        statePursue.id = StateID.Pursue;
        statePursue.during = State_Pursue;
        statePursue.transitions = new Transition[1];
        states[0] = statePursue;

        State stateEvade = new State();
        stateEvade.id = StateID.Evade;
        stateEvade.during = State_Evade;
        stateEvade.transitions = new Transition[1];
        states[1] = stateEvade;

        State stateReversal = new State();
        stateReversal.id = StateID.Reversal;
        stateReversal.during = State_Reversal;
        stateReversal.transitions = new Transition[1];
        states[2] = stateReversal;

        State stateAvoidWall = new State();
        stateAvoidWall.id = StateID.AvoidWall;
        stateAvoidWall.during = State_AvoidWall;
        stateAvoidWall.transitions = new Transition[1];
        states[3] = stateAvoidWall;

        currentState = statePursue;

        Transition transitionPursueEvade = new Transition();
        transitionPursueEvade.from = statePursue;
        transitionPursueEvade.to = stateEvade;
        transitionPursueEvade.cond = TransitionCond_Pursue_Evade;
        statePursue.transitions[0] = transitionPursueEvade;

        Transition transitionEvadeReversal = new Transition();
        transitionEvadeReversal.from = stateEvade;
        transitionEvadeReversal.to = stateReversal;
        transitionEvadeReversal.cond = TransitionCond_Evade_Reversal;
        stateEvade.transitions[0] = transitionEvadeReversal;

        Transition transitionReversalPursue = new Transition();
        transitionReversalPursue.from = stateReversal;
        transitionReversalPursue.to = statePursue;
        transitionReversalPursue.cond = TransitionCond_Reversal_Pursue;
        stateReversal.transitions[0] = transitionReversalPursue;

        Transition transitionAvoidWallPursue = new Transition();
        transitionAvoidWallPursue.from = stateAvoidWall;
        transitionAvoidWallPursue.to = statePursue;
        transitionAvoidWallPursue.cond = TransitionCond_AvoidWall_Pursue;
        stateAvoidWall.transitions[0] = transitionAvoidWallPursue;


    }

    public StateID state {
        get {
            return currentState.id;
        }

        set {
            foreach (var s in states) {
                if (s.id == value) {
                    TransitionTo(s);
                    return;
                }
            }
        }
    }

    public override string stateName {
        get {
            return state.ToString();
        }

        set {
            try {
                state = (StateID)System.Enum.Parse(typeof(StateID), value);
            } catch (System.ArgumentException) {
                throw new System.ArgumentException("Given string is not a valid state name!");
            }
        }
    }

    public override void Initialize() {
        base.Initialize();

        CallIfSet(currentState.enter);
    }

    public override void GetInput() {
        base.GetInput();
        CallIfSet(currentState.during);

        State cur = currentState;

        for (int i = 0; i < maxTransitionsPerUpdate; i++) {
            bool found = false;

            foreach (var t in cur.transitions) {
                if (t.cond()) {
                    CallIfSet(t.notify);
                    cur = t.to;
                    found = true;
                }
            }

            if (!found) {
                break;
            }
        }

        if (cur != currentState) {
            TransitionTo(cur);
        }
    }

    private void TransitionTo(State target) {
        CallIfSet(currentState.exit);
        currentState = target;
        CallIfSet(target.enter);
    }

    protected virtual void State_Pursue() { }
    protected virtual void State_Evade() { }
    protected virtual void State_Reversal() { }
    protected virtual void State_AvoidWall() { }

    protected virtual bool TransitionCond_Pursue_Evade() { return false; }
    protected virtual bool TransitionCond_Evade_Reversal() { return false; }
    protected virtual bool TransitionCond_Reversal_Pursue() { return false; }
    protected virtual bool TransitionCond_AvoidWall_Pursue() { return false; }

}
