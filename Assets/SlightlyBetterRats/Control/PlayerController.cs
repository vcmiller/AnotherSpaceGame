using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
using UnityEngine.UI;

public class PlayerController<T> : Controller where T : ControlProxy {
    
    private Dictionary<string, ButtonHandler> buttonDown;
    private Dictionary<string, ButtonHandler> buttonUp;
    private Dictionary<string, ButtonHandler> buttonHeld;
    private Dictionary<string, AxisHandler> axes;

    public string inputSuffix;

    private delegate void ButtonHandler();
    private delegate void AxisHandler(float value);

    public T controlled { get; private set; }

    public Canvas HUDPrefab;

    public Canvas playerHUD { get; private set; }

    private ViewTarget curViewTarget;
    public ViewTarget viewTarget {
        get {
            return curViewTarget;
        }

        set {
            if (curViewTarget) {
                curViewTarget.enabled = false;
            }
            curViewTarget = value;
            if (curViewTarget) {
                curViewTarget.enabled = enabled;
            }

            if (playerHUD) {
                if (curViewTarget && curViewTarget.camera) {
                    playerHUD.worldCamera = viewTarget.camera;
                } else {
                    playerHUD.worldCamera = null;
                }
            }
        }
    }

    protected virtual void Awake() {
        controlled = GetComponent<T>();
    }
    
    protected override void Start() {
        axes = new Dictionary<string, AxisHandler>();
        buttonDown = new Dictionary<string, ButtonHandler>();
        buttonUp = new Dictionary<string, ButtonHandler>();
        buttonHeld = new Dictionary<string, ButtonHandler>();

        viewTarget = GetComponentInChildren<ViewTarget>();

        foreach (MethodInfo m in GetType().GetMethods()) {
            if (m.Name.StartsWith("Axis_")) {
                var param = m.GetParameters();

                if (param.Length == 1 && param[0].ParameterType == typeof(float)) {
                    string axis = m.Name.Substring(5);
                    if (axes.ContainsKey(axis)) {
                        Debug.LogWarning("Waring: Duplicate event handler found for axis " + axis + ".");
                    } else {
                        axes.Add(axis, (AxisHandler)Delegate.CreateDelegate(typeof(AxisHandler), this, m));
                    }
                } else {
                    Debug.LogWarning("Warning: Axis event handler " + m.Name + " should take one argument of type float.");
                }
            } else if (m.Name.StartsWith("Button_")) {
                if (m.GetParameters().Length == 0) {
                    string btn = m.Name.Substring(7);
                    if (buttonHeld.ContainsKey(btn)) {
                        Debug.LogWarning("Waring: Duplicate event handler found for button " + btn + ".");
                    } else {
                        buttonHeld.Add(btn, (ButtonHandler)Delegate.CreateDelegate(typeof(ButtonHandler), this, m));
                    }
                } else {
                    Debug.LogWarning("Warning: Button event handler " + m.Name + " should take no arguments.");
                }
            } else if (m.Name.StartsWith("ButtonUp_")) {
                if (m.GetParameters().Length == 0) {
                    string btn = m.Name.Substring(9);
                    if (buttonUp.ContainsKey(btn)) {
                        Debug.LogWarning("Waring: Duplicate event handler found for button up " + btn + ".");
                    } else {
                        buttonUp.Add(btn, (ButtonHandler)Delegate.CreateDelegate(typeof(ButtonHandler), this, m));
                    }
                } else {
                    Debug.LogWarning("Warning: ButtonUp event handler " + m.Name + " should take no arguments.");
                }
            } else if (m.Name.StartsWith("ButtonDown_")) {
                if (m.GetParameters().Length == 0) {
                    string btn = m.Name.Substring(11);
                    if (buttonDown.ContainsKey(btn)) {
                        Debug.LogWarning("Waring: Duplicate event handler found for button down " + btn + ".");
                    } else {
                        buttonDown.Add(btn, (ButtonHandler)Delegate.CreateDelegate(typeof(ButtonHandler), this, m));
                    }
                } else {
                    Debug.LogWarning("Warning: ButtonDown event handler " + m.Name + " should take no arguments.");
                }
            }
        }
        
    }

    public virtual RectTransform AddToHUD(GameObject obj) {
        if (!playerHUD) {
            CreateHUD();
        }

        var rt = obj.GetComponent<RectTransform>();
        rt.transform.SetParent(playerHUD.transform, false);
        return rt;

    }

    protected virtual void CreateHUD() {
        if (HUDPrefab) {
            playerHUD = Instantiate(HUDPrefab.gameObject).GetComponent<Canvas>();
        } else {
            GameObject hudObject = new GameObject("HUD");
            playerHUD = hudObject.AddComponent<Canvas>();
            hudObject.AddComponent<CanvasScaler>();
            hudObject.AddComponent<GraphicRaycaster>();
        }

        playerHUD.renderMode = RenderMode.ScreenSpaceCamera;
        playerHUD.planeDistance = 1f;

        if (viewTarget && viewTarget.camera) {
            playerHUD.worldCamera = viewTarget.camera;
        }
    }

    public override void GetInput() {
        if (enabled) {
            foreach (var m in axes) {
                m.Value(Input.GetAxis(m.Key + inputSuffix));
            }

            foreach (var m in buttonDown) {
                if (Input.GetButtonDown(m.Key)) {
                    m.Value();
                }
            }

            foreach (var m in buttonHeld) {
                if (Input.GetButton(m.Key)) {
                    m.Value();
                }
            }

            foreach (var m in buttonUp) {
                if (Input.GetButtonUp(m.Key)) {
                    m.Value();
                }
            }
        }
    }

    protected virtual void OnDisable() {
        if (curViewTarget) {
            curViewTarget.enabled = false;
        }
    }

    protected virtual void OnEnable() {
        if (curViewTarget) {
            curViewTarget.enabled = true;
        }

        if (!playerHUD && HUDPrefab) {
            CreateHUD();
        }
    }
}
