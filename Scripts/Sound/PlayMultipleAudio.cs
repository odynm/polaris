// Polaris - Project VII 2018
// 08/03/18 -> 02/07/18
// Compiled in Unity 2017.2.0f3
// Writed by Mathias Ody (M-Ody)
// mathiasluizody@hotmail.com
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMultipleAudio : PlayAudio 
{
    private AudioSource[] _sources;

    private void Start()
    {
        _sources = GetComponents<AudioSource>();
    }

    public override void Play()
    {
        _source = _sources[Random.Range(0, _sources.Length)];

        if (randomizePitch)
        {
            randomPitch = Random.Range(0.85f, 1.15f);
            _source.pitch = (randomPitch);
            randomPitch = Random.Range(0.85f, 1.15f);
            _source.volume = _source.volume + (randomPitch - 1f);
        }
        _source.Play();
    }
}
