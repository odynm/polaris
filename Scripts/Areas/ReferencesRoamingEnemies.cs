// Polaris - Project VII 2018
// 08/03/18 -> 02/07/18
// Compiled in Unity 2017.2.0f3
// Writed by Mathias Ody (M-Ody)
// mathiasluizody@hotmail.com
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PAI;

public class ReferencesRoamingEnemies : MonoBehaviour
{

    [SerializeField]
    private bool isPrioritary;

    List<PAIEnemy> roamingEnemies;
    bool firstFrameFlag;

    Collider myCollider;
    Vector2[] points = new Vector2[2];

    private void OnEnable()
    {
        roamingEnemies = new List<PAIEnemy>();
        myCollider = GetComponent<Collider>();

        points[0] = new Vector2(myCollider.bounds.min.x, myCollider.bounds.min.z);
        points[1] = new Vector2(myCollider.bounds.max.x, myCollider.bounds.max.z);

        Invoke("SetEnemiesToRoam",1f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            if(other.gameObject.GetComponent<PAIEnemy>() != null)
                roamingEnemies.Add(other.gameObject.GetComponent<PAIEnemy>());
        }
    }

    private void SetEnemiesToRoam()
    {
        foreach(PAIEnemy enemy in roamingEnemies)
        {
            enemy.SetRoaming(points, isPrioritary);
        }

        gameObject.SetActive(false);
    }
}
