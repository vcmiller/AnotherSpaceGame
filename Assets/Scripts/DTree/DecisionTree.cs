using SBR;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DecisionTree<T> : Controller where T : Channels {
    public T controlled { get; private set; }

    protected delegate bool Test();
    protected delegate void Action();

    protected Action root { private get; set; }

    protected virtual void Start() {
        controlled = channels as T;
    }

    protected Action And(params Action[] actions) {
        return () => {
            foreach (Action act in actions) {
                act();
            }
        };
    }

    protected Action BoolTest(Test test, Action ifTrue, Action ifFalse) {
        return () => {
            if (test()) {
                ifTrue();
            } else {
                ifFalse();
            }
        };
    }

    public override void GetInput() {
        if (root != null) {
            root();
        }
    }
}
