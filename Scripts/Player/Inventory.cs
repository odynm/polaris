// Polaris - Project VII 2018
// 08/03/18 -> 02/07/18
// Compiled in Unity 2017.2.0f3
// Writed by Mathias Ody (M-Ody)
// mathiasluizody@hotmail.com
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PolarisCore
{
    public class Inventory : ScriptableObject
    {
        [Header("Guns")]
        public int pistolInitalAmmo = 15;
        public int shotgunInitalAmmo = 5;
        public int rocketLauncherInitalAmmo = 1;
        [Header("Ammo")]
        public int pistolItemAmmo = 5;
        public int shotgunItemAmmo = 3;
        public int rocketLauncherItemAmmo = 1;
        public int pistolAmmoKit = 30;
        public int shotgunAmmoKit = 15;
        public int rocketAmmoKit = 1;
        [Header("Medkits")]
        public int medicKitSmall = 20;
        public int medicKitMedium = 50;
        public int medicKitLarge = 100;

        public int maxAmmoPistol = 200;
        public int maxAmmoShotgun = 150;
        public int maxAmmoRocketLauncer = 50;

        private int _takeShotgunKey;
        private int _takeRocketLauncherKey;
        private int _takePistolAmmoKey;
        private int _takeShotgunAmmoKey;
        private int _takeRocketLauncherAmmoKey;
        private int _takeAmmoKitKey;
        private int _takeMedkitKey;
        private int _takeCardKey;
    
        private static Inventory _instance;

        public static Inventory Instance
        {
            get
            {
                if (!_instance)
                    _instance = CreateInstance<Inventory>();
                return _instance;
            }
        }

        public bool [] availableGuns = new [] {true,false,false};
        public int numberGuns = 1;

        private GUIManager _gui;
        private GunManager _gun;

        public int _health = 100;

        public int _pistolAmmo;
        public int _shotgunAmmo;
        public int _rocketLauncherAmmo;

        private List<ECards> _cards;

        public void Initialize()
        {
            _gun = GameObject.Find("Gun").GetComponent<GunManager>();
            _gui = GameObject.Find("Canvas").GetComponent<GUIManager>();
            _cards = new List<ECards>();

            _pistolAmmo = pistolInitalAmmo;
            _health = 100;

            _takePistolAmmoKey = Pool.Instance.GetSharedPoolKey("TakePistolAmmo");
            _takeShotgunAmmoKey = Pool.Instance.GetSharedPoolKey("TakeShotgunAmmo");
            _takeRocketLauncherAmmoKey = Pool.Instance.GetSharedPoolKey("TakeRocketLauncherAmmo");
            _takeAmmoKitKey = Pool.Instance.GetSharedPoolKey("TakeAmmoKit");
            _takeMedkitKey = Pool.Instance.GetSharedPoolKey("TakeMedkit");
            _takeCardKey = Pool.Instance.GetSharedPoolKey("TakeCard");

            UpdateHUD();
        }

        public void MaxAll()
        {
            _pistolAmmo = int.MaxValue/2;
            _shotgunAmmo = int.MaxValue/2;
            _rocketLauncherAmmo = int.MaxValue/2;
            _cards.Add(ECards.BLUE_CARD);
            _cards.Add(ECards.ORANGE_CARD);
            _cards.Add(ECards.PURPLE_CARD);
            numberGuns = 3;
            availableGuns = new bool[] { true, true, true };
        }

        private void AddCard(ECards card)
        {
            SoundManager.Play2D(_takeCardKey);
            _cards.Add(card); 
            _gui.UpdateCard(card);
        }

        private bool AddPistolAmmo(int ammo)
        {
            if ((_pistolAmmo + ammo) < maxAmmoPistol)
            {
                SoundManager.Play2D(_takePistolAmmoKey);
                _gui.IncreaseAmmo(EGunType.PISTOL);
                _pistolAmmo += ammo;
                if (_pistolAmmo > maxAmmoPistol)
                {
                    _pistolAmmo = maxAmmoPistol;
                }
                return true;
            }
            return false;
        }

        private bool AddShotgunAmmo(int ammo)
        {
            if ((_shotgunAmmo + ammo) < maxAmmoShotgun)
            {
                SoundManager.Play2D(_takeShotgunAmmoKey);
                _gui.IncreaseAmmo(EGunType.SHOTGUN);
                _shotgunAmmo += ammo;
                if (_shotgunAmmo > maxAmmoShotgun)
                {
                    _shotgunAmmo = maxAmmoShotgun;
                }
                return true;
            }
            return false;
        }

        private bool AddRocketLauncherAmmo(int ammo)
        {
            if ((_rocketLauncherAmmo + ammo) < maxAmmoShotgun)
            {
                SoundManager.Play2D(_takeRocketLauncherAmmoKey);
                _gui.IncreaseAmmo(EGunType.ROCKET_LAUNCHER);
                _rocketLauncherAmmo += ammo;
                if (_rocketLauncherAmmo > maxAmmoShotgun)
                {
                    _rocketLauncherAmmo = maxAmmoShotgun;
                }
                return true;
            }
            return false;
        }

        private bool AddKitAmmo(int pistolAmmo, int shotgunAmmo, int rocketLauncherAmmo)
        {
            int changes = 0;
            if (AddPistolAmmo(pistolAmmo))
                changes++;
            if (AddShotgunAmmo(shotgunAmmo))
                changes++;
            if (AddRocketLauncherAmmo(rocketLauncherAmmo))
                changes++;

            if (changes > 0)
                return true;
            else
                return false;
        }
            
        public bool AddItem(EItemType itemType)
        {
            bool shouldDeleteItem = true;
            switch (itemType)
            {
            case EItemType.NONE: break ;
            case EItemType.BLUE_CARD:
                AddCard(ECards.BLUE_CARD); break;
            case EItemType.ORANGE_CARD: 
                AddCard(ECards.ORANGE_CARD); break;
            case EItemType.PURPLE_CARD: 
                AddCard(ECards.PURPLE_CARD); break;

            case EItemType.PISTOL_AMMO_SMALL:
                shouldDeleteItem = AddPistolAmmo(pistolItemAmmo); break;
            case EItemType.SHOTGUN_AMMO_SMALL: 
                shouldDeleteItem = AddShotgunAmmo(shotgunItemAmmo); break;
            case EItemType.ROCKET_LAUNCHER_SMALL: 
                shouldDeleteItem = AddRocketLauncherAmmo(rocketLauncherItemAmmo); break;
            case EItemType.AMMO_KIT: 
                shouldDeleteItem = AddKitAmmo(pistolAmmoKit, shotgunAmmoKit, rocketAmmoKit); break;
                
            case EItemType.MEDIC_KIT_SMALL:
                shouldDeleteItem = IncreaseHealth(medicKitSmall); break;
            case EItemType.MEDIC_KIT_MEDIUM: 
                shouldDeleteItem = IncreaseHealth(medicKitMedium); break;
            case EItemType.MEDIC_KIT_LARGE: 
                shouldDeleteItem = IncreaseHealth(medicKitLarge); break;

            case EItemType.SHOTGUN: 
                numberGuns++; 
                availableGuns [1] = true; 
                _shotgunAmmo += shotgunInitalAmmo;
                _gun.ChooseGun(1);
            break;
            case EItemType.ROCKET_LAUNCHER:
                numberGuns++; 
                availableGuns [2] = true; 
                _rocketLauncherAmmo += rocketLauncherInitalAmmo;
                _gun.ChooseGun(2);
            break;
            }
            UpdateHUD();
            return shouldDeleteItem;
        }

        public void RemoveItem(EItemType itemType)
        {
            switch (itemType)
            {
            case EItemType.SHOTGUN: 
                if (availableGuns [1])
                {
                    numberGuns--; 
                    availableGuns [1] = false; 
                }
                break;
            case EItemType.ROCKET_LAUNCHER:
                if (availableGuns [2])
                {
                    numberGuns--; 
                    availableGuns [2] = false; 
                }
                break;
            }
        }

        public bool HasAmmo(EGunType _currentGun)
        {
            switch (_currentGun)
            {
                case EGunType.PISTOL: return _pistolAmmo > 0;
                case EGunType.SHOTGUN: return _shotgunAmmo > 0;
                case EGunType.ROCKET_LAUNCHER: return _rocketLauncherAmmo > 0;
            }
            return false;
        }

        public void DecreaseAmmo(EGunType itemType)
        {
            switch (itemType)
            {
                case EGunType.PISTOL: _pistolAmmo--; break;
                case EGunType.SHOTGUN: _shotgunAmmo--; break;
                case EGunType.ROCKET_LAUNCHER: _rocketLauncherAmmo--; break;
            }
            UpdateHUD();
        }


        public bool HasCard(ECards card)
        {
            return _cards.Contains(card);
        }

        public int Hit(int damage)
        {
            _health -= damage;

            if (_health < 0)
                _health = 0;

            UpdateHUD();
            return _health;
        }

        private bool IncreaseHealth (int ammount)
        {
            if (_health == 100)
                return false;
            _health = Mathf.Clamp(_health + ammount, 0, 100);
            SoundManager.Play2D(_takeMedkitKey);
            return true;
        }

        public void RequestUpdateHUD()
        {
            UpdateHUD();
        }

        private void UpdateHUD()
        {
            //if (_gui == null)
            //    _gui = GameObject.Find("Canvas").GetComponent<GUIManager>();

            _gui.UpdateHealth(_health);

            if (_gun.currentGun == EGunType.PISTOL)
                _gui.ammo.text = _pistolAmmo.ToString();
            else if (_gun.currentGun == EGunType.SHOTGUN)
                _gui.ammo.text = _shotgunAmmo.ToString();
            else if (_gun.currentGun == EGunType.ROCKET_LAUNCHER)
                _gui.ammo.text = _rocketLauncherAmmo.ToString();
        }
    }
}
