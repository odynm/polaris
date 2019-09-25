// Polaris - Project VII 2018
// 08/03/18 -> 02/07/18
// Compiled in Unity 2017.2.0f3
// Writed by Mathias Ody (M-Ody)
// mathiasluizody@hotmail.com
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PolarisCore;

public class EndSceneLoader : MonoBehaviour
{
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
        Cursor.lockState = CursorLockMode.Confined;
        Saver.Instance.DeleteProgress();
        GameObject.FindWithTag("GameManager").GetComponent<GameManager>().LoadEnd("Menu");
    }
}
