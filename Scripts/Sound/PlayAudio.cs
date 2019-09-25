// Polaris - Project VII 2018
// 08/03/18 -> 02/07/18
// Compiled in Unity 2017.2.0f3
// Writed by Mathias Ody (M-Ody)
// mathiasluizody@hotmail.com
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudio : MonoBehaviour 
{
    [SerializeField]
    protected bool randomizePitch;

    protected float randomPitch;

    protected AudioSource _source;

    private void Start()
    {
        _source = GetComponent<AudioSource>();
    }

    public virtual void Play()
    {
        if (randomizePitch)
        {
            randomPitch = Random.Range(0.85f, 1.15f);
            _source.pitch = (randomPitch);
            randomPitch = Random.Range(0.85f, 1.15f);
            _source.volume = _source.volume + (randomPitch - 1f);
        }
        _source.Play();
    }

    public virtual void Stop()
    {
        _source.Stop();
    }
}
