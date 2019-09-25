// Polaris - Project VII 2018
// 08/03/18 -> 02/07/18
// Compiled in Unity 2017.2.0f3
// Writed by Mathias Ody (M-Ody)
// mathiasluizody@hotmail.com
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PolarisCore
{
    public class ActiveByTrigger : MonoBehaviour
    {


        public GameObject[] targetObjects;

        void Start()
        {
            foreach (GameObject target in targetObjects)
            {
                target.SetActive(false);
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player") {
                
                foreach (GameObject target in targetObjects) {
                    target.SetActive (true);
                }
                gameObject.SetActive (false);

            }
        }
    }
}
