using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeybindTest : MonoBehaviour
{
    public Text textField = null;

    // void Awake() {}

    void Start()
    {
        if (textField == null && gameObject.GetComponent<Text>() != null)
            textField = gameObject.GetComponent<Text>();
    }

    void Update()
    {
        if (Input.GetKeyDown(InputManager._inst._keyBindings[InputAction.attack]))
            textField.text = "Key: " + InputManager._inst._keyBindings[InputAction.attack].ToString() + "\nAction: " + InputAction.attack.ToString();
        
        if (Input.GetKeyDown(InputManager._inst._keyBindings[InputAction.jump]))
            textField.text = "Key: " + InputManager._inst._keyBindings[InputAction.jump].ToString() + "\nAction: " + InputAction.jump.ToString();
        
        if (Input.GetKeyDown(InputManager._inst._keyBindings[InputAction.moveDown]))
            textField.text = "Key: " + InputManager._inst._keyBindings[InputAction.moveDown].ToString() + "\nAction: " + InputAction.moveDown.ToString();
        
        if (Input.GetKeyDown(InputManager._inst._keyBindings[InputAction.moveLeft]))
            textField.text = "Key: " + InputManager._inst._keyBindings[InputAction.moveLeft].ToString() + "\nAction: " + InputAction.moveLeft.ToString();
        
        if (Input.GetKeyDown(InputManager._inst._keyBindings[InputAction.moveUp]))
            textField.text = "Key: " + InputManager._inst._keyBindings[InputAction.moveUp].ToString() + "\nAction: " + InputAction.moveUp.ToString();
        
        if (Input.GetKeyDown(InputManager._inst._keyBindings[InputAction.moveRight]))
            textField.text = "Key: " + InputManager._inst._keyBindings[InputAction.moveRight].ToString() + "\nAction: " + InputAction.moveRight.ToString();
    }

    // void FixedUpdate() {}
}
