// Polaris - Project VII 2018
// 08/03/18 -> 02/07/18
// Compiled in Unity 2017.2.0f3
// Writed by Mathias Ody (M-Ody)
// mathiasluizody@hotmail.com
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PAI;

public class SpawnHorde : MonoBehaviour 
{
    void OnEnable () 
    {
        Invoke("ActivateChildEnemies", 0.1f);
    }

    void ActivateChildEnemies()
    {
        var enemies = GetComponentsInChildren<PAIEnemy>();

        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].ActivateEnemyAttack();
        }
    }
}
