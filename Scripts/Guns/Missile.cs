// Polaris - Project VII 2018
// 08/03/18 -> 02/07/18
// Compiled in Unity 2017.2.0f3
// Writed by Mathias Ody (M-Ody)
// mathiasluizody@hotmail.com
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PAI;

namespace PolarisCore
{
    public class Missile : MonoBehaviour 
    {
        private Rigidbody _rb;
        private int _explosionsKey;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            _explosionsKey = Pool.Instance.GetSharedPoolKey("Explosion");
        }

        private void OnCollisionEnter(Collision other)
        {
            _rb.velocity = Vector3.zero;
            gameObject.SetActive(false);
            var rot = Quaternion.FromToRotation(Vector3.up, other.contacts [0].normal);

            var explosion = Pool.Instance.GetNotActivated(_explosionsKey);
            explosion.transform.position = transform.position;
            explosion.transform.rotation = rot;
            explosion.SetActive (true);

            /*if (other.gameObject.tag == "Enemy")
            {
                other.gameObject.GetComponent<PAIEnemy>().BodyHit(transform.position, Vector3.zero, EGunType.ROCKET_LAUNCHER, 1000);
            }*/
        }
    }
}