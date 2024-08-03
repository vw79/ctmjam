using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume = 0.7f;

    public bool loop = false;
    public bool playOnAwake = false;

    [HideInInspector]
    public AudioSource source;
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public Sound[] sounds;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.loop = s.loop;
            s.source.playOnAwake = s.playOnAwake;

            if (s.playOnAwake)
            {
                s.source.Play();
            }
        }
    }

    public void Play(string soundName)
    {
        Sound s = Array.Find(sounds, sound => sound.name == soundName);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + soundName + " not found!");
            return;
        }
        s.source.Play();
    }

    public void Stop(string soundName)
    {
        Sound s = Array.Find(sounds, sound => sound.name == soundName);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + soundName + " not found!");
            return;
        }
        s.source.Stop();
    }

    public AudioClip GetClip(string clipName)
    {
        Sound s = Array.Find(sounds, sound => sound.name == clipName);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + clipName + " not found!");
            return null;
        }
        return s.clip;
    }
}
