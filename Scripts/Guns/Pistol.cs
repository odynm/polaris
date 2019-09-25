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
    public class Pistol : Gun
    {
        public Pistol()
        {
            GUN_TYPE = EGunType.PISTOL;
        }

        public Transform gunPoint;
        public GameObject wallHit;
        public GameObject line;
        [Range(3f,8f)]
        public float lazerBeamFadeSpeed = 4f;
        [Header("Damage")]
        public int damage = 20;

        public GameObject particle;
        private int _particleKey;

        public float forceOnObjects = 300f;

        private int _decalKey;
        private int _dynamicDecalKey;
        private int _lineKey;
        private int _changeToPistolKey;

        private float _decalSize;
        private Ray _ray;
        private RaycastHit _hit;
        private Animator _anim;

        private int _pistolHitEnemy;
        private int _pistolHitOther;
        private int _pistolShotKey;

        private void Awake()
        {
            base.Initialize();

            _lineKey = Pool.Instance.AddSimplePoolObject(line, 5);
            _decalKey = Pool.Instance.AddSimplePoolObjectDecal(wallHit, 10, true);
            _dynamicDecalKey = Pool.Instance.AddSimplePoolObjectDecal(wallHit, 10, true);
            _decalSize = wallHit.GetComponentInChildren<Renderer>().bounds.extents.x;

            _particleKey = Pool.Instance.AddSimplePoolObject(particle, 10);
        }

        private void Start()
        {
            _anim = GetComponent<Animator>();

            _pistolHitEnemy = Pool.Instance.GetSharedPoolKey("PistolHitEnemy");
            _pistolHitOther = Pool.Instance.GetSharedPoolKey("PistolHitOther");
            _pistolShotKey = Pool.Instance.GetSharedPoolKey("PistolShot");
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
                SoundManager.Play(_pistolShotKey, transform);
                _inventory.DecreaseAmmo(GUN_TYPE);

                base.cooldownElapsed = Time.timeSinceLevelLoad + base.cooldownTime;

                _ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0));
                if (Physics.Raycast(_ray, out _hit, Camera.main.farClipPlane, LayerMaskData.recieveShots))
                {
                    if (((1 << _hit.transform.gameObject.layer) & LayerMaskData.enemyBullets) != 0)
                    {
                        base.HitEnemy(_hit, damage);
                        SoundManager.Play(_pistolHitEnemy, _hit.transform);
                    }
                    else
                    {
                        SoundManager.Play(_pistolHitOther, _hit.transform);

                        if (((1 << _hit.transform.gameObject.layer) & LayerMaskData.dynamicFly) != 0)
                            _hit.transform.gameObject.GetComponent<Rigidbody>().AddForceAtPosition(Vector3.Normalize(_hit.point - transform.position) * forceOnObjects, _hit.point);
                    
                        if (((1 << _hit.transform.gameObject.layer) & LayerMaskData.recieveBulletDecal) != 0)
                            DecalManager.Instance.PlaceSimpleDecalWithParticles(_decalKey, _dynamicDecalKey, ref _hit, ref _decalSize);

                        if (((1 << _hit.transform.gameObject.layer) & LayerMaskData.dynamic) != 0 && _hit.transform.gameObject.tag == "ExplosiveBarrel")
                            _hit.transform.gameObject.GetComponent<ExplosiveBarrel>().Explode();
                    }

                    LineRendererManager.Instance.RenderShotLine(_lineKey, gunPoint.position, _hit.point, lazerBeamFadeSpeed);

                    var particles = Pool.Instance.GetNotActivated(_particleKey);
                    particles.transform.position = gunPoint.position;
                    particles.transform.rotation = gunPoint.rotation;
                    particles.SetActive(true);

                    CameraShaker.Instance.ShakeOnce(1f, 1f, 0.1f, 0.1f);
                    _anim.SetTrigger("Shoot");
                }
                /* Pistol Fire Particles*/
            }
            return true;
        }    
    }
}