// Polaris - Project VII 2018
// 08/03/18 -> 02/07/18
// Compiled in Unity 2017.2.0f3
// Writed by Mathias Ody (M-Ody)
// mathiasluizody@hotmail.com
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PolarisCore
{
    public class GUIManager : MonoBehaviour 
    {

        //Crosshair Control
        public Animation[] stripeList;

        //DamageAnimation
        public Animation damageAnim;
        public Animation lifeAnim;

        public Text ammo;
        public Text health;

        private int lastHealth = 100;

        public GameObject blue;
        public GameObject orange;
        public GameObject purple;

        private GunManager _gun;
        private int _maxHealthSize;

        private void Start()
        {
            _gun = GameObject.Find("Player").GetComponentInChildren<GunManager>();
        }

        public void UpdateHealth(int health)
        {
            if (lastHealth < health)
            {
                this.health.gameObject.GetComponent<Animator>().SetTrigger("Increase");
                CrossPlay();
                lifeAnim.Play ();
            }
            else if (lastHealth > health)
            {
                this.health.gameObject.GetComponent<Animator>().SetTrigger("Decrease");
                damageAnim.Play ();
            } 
                

            this.health.gameObject.GetComponent<Animator>().SetInteger("Value", health);
            this.health.text = health.ToString();

            lastHealth = health;


        }

        public void IncreaseAmmo(EGunType ammoGun)
        {
            if (_gun.currentGun == ammoGun)
            this.ammo.gameObject.GetComponent<Animator>().SetTrigger("Increase");
            CrossPlay();
        }

        public void UpdateCard(ECards card)
        {
            switch (card)
            {
            case ECards.BLUE_CARD:
                blue.SetActive(true);
                blue.GetComponent<Animator>().SetTrigger("Increase");
                CrossPlay();
                break;
            case ECards.ORANGE_CARD:
                orange.SetActive(true);
                orange.GetComponent<Animator>().SetTrigger("Increase");
                CrossPlay();
                break;
            case ECards.PURPLE_CARD:
                purple.SetActive(true);
                purple.GetComponent<Animator>().SetTrigger("Increase");
                CrossPlay();
                break;
            default:
                break;
            }
        }
            
        private void CrossPlay()
        {
            foreach (Animation anim in stripeList)
            {
                anim.Play();
            }
        }
    }
}