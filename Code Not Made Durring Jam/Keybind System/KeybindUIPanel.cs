using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeybindUIPanel : MonoBehaviour
{
    // PUBLIC VARS
    [ReadOnly] public bool rebindingKey = false;
    [SerializeField] public List<KeybindUIBox> _keybindNames = new List<KeybindUIBox>();


    // PRIVATE VARS
    private Dictionary<InputAction, KeybindUIBox> _uiBindings = new Dictionary<InputAction, KeybindUIBox>();
    private InputAction _actionToChange;


    void Start()
    {
        // Add the UI keybind boxes to a dictionary corresponding to their
        // input action.
        for (int i = 0; i < InputManager._inst._numberOfActions; i++)
        {
            _uiBindings.Add((InputAction)i, _keybindNames[i]);
        }

        // Debug check to make sure dict is working
        // for (int i = 0; i < _uiBindings.Count; i++)
        // {
        //     Debug.Log(((InputAction)i).ToString() + ": " + _uiBindings[(InputAction)i].actionName.text);
        // }
    }

    void Update()
    {
        if (rebindingKey)
        {
            gameObject.GetComponent<Image>().color = Color.red;
            if (Input.anyKeyDown)
            {
                foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
                {
                    if (Input.GetKeyDown(key))
                    {
                        // Check if the key is already bound to another action.
                        bool isKeyBound = false;
                        for (int i = 0; i < InputManager._inst._keyBindings.Count; i++)
                        {
                            if (key == InputManager._inst._keyBindings[(InputAction)i] && (InputAction)i != _actionToChange)
                            {
                                isKeyBound = true;
                            }
                        }
                        
                        // If the key is bound then break the loop, and "don't" bind 
                        // the action to a key.
                        if (isKeyBound)
                        {
                            Debug.Log("Cannot bind one key to multiple actions.");
                            gameObject.GetComponent<Image>().color = Color.white;
                            rebindingKey = false;
                            break;
                        }

                        // Update the input inside of the input manager, and change necessary
                        // text fields for the UI menu.
                        InputManager._inst._keyBindings[_actionToChange] = key;
                        _uiBindings[_actionToChange].actionName.text = _actionToChange.ToString();
                        _uiBindings[_actionToChange].keyName.text = key.ToString();
                        
                        // Change these back to orgininal values.
                        gameObject.GetComponent<Image>().color = Color.white;
                        rebindingKey = false;
                    }
                }
            }
        }
    }

    // void FixedUpdate() {}

    public void OnKeybindValueChange(int action)
    {
        _actionToChange = (InputAction)action;
        rebindingKey = true;
    }
}
