using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Instead of strings, the keybindings will use a public enum to describe
// each input action in both "written" and numeric form
public enum InputAction
{
    moveUp = 0,
    moveDown = 1,
    moveLeft = 2,
    moveRight = 3,
    jump = 4,
    attack = 5,
    nextItem = 6,
    prevItem = 7
}

// This is a hack-y sort of way to make it simple to manually input defined keybindings
// from the inspector. This struct is an object that simply stores an InputAction and a
// corresponding KeyCode for the action to be bound to.
[System.Serializable] // This allows the struct to be serializable/visible within the Unity inspector window
public struct Keybind
{
    public InputAction action;
    public KeyCode key;
}


public class InputManager : MonoBehaviour
{
    [Header("Singleton Instance")]
    
    [Tooltip("This singleton reference for the input manager.")]
    public static InputManager _inst;

    [Header("Keybind Data")]
    public bool manualInput = false;
    public int _numberOfActions = 0;

    [Tooltip("Manually Input Keybindings here.")]
    [SerializeField] public List<Keybind> _keyBindings_test = new List<Keybind>();

    // This dictionary is where the actual bindings should be accessed
    public Dictionary<InputAction, KeyCode> _keyBindings = new Dictionary<InputAction, KeyCode>();

    void Awake()
    {
        // Confirm singleton instance is active
        if (_inst == null)
        {
            _inst = this;
            DontDestroyOnLoad(this);   
        }
        else if (_inst != this)
            GameObject.Destroy(this);
    }

    void Start()
    {
        // manually feed each input into the dict
        if (manualInput == true)
        {
            foreach (Keybind input in _keyBindings_test)
                _keyBindings.Add(input.action, input.key);
        }
    }

    void Update() {}

    // void FixedUpdate() {}
}
