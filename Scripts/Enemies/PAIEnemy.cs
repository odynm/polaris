// Polaris - Project VII 2018
// 08/03/18 -> 02/07/18
// Compiled in Unity 2017.2.0f3
// Writed by Mathias Ody (M-Ody)
// mathiasluizody@hotmail.com
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using PolarisCore;

namespace PAI
{
    public class PAIEnemy : MonoBehaviour
    {

        public GameObject head;
        public GameObject hips;
        public bool canRoam;
        public int health = 100;

        private float nextAICycle = IA_CYCLE_STEP;
        private const float IA_CYCLE_STEP = 0.2f;

        private PAIStateMachine _states;
        private EnemyEffectsManager _effects;
        private Animator _anim;
        private NavMeshAgent _nav;
        private BoxCollider _col;
        private Rigidbody[] _rbs;

        private bool _canRoamArround;
        private Vector3[] _roamBounds;
        private bool _dead = false;

        private int _bloodSmokeKey;
        private int _bloodSprayKey;
        private int _smallBloodHitKey;
        private int _bigBloodHitKey;
        private int _smallFleshKey;
        private int _smallFleshKeyHeadKey;

        private int _alienTakeHitSoundKey;
        private int _gutsExplosionKey;

        private Door _lastDoorEntered;

        private Vector3[] _fleshFlyDirections = new Vector3[6];

        private int _alienPatrolKey;

        private void Awake()
        {
            _nav = GetComponent<NavMeshAgent>();
            _anim = GetComponent<Animator>();
            _states = GetComponent<PAIStateMachine>();
            _effects = GetComponent<EnemyEffectsManager>();
            _col = GetComponent<BoxCollider>();
            _rbs = hips.GetComponentsInChildren<Rigidbody>();

            SetRigidbodiesState(_rbs, true);

            Random.State s = Random.state;
            for (int i = 0; i < _fleshFlyDirections.Length; i++)
            {
                Random.InitState(i*10);
                _fleshFlyDirections[i] = Random.insideUnitSphere;
            }
            Random.state = s;
        }

        private void Start()
        {
            _bloodSmokeKey = Pool.Instance.GetSharedPoolKey("BloodSmoke");
            _bloodSprayKey = Pool.Instance.GetSharedPoolKey("BloodSpray");
            _smallBloodHitKey = Pool.Instance.GetSharedPoolKey("SmallBloodHit");
            _bigBloodHitKey = Pool.Instance.GetSharedPoolKey("BigBloodHit");
            _smallFleshKey = Pool.Instance.GetSharedPoolKey("SmallFlesh");
            _smallFleshKeyHeadKey = Pool.Instance.GetSharedPoolKey("SmallFleshHead");

            _gutsExplosionKey = Pool.Instance.GetSharedPoolKey("AlienGutsExplosion");

            if (gameObject.name.Contains("Enemy1"))
            {
                _alienTakeHitSoundKey = Pool.Instance.GetSharedPoolKey("Alien1TakeHit");
            }
            else if (gameObject.name.Contains("Enemy2"))
            {
                _alienTakeHitSoundKey = Pool.Instance.GetSharedPoolKey("Alien2TakeHit");
            }

            Invoke("InitializeStates", 1f);
        }

        private void InitializeStates()
        {
            _states.InitializeObjects();
            nextAICycle = Time.timeSinceLevelLoad + IA_CYCLE_STEP;
        }

        private void Update()
        {
            _anim.SetFloat("Speed", _nav.velocity.magnitude);

            if (!_dead && Time.timeSinceLevelLoad > nextAICycle)
            {
                nextAICycle += IA_CYCLE_STEP;
                _states.Execute();
            }
        }

        public void SetDoor(Door door)
        {
            _lastDoorEntered = door;
        }

        public void SetRoaming(Vector2[] points, bool isPrioritary)
        {
            if (canRoam)
            {
                GetComponent<PAIRoaming>().Setup(points, isPrioritary);
            }
        }

        public void ActivateEnemyAttack()
        {
            _states.ActivateEnemyAttack();
        }

        public void ActivateManualControl()
        {
            _states.ActivateManualControl();
        }

        //
        // Organizar melhor sistema decal-animação-partícula
        //
        //(outra classe)

        public void BodyHit(Vector3 location, Vector3 normal, EGunType gun, int damage, Transform t)
        {
            ApplyDamage(damage);

            if (gun == EGunType.PISTOL)
            {
                BloodHit(location, normal, _smallBloodHitKey, t);
                if (_dead)
                    t.GetComponent<Rigidbody>().AddForceAtPosition(-normal * damage * 80f, location);
            }
            else if (gun == EGunType.SHOTGUN)
            {
                BloodHit(location, normal, _bigBloodHitKey, t);
                _effects.SpawnParticlesFromBack(location, normal);
                if (_dead)
                    t.GetComponent<Rigidbody>().AddForceAtPosition(-normal * damage * 140f,location);
            }
            else if (gun == EGunType.ROCKET_LAUNCHER)
            if(damage>=100)
                ExplodeBody();
        }

        public void HeadHit(Vector3 location, Vector3 normal, EGunType gun, int damage, Transform parent)
        {
            BloodHit(location, normal, _smallFleshKeyHeadKey, parent);

            ApplyDamage(damage);
        }

        public void ExplosionHit(int damage)
        {
            BodyHit(Vector3.zero, Vector3.zero, EGunType.ROCKET_LAUNCHER,damage, transform);
        }

        public bool IsDead()
        {
            return _dead;
        }

        //Retirado do jogo
        /*private void BlowHeadOff()
        {
            head.SetActive(false);
            BloodSmookeExplosion(head.transform.position, head.transform.rotation);
            for(int i = 0; i < 3; i++)
            {
                GameObject obj = Pool.Instance.Get(_smallHeadFleshKey);
                obj.transform.position = head.transform.position;
                obj.transform.rotation = Quaternion.Euler(Random.Range(0f,360f), Random.Range(0f, 360f), Random.Range(0f, 360f));
                obj.GetComponent<Rigidbody>().AddForce(obj.transform.forward * 250f);
            }
        }*/

        private void BloodHit(Vector3 location, Vector3 normal, int key, Transform parent)
        {
            Quaternion rotation = Quaternion.LookRotation(normal);
            GameObject obj = Pool.Instance.GetNotActivated(key);
            obj.transform.position = location + (normal * 0.02f);
            obj.transform.rotation = rotation;
            obj.transform.parent = parent;
            obj.SetActive(true);
            obj.GetComponent<BloodDecal>().Reset();

            _effects.PlaceBloodPoolDecalOnFloor();
        }

        private void BloodSmookeExplosion(Vector3 location, Quaternion rotation)
        {
            GameObject obj = Pool.Instance.Get(_bloodSmokeKey);
            obj.transform.position = location;

            obj = Pool.Instance.Get(_bloodSprayKey);
            obj.transform.position = location;
            obj.transform.rotation = rotation;
        }

        private void ExplodeBody()
        {
            SoundManager.Play(_gutsExplosionKey, transform);

            Vector3 spawner = hips.transform.position;
            GameObject obj;
            for (int i = 0; i < 6; i++)
            {
                obj = Pool.Instance.Get(_smallFleshKey);
                obj.transform.localPosition = spawner;
                obj.GetComponent<Rigidbody>().AddForce(_fleshFlyDirections[i] * 250f);
            }
            gameObject.SetActive(false);

            BloodSmookeExplosion(spawner, Quaternion.identity);
            _effects.PlaceBloodPoolDecalOnFloor();
            _effects.PlaceDecalOnWalls();


            //se explodir, feche as portas que dependiam desse cara
            if (_lastDoorEntered != null)
                _lastDoorEntered.SubtractEnemy();
        }

        private void ApplyDamage(int damage)
        {
            _states.ActivateEnemyAttack();

            if (!_dead)
                SoundManager.Play(_alienTakeHitSoundKey, transform);

            health -= damage;
            if (health <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            _anim.enabled = false;
            _col.enabled = false;
            _nav.enabled = false;

            SetRigidbodiesState(_rbs, false);

            _dead = true;
        }

        private void SetRigidbodiesState (Rigidbody[] rbs, bool state)
        {
            foreach (Rigidbody rb in rbs)
            {
                rb.isKinematic = state;
            }
        }
    }

}