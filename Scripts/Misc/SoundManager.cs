// Polaris - Project VII 2018
// 08/03/18 -> 02/07/18
// Compiled in Unity 2017.2.0f3
// Writed by Mathias Ody (M-Ody)
// mathiasluizody@hotmail.com
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PolarisCore;

public class SoundManager : MonoBehaviour 
{
    private static SoundManager instance;

    private void Awake()
    {
        instance = gameObject.GetComponent<SoundManager>();
    }

    public static void Play(int key, Transform t)
    {
        instance.PPlay(key, t);
    }

    public static void Play2D(int key)
    {
        instance.PPlay2D(key);
    }

    private void PPlay(int key, Transform t)
    {
        var sound = Pool.Instance.GetNotActivated(key);
        sound.transform.position = t.position;
        sound.GetComponent<PlayAudio>().Play();
    }

    private void PPlay2D(int key)
    {
        var sound = Pool.Instance.GetNotActivated(key);
        sound.GetComponent<PlayAudio>().Play();
    }
}
