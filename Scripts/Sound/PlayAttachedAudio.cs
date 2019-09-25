// Polaris - Project VII 2018
// 08/03/18 -> 02/07/18
// Compiled in Unity 2017.2.0f3
// Writed by Mathias Ody (M-Ody)
// mathiasluizody@hotmail.com
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAttachedAudio : MonoBehaviour 
{
    private AudioSource source;

    public void OnEnable()
    {
        if (source == null)
            source = GetComponent<AudioSource>();
        source.pitch = Random.Range(0.8f,1.1f);
        source.Play();
    }
}
