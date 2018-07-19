﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MyUtility;

///
/// This is a helper class for AudioManager. It is basically a wrapper for AudioClip
///  that we use to keep track of which sounds are playing.  Like PlayClipAtPoint,
///  it creates a temporary GameObject and attaches an AudioSource to it.  It is
///  destoryed when the clip finishes or Stop() is called
public class Sound
{
    
//------------------------------------------------------------------------CONSTANTS:

	private const string LOG_TAG = "Sound";

//---------------------------------------------------------------------------FIELDS:
	
    public float Duration
    {
        get
        {
            return clip.length;
        }
    }

    public bool IsPlaying
    {
        get
        {
            return tempAudioSource != null  &&  tempAudioSource.isPlaying;
        }
    }

	private AudioClip clip;
    private AudioSource tempAudioSource;
	private float volume;

//---------------------------------------------------------------------CONSTRUCTORS:

	public Sound( AudioClip clip, float volume )
	{
		this.clip = clip;
		this.volume = volume;
	}

//--------------------------------------------------------------------------METHODS:

	public void Play( Vector3 soundOrigin, 
                      string name = "",
                      bool spatial = false )
	{
        if( tempAudioSource != null )
        {
            GameObject.Destroy( tempAudioSource );
        }
        GameObject tempObject = new GameObject();
        tempObject.name = name + "Sound";
        tempAudioSource = tempObject.AddComponent<AudioSource>();
        tempAudioSource.volume = volume;
        tempAudioSource.loop = false;
        tempAudioSource.clip = clip;
        if( spatial )   tempAudioSource.spatialBlend = 1;
        tempObject.transform.position = soundOrigin;
        tempAudioSource.Play();

        tempObject.AddComponent<GameObjectDestroyer>().Destroy( clip.length );        
	}

    public void Stop()
    {
        if( tempAudioSource != null )
        {
            tempAudioSource.Stop();
            tempAudioSource.GetComponent<GameObjectDestroyer>().CancelInvoke();
            GameObject.Destroy( tempAudioSource.gameObject );
        }
    }

//--------------------------------------------------------------------------HELPERS:
	
}