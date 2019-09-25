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
    public class Manager : MonoBehaviour
    {
        [Header("Shared Pools")]
        public GameObject explosion;
        public GameObject bloodSmoke;
        public GameObject bloodSpray;
        public GameObject smallBloodHit;
        public GameObject bigBloodHit;
        public GameObject smallFlesh;
        public GameObject bloodSplash;
        public GameObject bloodPoolSplash;
        public GameObject smallFleshHead;
        public GameObject spit;
        public GameObject burn;
        public GameObject goopDecal;

        [Header("Shared Audios")]
        [Header("Misc")]
        public GameObject doorOpen;
        public GameObject doorClose;
        [Header("Player")]
        public GameObject playerPain;
        public GameObject playerJump;
        public GameObject playerDeath;
        public GameObject playerSteps;
        /*public GameObject changeToPistol;
        public GameObject changeToShotgun;
        public GameObject changeToRocketLauncher;*/
        [Header("Aliens")]
        public GameObject alien1Attack;
        public GameObject alien2Attack;
        public GameObject alien2PreSpit;
        public GameObject alien2Spit;
        public GameObject alien1Patrol;
        public GameObject alien2Patrol;
        public GameObject alien1FoundPlayer;
        public GameObject alien2FoundPlayer;
        public GameObject alien1TakeHit;
        public GameObject alien2TakeHit;
        public GameObject alienHitPlayer;
        public GameObject alienGutsExplosion;
        public GameObject goopHitOther;
        public GameObject goopHitPlayer;
        [Header("Itens")]
        public GameObject takePistolAmmo;
        public GameObject takeShotgunAmmo;
        public GameObject takeRocketLauncherAmmo;
        public GameObject takeAmmoKit;
        public GameObject takeMedkit;
        public GameObject takeCard;
        [Header("Guns")]
        public GameObject shotgunHitEnemy;
        public GameObject shotgunHitOther;
        public GameObject pistolHitEnemy;
        public GameObject pistolHitOther;
        public GameObject shotgunShot;
        public GameObject pistolShot;
        public GameObject rocketShot;
        public GameObject explosionSound;

        void Awake()
        {
            Shader.WarmupAllShaders();

            ScriptableObject.CreateInstance<LayerMaskData>();

            Pool.Instance.Setup();
            Pool.Instance.CleanSharedPool();

            Pool.Instance.AddSharedPool("Explosion", explosion, 10);
            Pool.Instance.AddSharedPool("BloodSmoke", bloodSmoke, 20);
            Pool.Instance.AddSharedPool("BloodSpray", bloodSpray, 20);
            Pool.Instance.AddSharedPool("SmallBloodHit", smallBloodHit, 20);
            Pool.Instance.AddSharedPool("BigBloodHit", bigBloodHit, 20);
            Pool.Instance.AddSharedPool("SmallFlesh", smallFlesh, 40);
            Pool.Instance.AddSharedPoolDecal("BloodSplash", bloodSplash, 70, true);
            Pool.Instance.AddSharedPoolDecal("BloodPoolSplash", bloodPoolSplash, 60, true);
            Pool.Instance.AddSharedPool("SmallFleshHead", smallFleshHead, 10);
            Pool.Instance.AddSharedPool("Spit", spit, 20);
            Pool.Instance.AddSharedPoolDecal("Burn", burn, 10);
            Pool.Instance.AddSharedPoolDecal("EnemyGoopDecal", goopDecal, 30);

            Pool.Instance.AddSharedPool("PlayerPain", playerPain, 5, true);
            Pool.Instance.AddSharedPool("DoorOpen", doorOpen, 5, true);
            Pool.Instance.AddSharedPool("DoorClose", doorClose, 5, true);
            Pool.Instance.AddSharedPool("Alien1Attack", alien1Attack, 12, true);
            Pool.Instance.AddSharedPool("Alien2Attack", alien2Attack, 12, true);
            Pool.Instance.AddSharedPool("Alien2PreSpit", alien2PreSpit, 12, true);
            Pool.Instance.AddSharedPool("Alien2Spit", alien2Spit, 12, true);
            Pool.Instance.AddSharedPool("PlayerJump", playerJump, 3, true);
            Pool.Instance.AddSharedPool("PlayerDeath", playerDeath, 1, true);
            Pool.Instance.AddSharedPool("PlayerSteps", playerSteps, 10, true);
            /*Pool.Instance.AddSharedPool("ChangeToPistol", changeToPistol, 1, true);
            Pool.Instance.AddSharedPool("ChangeToShotgun", changeToShotgun, 1, true);
            Pool.Instance.AddSharedPool("ChangeToRocketLauncher", changeToRocketLauncher, 1, true);*/

            Pool.Instance.AddSharedPool("Alien1Patrol", alien1Patrol, 15, true);
            Pool.Instance.AddSharedPool("Alien2Patrol", alien2Patrol, 15, true);
            Pool.Instance.AddSharedPool("Alien1FoundPlayer", alien1FoundPlayer, 15, true);
            Pool.Instance.AddSharedPool("Alien2FoundPlayer", alien2FoundPlayer, 15, true);
            Pool.Instance.AddSharedPool("Alien1TakeHit", alien1TakeHit, 15, true);
            Pool.Instance.AddSharedPool("Alien2TakeHit", alien2TakeHit, 15, true);
            Pool.Instance.AddSharedPool("AlienHitAttack", alienHitPlayer, 15, true);
            Pool.Instance.AddSharedPool("AlienGutsExplosion", alienGutsExplosion, 10, true);
            Pool.Instance.AddSharedPool("EnemyGoopSound", goopHitOther, 10, true);
            Pool.Instance.AddSharedPool("EnemyGoopPlayerSound", goopHitPlayer, 10, true);

            Pool.Instance.AddSharedPool("TakePistolAmmo", takePistolAmmo, 10, true);
            Pool.Instance.AddSharedPool("TakeShotgunAmmo", takeShotgunAmmo, 10, true);
            Pool.Instance.AddSharedPool("TakeRocketLauncherAmmo", takeRocketLauncherAmmo, 5, true);
            Pool.Instance.AddSharedPool("TakeAmmoKit", takeAmmoKit, 5, true);
            Pool.Instance.AddSharedPool("TakeMedkit", takeMedkit, 5, true);
            Pool.Instance.AddSharedPool("TakeCard", takeCard, 1, true);

            Pool.Instance.AddSharedPool("ShotgunHitEnemy", shotgunHitEnemy, 10, true);
            Pool.Instance.AddSharedPool("ShotgunHitOther", shotgunHitOther, 10, true);
            Pool.Instance.AddSharedPool("PistolHitEnemy", pistolHitEnemy, 10, true);
            Pool.Instance.AddSharedPool("PistolHitOther", pistolHitOther, 10, true);
            Pool.Instance.AddSharedPool("ShotgunShot", shotgunShot, 5, true);
            Pool.Instance.AddSharedPool("PistolShot", pistolShot, 15, true);
            Pool.Instance.AddSharedPool("ExplosionSound", explosionSound, 7, true);
            Pool.Instance.AddSharedPool("RocketShot", rocketShot, 2, true);
        }
    }
}
