using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeybindUIBox : MonoBehaviour
{
    public InputAction _action;
    public Text actionName = null;
    public Text keyName = null;

    // private string _prevActionName = "";
    // private string _prevKeyName = "";

    void Start()
    {
        if (InputManager._inst != null)
        {
            keyName.text = InputManager._inst._keyBindings[_action].ToString();
            actionName.text = _action.ToString();
        }
    }
}
