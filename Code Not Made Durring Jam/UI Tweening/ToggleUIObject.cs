using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleUIObject : MonoBehaviour
{
    [Header("Object Data")]
    public GameObject objectToToggle = null;
    [SerializeField] public List<GameObject> toggleObjects = new List<GameObject>();
    // public bool startState = false;

    /*
    public Vector3 originalPos = Vector3.zero;
    public Vector3 originalScale = Vector3.zero;
    public Vector3 newPos = Vector3.zero;
    public Vector3 newScale = Vector3.zero;

    [Header("Tween Data")]
    public LeanTweenType easeIn;
    public LeanTweenType easeOut;
    public float timeToTween = 0f;
    public float delayTime = 0f;
    */


    void Awake()
    {
        if (objectToToggle == null)
            Debug.Log("Warning. reference to 'objectToToggle' on " + gameObject.name + " is null.");
    }

    void Start() 
    {
        // if (objectToToggle != null)
        //     objectToToggle.SetActive(startState);
    }

    public void ToggleObject()
    {
        if (objectToToggle != null)
        {
            if (!objectToToggle.activeSelf)
                objectToToggle.SetActive(!objectToToggle.activeSelf);
            else
            {
                if (objectToToggle.GetComponent<UITweener>() != null)
                    objectToToggle.GetComponent<UITweener>().DisableSelf();
            }
        }
    }

    public void ToggleObject(GameObject obj)
    {
        if (obj != null)
        {
            if (!obj.activeSelf)
                obj.SetActive(!obj.activeSelf);
            else
            {
                if (obj.GetComponent<UITweener>() != null)
                    obj.GetComponent<UITweener>().DisableSelf();
                else
                    obj.SetActive(!obj.activeSelf);
            }
        }
    }

    public void ToggleMultipleObjects()
    {
        if (toggleObjects.Count <= 0)
            return;
        
        foreach (GameObject ob in toggleObjects)
            ToggleObject(ob);
    }
}
