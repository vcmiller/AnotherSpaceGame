using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFighterController : PlayerController<FighterProxy> {
    public void Axis_Horizontal(float value) {
        controlled.rotation += transform.forward * -value;
    }

    public void Axis_Vertical(float value) {
        controlled.thrust = value;
    }

    public void Axis_LookHorizontal(float value) {
        controlled.rotation += transform.up * value;
    }

    public void Axis_LookVertical(float value) {
        controlled.rotation += transform.right * value;
    }
}
