using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace SBR.Editor {
    public class MakeTransitionOperation : Operation {
        private Vector2 target;

        public MakeTransitionOperation(StateMachineDefinition def, StateMachineEditorWindow window, StateMachineDefinition.State state) : base(def, window, state) {
            showBaseGUI = true;
        }

        public override void Update() {
            var evt = Event.current;
            if (evt.type == EventType.MouseMove) {
                target = evt.mousePosition;
                repaint = true;
            } else if (evt.type == EventType.MouseDown) {
                var targ = definition.SelectState(target + window.scrollPos);

                if (evt.button == 0 && targ != null && targ != state) {
                    done = true;
                    Confirm();
                } else {
                    done = true;
                    Cancel();
                }
            }
        }

        public override void Cancel() {
        }

        public override void Confirm() {
            var targ = definition.SelectState(target + window.scrollPos);

            state.AddTransition(targ);
        }

        public override void OnGUI() {
            Handles.BeginGUI();

            Vector2 src = state.center;

            var targ = definition.SelectState(target + window.scrollPos);

            if (targ == null) {
                Handles.color = Color.red;
                Handles.DrawAAPolyLine(3, src - window.scrollPos, target);
            } else if (targ != state) {
                if (state.GetTransition(targ) == null) {
                    Handles.color = Color.black;
                } else {
                    Handles.color = Color.red;
                }

                Handles.DrawAAPolyLine(3, src - window.scrollPos, targ.center - window.scrollPos);
            }

            Handles.EndGUI();
        }
    }
}