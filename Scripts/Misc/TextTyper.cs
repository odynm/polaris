// Polaris - Project VII 2018
// 08/03/18 -> 02/07/18
// Compiled in Unity 2017.2.0f3
// Writed by Mathias Ody (M-Ody)
// mathiasluizody@hotmail.com
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent (typeof (Text))]
[RequireComponent (typeof (RectTransform))]

//https://georgeblackwell.itch.io/unity-ui-typewriter
public class TextTyper : MonoBehaviour 
{    
    [SerializeField] private float typeSpeed;
    [SerializeField] private float startDelay;
    //[SerializeField] private float volumeVariation;
    [SerializeField] private bool startOnAwake;

    private int counter;
    private string textToType;
    private bool typing;
    private Text textComp;
    //private AudioSource audioComp;

    void Awake()
{    
        textComp = GetComponent<Text>();
        //audioComp = GetComponent<AudioSource>();

        counter = 0;
        textToType = textComp.text;
        textComp.text = "";

        if(startOnAwake)
        {
            StartTyping();
        }
    }

    public void StartTyping()
    {    
        if(!typing)
        {
            InvokeRepeating("Type", startDelay, typeSpeed);
        }
        else
        {
            print(gameObject.name + " : Is already typing!");
        }
    }

    public void StopTyping()
    {
        counter = 0;
        typing = false;
        CancelInvoke("Type");
    }

    public void UpdateText(string newText)
    {   
        StopTyping();
        textComp.text = "";
        textToType = newText;
        StartTyping();
    }

    private void Type()
    {    
        typing = true;
        textComp.text = textComp.text + textToType[counter];
        //audioComp.Play();
        counter++;

        RandomiseVolume();

        if(counter == textToType.Length)
        {    
            typing = false;
            CancelInvoke("Type");
        }
    }

    private void RandomiseVolume()
    {
        //audioComp.volume = Random.Range(1 - volumeVariation, volumeVariation + 1);
    }

    public bool IsTyping() { return typing; }
}
