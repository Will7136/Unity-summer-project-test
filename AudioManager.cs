using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
  //  private bool IsPlaying = false;

    void Awake(){
        foreach(Sound s in sounds)
        {
            s.scource = gameObject.AddComponent<AudioSource>();
            s.scource.clip = s.clip;

            s.scource.volume = s.volume;
            s.scource.pitch = s.pitch;
        }
    }

    public void Play (string name){
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.scource.Play();
     //   if (s.isPlaying){
    //        IsPlaying = true;
    //    }
    //    else if (!s.isPlaying){
    //        IsPlaying = false;
    //    }
    //    return IsPlaying;
    }

}
