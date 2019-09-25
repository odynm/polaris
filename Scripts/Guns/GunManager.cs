// Polaris - Project VII 2018
// 08/03/18 -> 02/07/18
// Compiled in Unity 2017.2.0f3
// Writed by Mathias Ody (M-Ody)
// mathiasluizody@hotmail.com
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PolarisCore
{
    public class GunManager : MonoBehaviour
    {
        public GameObject[] gunsObj;
        public EGunType currentGun = 0;

        private int _numberGuns;
        private List<Gun> _guns;
        private Inventory _inventory;
        private Animator _anim;
        private int _gunChanged; //para uso do scroll
        private int _choosedGun = -1; //para uso de teclas

        private bool canShot = true;

        private const int TOTAL_GUNS = 3;

        private void Awake()
        {
            _guns = new List<Gun>();
            _anim = GetComponentInParent<Animator>();
            _inventory = Inventory.Instance;

            bool firstFlag = true;
            foreach (GameObject obj in gunsObj)
            {
                if (!firstFlag) obj.SetActive(false);
                else firstFlag = false;
                _guns.Add(obj.GetComponent<Gun>());
            }
            _numberGuns = 1;
        }

        public void Shoot()
        {
            if (_gunChanged == 0 && canShot)
            {
                bool successfulShot = _guns [(int)currentGun].Shoot();
                if (!successfulShot)
                {
                    if ((int)currentGun == 0)
                        NextGun();
                    else
                        PreviousGun();
                }
            }           
        }

        public void NextGun()
        {
            _numberGuns = _inventory.numberGuns; //atualizar numbero de armas com inventário (pode ser otimizado)_
            //se tiver só uma arma, não faça nada
            if (_numberGuns == 1)
                return;
            
            if (_choosedGun != -1 || _gunChanged != 0) return;
            _anim.SetTrigger ("GunDown");
            _gunChanged = 1;
            canShot = false;
            Invoke("CanShotAgain", 0.7f);
        }

        public void PreviousGun()
        {
            _numberGuns = _inventory.numberGuns; //atualizar numbero de armas com inventário (pode ser otimizado)_
            //se tiver só uma arma, não faça nada
            if (_numberGuns == 1)
                return;
            
            if (_choosedGun != -1 || _gunChanged != 0) return;
            _anim.SetTrigger ("GunDown");
            _gunChanged = -1;
            canShot = false;
            Invoke("CanShotAgain", 0.7f);
        }

        public void ChooseGun(int num)
        {
            _numberGuns = _inventory.numberGuns; //atualizar numbero de armas com inventário (pode ser otimizado)_
            //se tiver só uma arma, não faça nada
            if (_numberGuns == 1)
                return;
            
            if (_choosedGun != -1 || _gunChanged != 0|| num == (int)currentGun || !_inventory.availableGuns[num]) return;

            _choosedGun = num;
            _anim.SetTrigger("GunDown");
            canShot = false;
            Invoke("CanShotAgain", 0.7f);
        }

        public void OnGunDown()
        {
            ChangeGun();
            _anim.SetTrigger ("GunUp");
        }

        public void OnGunUp()
        {

        }

        private void CanShotAgain()
        {
            _gunChanged = 0;
            canShot = true;
        }

        private void ChangeGun()
        {
            _guns[(int)currentGun].gameObject.SetActive(false);

            if (_choosedGun < 0) //se for arma escolhida pelo scroll
            {
                if (_gunChanged > 0)
                {
                    while (true)
                    {
                        currentGun = (int)currentGun == (TOTAL_GUNS - 1) ? currentGun = 0 : ++currentGun;
                        if (_inventory.availableGuns [(int)currentGun])
                            break;
                    }
                }
                else
                {
                    while (true)
                    {
                        currentGun = currentGun == 0 ? currentGun = (EGunType)(TOTAL_GUNS - 1) : --currentGun;
                        if (_inventory.availableGuns [(int)currentGun])
                            break;
                    }
                }
            }
            else //se for arma escolhida pelo teclado
            {
                //se não tiver nas armas disponívies, não faça mais nada
                if (!_inventory.availableGuns[_choosedGun]) return;
                    
                currentGun = (EGunType)_choosedGun;
                _choosedGun = -1;
            }

            _guns[(int)currentGun].gameObject.SetActive(true);

            Inventory.Instance.RequestUpdateHUD();
        }
    }
}
