using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMusicOnLoad : MonoBehaviour
{
    public AudioClip _songToPlay = null;

    [SerializeField, ReadOnly] private bool isPlaying = false;

    void Start()
    {
    }

    void Update()
    {
        if (!isPlaying)
        {
            if (AudioManager._inst != null)
                AudioManager._inst.PlayMusic(_songToPlay);
            isPlaying = true;
        }
    }
}
