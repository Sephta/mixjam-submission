using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITweener : MonoBehaviour
{
    public enum PosType
    {
        posX,
        posY,
        both
    }
    public enum UITweenSetting
    {
        no = 0,
        yes = 1,
    }


    [Header("Object")]
    public RectTransform objectToTween = null;
    public bool startState = false;

    [Header("Tween Data")]
    public LeanTweenType easeIn;
    public LeanTweenType easeOut;
    public float timeToTween_in = 0f;
    public float timeToTween_out = 0f;
    public float delayTime_in = 0f;
    public float delayTime_out = 0f;

    [Header("Positional Data")]
    public Vector3 originalPos = Vector3.zero;
    public PosType desiredPosType = PosType.posX;
    public float desiredPosX = 0f;
    public float desiredPosY = 0f;

    [Header("Settings")]
    public UITweenSetting fadeOut = UITweenSetting.no;
    public UITweenSetting destroyAfterCompletion = UITweenSetting.no;


    void Awake()
    {
        if (objectToTween == null)
            Debug.Log("Warning. reference to 'objectToTween' on " + gameObject.name + " is null.");
    }

    void OnEnable()
    {
        if (objectToTween != null)
        {
            switch(desiredPosType)
            {
                case PosType.posX:
                    LeanTween.moveX(objectToTween, desiredPosX, timeToTween_in).setEase(easeIn).setDelay(delayTime_in);
                    break;

                case PosType.posY:
                    if (destroyAfterCompletion == UITweenSetting.yes)
                        LeanTween.moveY(objectToTween, desiredPosY, timeToTween_in).setEase(easeIn).setDelay(delayTime_in).setOnComplete(DestroySelf);
                    else
                        LeanTween.moveY(objectToTween, desiredPosY, timeToTween_in).setEase(easeIn).setDelay(delayTime_in);
                    break;

                case PosType.both:
                    LeanTween.move(objectToTween, new Vector3(desiredPosX, desiredPosY, 0f), timeToTween_in).setDelay(delayTime_in);
                    break;
            }

            if (fadeOut == UITweenSetting.yes)
                LeanTween.alpha(this.gameObject, 0, timeToTween_in).setDelay(delayTime_in);
        }
    }

    private void SetActiveFalse()
    {
        gameObject.SetActive(false);
    }

    public void DestroySelf()
    {
        Destroy(this.gameObject);
    }

    public void DisableSelf()
    {
        LeanTween.move(objectToTween, originalPos, timeToTween_out).setDelay(delayTime_out).setEase(easeOut).setOnComplete(SetActiveFalse);
    }
}
