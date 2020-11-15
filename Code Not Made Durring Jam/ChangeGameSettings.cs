using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ChangeGameSettings : MonoBehaviour
{
    public Slider _sfxSlider = null;
    public Slider _musicSlider = null;

    private float timeSinceLastPlay = 0f;


    // void Start() {}

    public void ChangeSFXVolume()
    {
        if (GameSettings._inst != null && _sfxSlider != null)
        {
            GameSettings._inst._sfxVolume = _sfxSlider.value;
            if (AudioManager._inst != null)
            {
                if (AudioManager._inst._sfxSources.Count <= 0)
                    return;

                if ((Time.time - timeSinceLastPlay) >= 0.125f)
                {
                    AudioManager._inst._sfxSources[5].volume = _sfxSlider.value;
                    AudioManager._inst._sfxSources[5].PlayOneShot(AudioManager._inst._sfxSources[5].clip);

                    timeSinceLastPlay = Time.time;
                }
            }
        }
    }

    public void ChangeMusicVolume()
    {
        if (GameSettings._inst != null && _musicSlider != null)
        {
            GameSettings._inst._musicVolume = _musicSlider.value;
            if (AudioManager._inst != null)
            {
                foreach (AudioSource source in AudioManager._inst._musicSources)
                {
                    if (source.isPlaying)
                        source.volume = _musicSlider.value;
                }
            }
        }
    }
}
