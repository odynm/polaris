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
    public class LayerMaskData : ScriptableObject 
    {
        public static int recieveShots;     
        public static int recieveBulletDecal;
        public static int recieveChildDecal;
        public static int recieveBloodDecal;
        public static int enemyBullets;
        public static int generalTrigger;
        public static int player;
        public static int enemy;
        public static int enemySensor;
        public static int dynamicFly;
        public static int dynamic;
        public static int barriers;
        public static int recieveExplosionCheck;

        void Awake()
        {
            recieveShots = LayerMask.GetMask("Default","EnemiesBullets", "StaticDecal", "StaticOnlyBulletDecal", "Dynamic", "NoneDecal", "DynamicOnlyBulletDecal", "DynamicAllDecal", "DynamicFly");     
            recieveBulletDecal = LayerMask.GetMask("Default", "StaticDecal", "StaticOnlyBulletDecal", "DynamicAllDecal", "DynamicOnlyBulletDecal");
            recieveChildDecal = LayerMask.GetMask("DynamicOnlyBulletDecal", "DynamicAllDecal");
            recieveBloodDecal = LayerMask.GetMask("Default", "StaticDecal", "DynamicAllDecal");
            enemyBullets = LayerMask.GetMask("EnemiesBullets");
            generalTrigger = LayerMask.GetMask("GeneralTrigger");
            player = LayerMask.GetMask("Player");
            enemy = LayerMask.GetMask("Enemies");
            enemySensor = LayerMask.GetMask("Default", /*"Player",*/ "StaticDecal", "StaticOnlyBulletDecal", "Dynamic", "NoneDecal", "DynamicOnlyBulletDecal", "DynamicAllDecal");
            dynamicFly = LayerMask.GetMask("DynamicFly");
            dynamic = LayerMask.GetMask("Dynamic");
            barriers = LayerMask.GetMask("Default", "NoneDecal", "StaticDecal", "StaticOnlyBulletDecal", "DynamicOnlyBulletDecal");
            recieveExplosionCheck = LayerMask.GetMask("Enemies", "DynamicFly", "Dynamic", "Player");
        }
    }
}
