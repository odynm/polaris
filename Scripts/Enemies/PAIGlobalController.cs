// Polaris - Project VII 2018
// 08/03/18 -> 02/07/18
// Compiled in Unity 2017.2.0f3
// Writed by Mathias Ody (M-Ody)
// mathiasluizody@hotmail.com
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//(NOT USED)
namespace PAI
{
    public class PAIGlobalController : MonoBehaviour
    {

        //Todos inimigos, incluindo os do pool
        private PAIStateMachine[] allEnemies;
        //Todos inimigos spawnados
        private PAIStateMachine[] spawnedEnemies;
        //Todos os inimigos que estão perseguindo o player
        private PAIStateMachine[] aggroEnemies;

        void Awake()
        {
            var enemiesObj = GameObject.FindGameObjectsWithTag("Enemy");
            allEnemies = new PAIStateMachine[enemiesObj.Length];

            for (int i = 0; i < enemiesObj.Length; i++)
            {
                allEnemies[i] = enemiesObj[i].GetComponent<PAIStateMachine>();
            }

            enemiesObj = null;
        }

        void Update()
        {

        }
    }
}