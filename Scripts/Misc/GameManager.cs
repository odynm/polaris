// Polaris - Project VII 2018
// 08/03/18 -> 02/07/18
// Compiled in Unity 2017.2.0f3
// Writed by Mathias Ody (M-Ody)
// mathiasluizody@hotmail.com
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//
// Responsável por controles macro do game e carregamento de cenas
// O carregamento de cenas pode ser desacoplado daqui futuramente
//
using UnityEngine.Audio;


public class GameManager : MonoBehaviour
{
    public AudioMixer mixer;

    private string nextScene = "";
    private bool allowActivation = true;
    private PauseMenu _pauseMenu;

    private void Awake()
    {
        if (GameObject.FindGameObjectsWithTag("GameManager").Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            UpdateSettings();
        }

        if (SceneManager.GetActiveScene().name.Contains("Level"))
        {
            _pauseMenu = GameObject.Find("Canvas").GetComponentInChildren<PauseMenu>();
        }
    }

    public void Pause ()
    {
        if (_pauseMenu == null)
            _pauseMenu = GameObject.Find("Canvas").GetComponentInChildren<PauseMenu>();

        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
            _pauseMenu.HideMenu();
            var prefs = Saver.Instance.GetPrefs();
            mixer.SetFloat("master_Volume", (prefs.volumeMaster * 80f) - 80f);
            mixer.SetFloat("sfx_Volume", (prefs.volumeFX * 80f) - 80f);
            mixer.SetFloat("background_Volume", (prefs.volumeMusic * 80f) -80f);
        }
        else
        {
            Time.timeScale = 0;
            _pauseMenu.ShowMenu();
        }
    }

    public void LoadScene(string sceneName, bool allowActivaton = true)
    {
        this.allowActivation = allowActivaton;
        nextScene = sceneName;
        SceneManager.LoadScene("Loading");
        SceneManager.sceneLoaded += SetUpLoader;
    }

    public void LoadEnd(string sceneName)
    {
        this.allowActivation = false;
        nextScene = sceneName;
        SceneManager.LoadScene("Ending");
        SceneManager.sceneLoaded += SetUpLoader;
    }

    public void UpdateSettings()
    {
        var prefs = Saver.Instance.GetPrefs();

        QualitySettings.SetQualityLevel(prefs.graphicsQuality);
        mixer.SetFloat("master_Volume", (prefs.volumeMaster * 80f) - 80f);
        mixer.SetFloat("sfx_Volume", (prefs.volumeFX * 80f) - 80f);
        mixer.SetFloat("background_Volume", (prefs.volumeMusic * 80f) -80f);
    }

    private void SetUpLoader(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= SetUpLoader;

        if (scene.name == "Loading" || scene.name == "Ending")
        {
            Invoke("LoadScene", 0.5f);
        }
    }

    private void LoadScene()
    {
        var loader = GameObject.Find("_Loader").GetComponent<LoaderManager>();
        loader.Load(nextScene, allowActivation);
    }
}
