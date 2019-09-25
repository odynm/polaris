// Polaris - Project VII 2018
// 08/03/18 -> 02/07/18
// Compiled in Unity 2017.2.0f3
// Writed by Mathias Ody (M-Ody)
// mathiasluizody@hotmail.com
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PolarisCore;

public class PauseMenu : MonoBehaviour 
{
    public GameObject pauseMenu;
    public GameObject optionsMenu;
    public GameObject cross;

    [Header("Options")]
    public Slider volumeMaster;
    public Slider volumeFX;
    public Slider volumeMusic;
    public Slider mouseSensibility;
    public Toggle invertMouseClick;

    private GameManager _manager;
    private AudioSource _source;

    private void Awake()
    {
        _manager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        _source = GetComponent<AudioSource>();
    }

    public void ShowMenu()
    {
        pauseMenu.SetActive(true);
        cross.SetActive(false);
    }

    public void HideMenu()
    {
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(false);
        cross.SetActive(true);

        //Update
        var input = GameObject.FindWithTag("Player").GetComponent<InputHandler>();
        input.SetMouseSensibility(mouseSensibility.value);
        input.SetInvertMouse(invertMouseClick.isOn);

        //Save
        var prefs = Saver.Instance.GetPrefs();
        prefs.volumeMaster = volumeMaster.value;
        prefs.volumeFX = volumeFX.value;
        prefs.volumeMusic = volumeMusic.value;
        prefs.mouseSensitivity = mouseSensibility.value;
        prefs.invertMouseClick = invertMouseClick.isOn;
        Saver.Instance.SavePrefs(prefs);

        _source.Play();
    }

    public void ToOptions()
    {
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(true);

        var prefs = Saver.Instance.GetPrefs();
        volumeMaster.value = prefs.volumeMaster;
        volumeFX.value = prefs.volumeFX;
        volumeMusic.value = prefs.volumeMusic;
        mouseSensibility.value = prefs.mouseSensitivity;
        invertMouseClick.isOn = prefs.invertMouseClick;
    }

    public void ToMenu()
    {
        pauseMenu.SetActive(true);
        optionsMenu.SetActive(false);
    }

    public void Continue()
    {
        _manager.Pause();
    }

    public void BackToMainMenu()
    {
        _manager.Pause();
        _manager.LoadScene("Menu", true);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
