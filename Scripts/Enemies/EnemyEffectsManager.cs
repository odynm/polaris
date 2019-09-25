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
public class EnemyEffectsManager : MonoBehaviour
    {
        public float enemyAnchorHeight;

        private int _bloodSplashKey;
        private int _bloodSplashPoolKey;
        private int _bigBloodHitKey;
        
        private RaycastHit _hit;

        private void Start()
        {
            //Pode haver um array de keys com um tipo de splash para randomizar
            _bloodSplashKey = Pool.Instance.GetSharedPoolKey("BloodSplash");
            _bloodSplashPoolKey = Pool.Instance.GetSharedPoolKey("BloodPoolSplash");
            _bigBloodHitKey = Pool.Instance.GetSharedPoolKey("BigBloodHit");
        }

        public void PlaceBloodDecalOnFloor()
        {
            PlaceDecalOnFloor(_bloodSplashKey);
        }

        public void PlaceBloodPoolDecalOnFloor()
        {
            if (Physics.Raycast(transform.position, -Vector3.up, out _hit, 1.5f, LayerMaskData.recieveBloodDecal))
            {
                PlaceDecalOnFloor(_bloodSplashPoolKey);
            }
        }

        public void PlaceDecalOnWalls()
        {
            if (Physics.Raycast(transform.position, transform.right, out _hit, 1.5f, LayerMaskData.recieveBloodDecal))
            {
                PlaceDecal(_bloodSplashKey,_hit.point, _hit.normal);
            }
            if (Physics.Raycast(transform.position, -transform.right, out _hit, 1.5f, LayerMaskData.recieveBloodDecal))
            {
                PlaceDecal(_bloodSplashKey,_hit.point, _hit.normal);
            }

            if (Physics.Raycast(transform.position, -transform.forward, out _hit, 1.5f, LayerMaskData.recieveBloodDecal))
            {
                PlaceDecal(_bloodSplashKey,_hit.point, _hit.normal);
            }
            else if (Physics.Raycast(transform.position, transform.forward, out _hit, 1.5f, LayerMaskData.recieveBloodDecal))
            {
                PlaceDecal(_bloodSplashKey,_hit.point, _hit.normal);
            }
        }

        public void SpawnParticlesFromBack(Vector3 location, Vector3 normal)
        {
            var obj = Pool.Instance.GetNotActivated(_bigBloodHitKey);
            obj.transform.position = new Vector3 (
                    transform.position.x + (transform.position.x - location.x), 
                    transform.position.y, 
                    transform.position.z + (transform.position.z - location.z));
            
            obj.transform.rotation = Quaternion.LookRotation(-normal);
            obj.transform.parent = transform;
            obj.SetActive(true);
        }

        private void PlaceDecalOnFloor(int key)
        {
            if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y - enemyAnchorHeight, transform.position.z), -transform.up, out _hit, 2f, LayerMaskData.recieveBloodDecal))
            {
                PlaceDecal(key, _hit.point + (transform.right * 0.5f), _hit.normal);
                PlaceDecal(key, _hit.point + (transform.right * -0.5f), _hit.normal);
                PlaceDecal(key, _hit.point + (transform.forward * 0.5f), _hit.normal);
            }
        }

        private void PlaceDecal(int key, Vector3 position, Vector3 normal)
        {
            var obj = Pool.Instance.Get(key);
            obj.transform.position = position;
            obj.transform.rotation = Quaternion.LookRotation(-normal);
        }
    }
}
