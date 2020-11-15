using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager _inst;

    [Header("Audio Sources")]
    [SerializeField] public List<AudioClip> _musicClips = new List<AudioClip>();
    [SerializeField] public List<AudioClip> _sfxClips = new List<AudioClip>();
    [SerializeField, ReadOnly] public List<AudioSource> _musicSources = new List<AudioSource>();
    [SerializeField, ReadOnly] public List<AudioSource> _sfxSources = new List<AudioSource>();


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
        // Initialize all audio clips with an associated source
        for (int i = 0; i < _musicClips.Count; i++)
        {
            _musicSources.Add(new AudioSource());
            _musicSources[i] = gameObject.AddComponent<AudioSource>();
            _musicSources[i].playOnAwake = false;
            _musicSources[i].clip = _musicClips[i];
        }

        for (int i = 0; i < _sfxClips.Count; i++)
        {
            _sfxSources.Add(new AudioSource());
            _sfxSources[i] = gameObject.AddComponent<AudioSource>();
            _sfxSources[i].playOnAwake = false;
            _sfxSources[i].clip = _sfxClips[i];
        }
    }

    public void PlaySFX(int index)
    {
        // avoid index out of range exception
        if (index > (_sfxClips.Count - 1))
            return;

        _sfxSources[index].volume = GameSettings._inst._sfxVolume;

        _sfxSources[index].PlayOneShot(_sfxSources[index].clip);
    }

    public void PlayMusic(int index)
    {
        if (GameSettings._inst == null)
            return;

        foreach (AudioSource source in _musicSources)
        {
            if (source.isPlaying)
            {
                source.Stop();
            }
        }

        for (int i = 0; i < _musicSources.Count; i++)
        {
            if (i == index)
            {
                if (!_musicSources[i].isPlaying)
                {
                    _musicSources[i].Play();
                    _musicSources[i].volume = GameSettings._inst._musicVolume;
                    _musicSources[i].loop = true;
                }
            }
        }
    }

    public void PlayMusic(AudioClip toPlay)
    {
        if (GameSettings._inst == null)
            return;

        foreach (AudioSource source in _musicSources)
        {
            if (source.isPlaying)
            {
                source.Stop();
            }
        }


        foreach (AudioSource source in _musicSources)
        {
            if (source.clip == toPlay)
            {
                source.Play();
                source.volume = GameSettings._inst._musicVolume;
                source.loop = true;
            }
        }
    }
}
