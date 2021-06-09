using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Audio;
public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    void Awake()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
        }
    }


    public void Play(string name)
    {
        Sound s = Array.Find(sounds, Sound => Sound.name == name);
        s.source.Play();
    }

    // Overload that lets you play a sound from a specific point
    public void Play(string name, Vector3 pos)
    {
        Sound s = Array.Find(sounds, Sound => Sound.name == name);
        AudioSource.PlayClipAtPoint(s.clip, pos);
    }

    // Overload that lets you play a sound from an object
    public void Play(string name, GameObject obj)
    {
        Sound s = Array.Find(sounds, Sound => Sound.name == name);
        AudioSource audio = obj.AddComponent<AudioSource>();
        audio = s.source;
        audio.spatialBlend = 1.0f;
        audio.Play();
    }

    public void StopPlaying(string name)
    {
        Sound s = Array.Find(sounds, Sound => Sound.name == name);
        s.source.Stop();
    }

    public void Pause(string name)
    {
        Sound s = Array.Find(sounds, Sound => Sound.name == name);
        s.source.Pause();
    }
}


