// Polaris - Project VII 2018
// 08/03/18 -> 02/07/18
// Compiled in Unity 2017.2.0f3
// Writed by Mathias Ody (M-Ody)
// mathiasluizody@hotmail.com
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PolarisCore;

public class SceneLoader : MonoBehaviour
{
    public string sceneName;
    public Animator fadeAnim;

    private bool isLoading = false;

    void OnTriggerEnter (Collider other)
    {
        if (other.tag == "Player" && !isLoading)
        {
            isLoading = true;
            fadeAnim.gameObject.SetActive(true);
            fadeAnim.SetTrigger("FadeOut");

            Invoke("LoadScene", 0.5f);
        }
    }

    private void LoadScene()
    {
        GameStats stats = new GameStats {
            //ATENÇÃO: funciona até a cena 9
            level = int.Parse(sceneName[sceneName.Length-1].ToString()),

            hasShotgun = Inventory.Instance.availableGuns[1],
            hasRocketLauncher = Inventory.Instance.availableGuns[2],
            health = Inventory.Instance._health,
            pistolAmmo = Inventory.Instance._pistolAmmo,
            shotgunAmmo = Inventory.Instance._shotgunAmmo,
            rocketLauncherAmmo = Inventory.Instance._rocketLauncherAmmo
        };

        Saver.Instance.SaveStats(stats);
        GameObject.FindWithTag("GameManager").GetComponent<GameManager>().LoadScene(sceneName, false);
    }
}
