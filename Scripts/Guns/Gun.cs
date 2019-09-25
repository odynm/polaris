// Polaris - Project VII 2018
// 08/03/18 -> 02/07/18
// Compiled in Unity 2017.2.0f3
// Writed by Mathias Ody (M-Ody)
// mathiasluizody@hotmail.com
using PAI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//
// Mais variáveis comuns às armas poderiam ser transferidas para esse script.
//

namespace PolarisCore
{
    public class Gun : MonoBehaviour
    {
        public GameObject hudGunName;
        public float cooldownTime = 1f;

        protected float cooldownElapsed;
        protected EGunType GUN_TYPE;

        protected Inventory _inventory;

        protected void Enable()
        {
            hudGunName.SetActive(true);
        }

        protected void Disable()
        {
            if (hudGunName != null)
                hudGunName.SetActive(false);
        }

        protected void Initialize()
        {
            _inventory = Inventory.Instance;
        }

        public virtual bool Shoot() { return false; }

        protected void HitEnemy(RaycastHit hit, int damage)
        {
            HitEnemy(hit, GUN_TYPE, damage);
        }

        protected void HitEnemy(RaycastHit hit, EGunType gun, int damage)
        {
            if (hit.collider.gameObject.tag == "Enemy")
            {
                hit.collider.gameObject.GetComponentInParent<PAIEnemy>().BodyHit(hit.point, hit.normal, gun, damage, hit.collider.gameObject.transform);
            }
            else if (hit.collider.gameObject.tag == "EnemyHead")
            {
                hit.collider.gameObject.GetComponentInParent<PAIEnemy>().HeadHit(hit.point, hit.normal, gun, damage*3, hit.collider.gameObject.transform);
            }
        }

        protected void PlayAttachedAudio()
        {
            GetComponent<AudioSource>().Play();
        }
    }
}