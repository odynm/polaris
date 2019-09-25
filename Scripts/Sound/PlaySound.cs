// Polaris - Project VII 2018
// 08/03/18 -> 02/07/18
// Compiled in Unity 2017.2.0f3
// Writed by Mathias Ody (M-Ody)
// mathiasluizody@hotmail.com
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour {

    public AudioSource[] soundList;
    private int listLength;
    private int index;

    private void Awake()
    {
        listLength = soundList.Length - 1;
    }

    public void Play ()
    {
        index = Random.Range(0, listLength);
        soundList[index].volume = Random.Range(0.9f, 1);
        soundList[index].pitch = Random.Range(0.9f, 1.1f);
        soundList[index].Play();
    }
}
