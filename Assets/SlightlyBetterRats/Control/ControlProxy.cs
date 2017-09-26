using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlProxy : MonoBehaviour {
    private Dictionary<string, InputChannel> channels;

    private class InputChannel {
        public InputChannel(object def, bool clears) {
            value = defaultValue = def;
            this.clears = clears;
        }

        public void Set(object val) {
            value = val;
        }

        public object Get() {
            return value;
        }

        public void Clear() {
            if (clears) {
                value = defaultValue;
            }
        }

        private object defaultValue;
        private object value;
        private bool clears;
    }

    protected void RegisterInputChannel(string name, object defaultValue, bool clears) {
        channels[name] = new InputChannel(defaultValue, clears);
    }

    protected void SetInput(string name, object value) {
        channels[name].Set(value);
    }

    protected T GetInput<T>(string name) {
        return (T)channels[name].Get();
    }

    protected void SetFloat(string name, float value, float min = Mathf.NegativeInfinity, float max = Mathf.Infinity) {
        SetInput(name, Mathf.Clamp(value, min, max));
    }

    protected float GetFloat(string name) {
        return GetInput<float>(name);
    }

    protected void SetVector(string name, Vector3 value, float maxLength = Mathf.Infinity) {
        if (value.sqrMagnitude > maxLength * maxLength) {
            value = value.normalized * maxLength;
        }

        SetInput(name, value);
    }

    protected Vector3 GetVector(string name) {
        return GetInput<Vector3>(name);
    }

    protected void SetBool(string name, bool value) {
        SetInput(name, value);
    }

    protected bool GetBool(string name) {
        return GetInput<bool>(name);
    }

    protected void SetInt(string name, int value, int min = int.MinValue, int max = int.MaxValue) {
        SetInput(name, Mathf.Clamp(value, min, max));
    }

    protected int GetInt(string name) {
        return GetInput<int>(name);
    }

    protected void SetQuaternion(string name, Quaternion value) {
        SetInput(name, value);
    }

    protected Quaternion GetQuaternion(string name) {
        return GetInput<Quaternion>(name);
    }

    protected virtual void Awake() {
        channels = new Dictionary<string, InputChannel>();
    }

    private void Update() {
        foreach (var ctrl in GetComponents<Controller>()) {
            if (ctrl.enabled) {
                ctrl.GetInput();
            }
        }

        foreach (var motr in GetComponentsInChildren<Motor>()) {
            if (motr.enabled) {
                motr.TakeInput();
            }
        }

        ClearInput();
    }

    private void ClearInput() {
        foreach (var input in channels) {
            input.Value.Clear();
        }
    }
}
