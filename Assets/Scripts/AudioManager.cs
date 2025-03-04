using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public Sound[] musicSounds, sfxSounds;
    public AudioSource musicSource, sfxSource, loopingsfxSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayMusic(string name)
    {

        Sound s = Array.Find(musicSounds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("Sound not found");
        }

        else 
        {
            musicSource.clip = s.clip;
            musicSource.loop = false;
            musicSource.Play();
        }
    }

    public void StopMusic()
    {
            musicSource.Stop();
    }

    public void PlayLoopingMusic(string name)
    {

        Sound s = Array.Find(musicSounds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("Sound not found");
        }

        else 
        {
            musicSource.clip = s.clip;
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    public void PlaySFX(string name)
    {
        Sound s = Array.Find(sfxSounds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("Sound not found");
        }

        else
        {
            if(s.name == "Throw" || s.name == "BirdIdle")
            {
                sfxSource.Stop(); //makes sure the previous sounds aren't playing when making new throw to stop audio from breaking
            }
            sfxSource.PlayOneShot(s.clip);
        }
    }

    public void PlayLoopingSFX(string name)
    {
        Sound s = Array.Find(sfxSounds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("Sound not found");
        }
        else
        {
            loopingsfxSource.clip = s.clip;
            loopingsfxSource.loop = true; // Enable looping
            loopingsfxSource.Play();
        }
    }

    public void StopLoopingSFX()
    {
        if (loopingsfxSource.isPlaying && loopingsfxSource.loop)
        {
            loopingsfxSource.Stop();
            loopingsfxSource.loop = false; // Reset looping to avoid affecting other sounds
        }
    }

    public void RestartMusic(string name)
    {
        Sound s = Array.Find(musicSounds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("Sound not found");
        }
        else
        {
            musicSource.Stop(); // Stop current music
            musicSource.clip = s.clip;
            musicSource.Play(); // Play from the beginning
        }
    }

}
