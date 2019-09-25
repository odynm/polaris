// Polaris - Project VII 2018
// 08/03/18 -> 02/07/18
// Compiled in Unity 2017.2.0f3
// Writed by Mathias Ody (M-Ody)
// mathiasluizody@hotmail.com
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

namespace PolarisCore
{
    public class RocketLauncher : Gun 
    {
        public RocketLauncher()
        {
            GUN_TYPE = EGunType.ROCKET_LAUNCHER;
        }

        public Transform gunPoint;
        public GameObject missile;
        public float force = 3000f;

        public GameObject particle;
        private int _particleKey;
        private int _rocketShotKey;

        private Ray _ray;
        private RaycastHit _hit;
        private Animator _anim;

        private int _missileKey;

        private void Awake()
        {
            base.Initialize();

            _missileKey = Pool.Instance.AddSimplePoolObject(missile, 7);

            _anim = GetComponent<Animator>();

            _particleKey = Pool.Instance.AddSimplePoolObject(particle, 10);
        }

        private void Start()
        {
            _rocketShotKey = Pool.Instance.GetSharedPoolKey("RocketShot");
        }

        private void OnEnable()
        {
            base.PlayAttachedAudio();
            base.Enable();
        }

        private void OnDisable()
        {
            base.Disable();
        }

        public override bool Shoot()
        {
            if (!_inventory.HasAmmo(GUN_TYPE)) return false;

            if (Time.timeSinceLevelLoad > base.cooldownElapsed)
            {
                SoundManager.Play(_rocketShotKey ,transform);
                _inventory.DecreaseAmmo(GUN_TYPE);

                base.cooldownElapsed = Time.timeSinceLevelLoad + base.cooldownTime;

                _ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0));
                if (Physics.Raycast(_ray, out _hit, Camera.main.farClipPlane, LayerMaskData.recieveShots))
                {
                    var particles = Pool.Instance.GetNotActivated(_particleKey);
                    particles.transform.position = gunPoint.position;
                    particles.transform.rotation = gunPoint.rotation;
                    particles.SetActive(true);

                    var missileObj = Pool.Instance.Get(_missileKey);
                    missileObj.transform.position = gunPoint.position;
                    missileObj.transform.rotation = gunPoint.rotation;

                    missileObj.GetComponentInChildren<Rigidbody>().AddForce
                    (Vector3.Normalize(_hit.point - gunPoint.position) * force);
                    
                    CameraShaker.Instance.ShakeOnce(0.5f, 0.5f, 0.1f, 0.1f);
                    _anim.SetTrigger("Shoot");
                }
            }
            return true;
        }
    }
}
