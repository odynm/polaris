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
    public class PAIAttackRange : PAIState, IPAIAttack
    {
        public float speed = 5f;
        public float rotationSpeed = 4000f;
        public float rangeHitDistance = 6f;
        public float rangeCooldown = 4f;
        public int meleeDamage = 10;
        public GameObject enemyHead;

        private int _particles;

        private readonly float DISTANCE_HIT = 1.5f;
        private readonly float DISTANCE_FOLLOW = 3f; 

        private string _lastTagHitted = "";

        private PAIMovement _movement;
        private PAIFrontSensor _sensor;
        private PAIEnemy _enemy;
        private Animator _anim;

        private GameObject _player;
        private PlayerController _playerScript;
        private NavMeshAgent _agent;
        private Rigidbody _rb;

        private bool _isClose;
        private bool _canFollow = true;
        private bool _isAttacking = false;
        private bool _rotateToPlayer = false;

        private bool _isOnMelee = false;
        private float _nextRangeAttack;
        private bool _isRangeAttacking = false;

        private int _attackHitKey;
        private int _attackKey;
        private int _preSpitKey;
        private int _spitKey;

        private void Awake()
        {
            _anim = GetComponent<Animator>();
            _movement = GetComponent<PAIMovement>();
            _agent = GetComponent<NavMeshAgent>();
            _sensor = GetComponentInChildren<PAIFrontSensor>();
            _enemy = GetComponent<PAIEnemy>();
            _rb = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            _player = GameObject.FindWithTag("Player");
            _playerScript = _player.GetComponent<PlayerController>();
            _particles = Pool.Instance.GetSharedPoolKey("Spit");

            _attackHitKey = Pool.Instance.GetSharedPoolKey("AlienHitAttack");
            _attackKey = Pool.Instance.GetSharedPoolKey("Alien2Attack");
            _preSpitKey = Pool.Instance.GetSharedPoolKey("Alien2PreSpit");
            _spitKey = Pool.Instance.GetSharedPoolKey("Alien2Spit");
        }

        public override void Execute()
        {
            if (_canFollow)
            {
                _agent.speed = speed;
                _movement.GoTo(_player.transform.position);
                if (!_isOnMelee && !_isRangeAttacking && Vector3.Distance(_player.transform.position, transform.position) < rangeHitDistance)
                {
                    //_movement.GoTo(transform.position);
                    //transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(_player.transform.position - transform.position, transform.up), 2f);

                    if (_nextRangeAttack < Time.timeSinceLevelLoad)
                    {
                        AttackRange();
                        _rotateToPlayer = true;
                    }
                }
                else if (!_isOnMelee && Vector3.Distance(_player.transform.position, transform.position) < 3f)
                {
                    _isOnMelee = true;
                }

                else if (_isRangeAttacking)
                {
                    _movement.GoTo(transform.position);
                }
            }
        }

        public void OnAttack()
        {
            if (_isClose)
            {
                SoundManager.Play(_attackHitKey, transform);
                _playerScript.Hit(meleeDamage);
            }
            SoundManager.Play(_attackKey, transform);
        }

        public void OnSpit()
        {
            SoundManager.Play(_spitKey, transform);
            var particle = Pool.Instance.Get(_particles);
            particle.transform.position = enemyHead.transform.position;
            Vector3 difference = (_player.transform.position + _player.transform.up) - enemyHead.transform.position;
            particle.transform.rotation = Quaternion.LookRotation(difference);
        }

        public void RangeAttackDone()
        {
            _nextRangeAttack = Time.timeSinceLevelLoad + rangeCooldown;
            _isRangeAttacking = false;
            _rotateToPlayer = false;
        }

        private void AttackRange()
        {
            _isRangeAttacking = true;

            _anim.SetTrigger("RangeAttack");
            SoundManager.Play(_preSpitKey, transform);
        }

        private void Update()
        {
            if (!_enemy.IsDead())
            {
                if (_rotateToPlayer)
                {
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(_player.transform.position - transform.position, transform.up), Time.deltaTime * rotationSpeed);
                }

                if (_sensor.Hitted(out _lastTagHitted))
                {
                    if (_lastTagHitted == "Player")
                    {
                        CanAttack(true);
                        _nextRangeAttack = Time.timeSinceLevelLoad + rangeCooldown;
                    }
                }

                if (_sensor.Exited(out _lastTagHitted))
                {
                    if (_lastTagHitted == "Player")
                    {
                        CanAttack(false);
                    }
                }


                if (_isClose)
                {
                    _agent.isStopped = true;
                    _agent.SetDestination(transform.position);
                    _canFollow = false;

                    if (Vector3.Dot(transform.forward, _player.transform.position - transform.position) > 0.9f)
                    {
                        if (!_isAttacking)
                            HitPlayer();
                    }
                    else
                    {
                        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(_player.transform.position - transform.position, transform.up), Time.deltaTime * rotationSpeed);
                    }
                }

                if (!_isOnMelee && !_isRangeAttacking && Vector3.Distance(_player.transform.position, transform.position) < 6f)
                {
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(_player.transform.position - transform.position, transform.up), 2f);
                }
            }
        }

        private void Attacked()
        {
            _isAttacking = false;
        }

        private void CanAttack(bool flag)
        {
            if (flag)
            {
                _isClose = true;
            }
            else if (_isClose == true)
            {
                _agent.isStopped = false;
                _isClose = false;
                _canFollow = true;
            }  
        }

        private void HitPlayer()
        {
            _anim.SetTrigger("Attack");

            _isAttacking = true;
        }
    }
}