using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameSettings : MonoBehaviour
{
    public static GameSettings _inst;

    [Header("Game Settings")]
    [Range(0, 1)] public float _musicVolume = 0f;
    [Range(0, 1)] public float _sfxVolume = 0f;


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
}
