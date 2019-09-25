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
    public class Shotgun : Gun
    {
        public Shotgun()
        {
            GUN_TYPE = EGunType.SHOTGUN;
        }

        public Transform gunPoint;
        public GameObject wallHit;
        public GameObject smallWallHit;
        public GameObject shotParticles;
        public float bonusHitMaxDistance = 5f;
        public float bonusHitRange = 6f;
        [Range(0f,1f)]
        public float randomFactor = 0.5f;
        [Header("Damage")]
        public int damage;

        public float forceOnObjects = 600f;
        private float forceFragmentsOnObjects;

        public GameObject particle;
        private int _particleKey;

        private const float NUMBER_BONUS_SHELLS = 3f;
        private const float DISTANCE_BONUS_RAYS = 2f;

        private int _decalKey;
        private int _smallDecalKey;
        private int _dynamicDecalKey;
        private int _dynamicSmallDecalKey;
        private int _particlesKey;
        private int _changeToShotgunKey;
        private float _decalSize;
        private float _smallDecalSize;
        private Ray _ray;
        private RaycastHit _hit;
        private Vector3[] _randomDirections = new Vector3[17];
        private int _nextRandomDirection;
        private Animator _anim;

        private int _shotgunHitEnemy;
        private int _shotgunHitOther;
        private int _pistolHitEnemy;
        private int _pistolHitOther;
        private int _shotgunShotKey;

        private void Awake()
        {
            base.Initialize();

            _decalKey = Pool.Instance.AddSimplePoolObjectDecal(wallHit, 10, true);
            _smallDecalKey = Pool.Instance.AddSimplePoolObjectDecal(smallWallHit, 10, true);
            _dynamicDecalKey = Pool.Instance.AddSimplePoolObjectDecal(wallHit, 10, true);
            _dynamicSmallDecalKey = Pool.Instance.AddSimplePoolObjectDecal(smallWallHit, 10, true);
            _particlesKey = Pool.Instance.AddSimplePoolObject(shotParticles, 10);

            _decalSize = wallHit.GetComponentInChildren<Renderer>().bounds.extents.x;
            _smallDecalSize = smallWallHit.GetComponentInChildren<Renderer>().bounds.extents.x;

            SetUpRandomDirections();

            _particleKey = Pool.Instance.AddSimplePoolObject(particle, 10);

            forceFragmentsOnObjects = forceOnObjects / 2;
        }

        private void Start()
        {
            _anim = GetComponent<Animator>();

            _shotgunHitEnemy = Pool.Instance.GetSharedPoolKey("ShotgunHitEnemy");
            _shotgunHitOther = Pool.Instance.GetSharedPoolKey("ShotgunHitOther");
            _pistolHitEnemy = Pool.Instance.GetSharedPoolKey("PistolHitEnemy");
            _pistolHitOther = Pool.Instance.GetSharedPoolKey("PistolHitOther");
            _shotgunShotKey = Pool.Instance.GetSharedPoolKey("ShotgunShot");
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
                SoundManager.Play(_shotgunShotKey, transform);
                _inventory.DecreaseAmmo(GUN_TYPE);

                base.cooldownElapsed = Time.timeSinceLevelLoad + base.cooldownTime;

                _ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0));
                if (Physics.Raycast(_ray, out _hit, Camera.main.farClipPlane, LayerMaskData.recieveShots))
                {
                    if (((1 << _hit.transform.gameObject.layer) & LayerMaskData.enemyBullets) != 0)
                    {
                        base.HitEnemy(_hit, damage);
                        SoundManager.Play(_shotgunHitEnemy, _hit.transform);
                    }
                    else
                    {
                        SoundManager.Play(_shotgunHitOther, _hit.transform);

                        if (((1 << _hit.transform.gameObject.layer) & LayerMaskData.dynamicFly) != 0)
                            _hit.transform.gameObject.GetComponent<Rigidbody>().AddForceAtPosition(Vector3.Normalize(_hit.point - transform.position) * forceOnObjects, _hit.point);

                        if (((1 << _hit.transform.gameObject.layer) & LayerMaskData.recieveBulletDecal) != 0)
                            DecalManager.Instance.PlaceSimpleDecal(_decalKey, _dynamicDecalKey, ref _hit, ref _decalSize);

                        if (((1 << _hit.transform.gameObject.layer) & LayerMaskData.dynamic) != 0 && _hit.transform.gameObject.tag == "ExplosiveBarrel")
                            _hit.transform.gameObject.GetComponent<ExplosiveBarrel>().Explode();
                    }

                    if (_hit.distance < bonusHitMaxDistance)
                        if (_hit.distance > DISTANCE_BONUS_RAYS)
                            ShootBonusShells(_hit.point - (_hit.normal * -DISTANCE_BONUS_RAYS), _ray.direction);
                        else
                            ShootBonusShells(_ray.origin, _ray.direction);

                    CameraShaker.Instance.ShakeOnce(1.5f, 1.5f, 0.1f, 0.1f);
                }
                var particles = Pool.Instance.Get(_particlesKey);
                particles.transform.position = gunPoint.position;
                particles.transform.rotation = gunPoint.rotation;

                var muzzle = Pool.Instance.Get(_particleKey);
                muzzle.transform.position = gunPoint.position;
                muzzle.transform.rotation = gunPoint.rotation;

                _anim.SetTrigger("Shoot");
            }
            return true;
        }

        private void ShootBonusShells(Vector3 origin, Vector3 direction)
        {
            for (int i = 0; i < NUMBER_BONUS_SHELLS; i++)
            {
                if (Physics.Raycast (origin, GetRandomDirection (direction), out _hit, bonusHitRange, LayerMaskData.recieveShots))
                {
                    if (((1 << _hit.transform.gameObject.layer) & LayerMaskData.enemyBullets) != 0)
                    {
                        base.HitEnemy(_hit, EGunType.PISTOL, damage); //os fragmentos são tratados como tiros de pistola
                        SoundManager.Play(_pistolHitEnemy, _hit.transform);
                    }
                    else
                    {
                        SoundManager.Play(_pistolHitOther, _hit.transform);

                        if (((1 << _hit.transform.gameObject.layer) & LayerMaskData.dynamicFly) != 0)
                            _hit.transform.gameObject.GetComponent<Rigidbody>().AddForceAtPosition(Vector3.Normalize(_hit.point - transform.position) * forceFragmentsOnObjects, _hit.point);
                    
                        if (((1 << _hit.transform.gameObject.layer) & LayerMaskData.recieveBulletDecal) != 0)
                            DecalManager.Instance.PlaceSimpleDecal(_smallDecalKey, _dynamicSmallDecalKey, ref _hit, ref _smallDecalSize);

                        if (((1 << _hit.transform.gameObject.layer) & LayerMaskData.dynamic) != 0 && _hit.transform.gameObject.tag == "ExplosiveBarrel")
                            _hit.transform.gameObject.GetComponent<ExplosiveBarrel>().Explode();
                    }
                }
            }
        }

        private void SetUpRandomDirections()
        {
            for (int i = 0; i < _randomDirections.Length; i++)
            {
                _randomDirections [i] = new Vector3 (Random.Range (-randomFactor, randomFactor),
                                                    Random.Range (-randomFactor, randomFactor),
                                                    Random.Range (-randomFactor, randomFactor));
            }
        }

        private Vector3 GetRandomDirection(Vector3 direction)
        {
            _nextRandomDirection++;
            if (_nextRandomDirection == _randomDirections.Length)
                _nextRandomDirection = 0;
            return direction + _randomDirections[_nextRandomDirection];
        }
    }
}
