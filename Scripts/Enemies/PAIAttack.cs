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
    public class PAIAttack : PAIState, IPAIAttack
    {
        public float speed = 5f;
        public float rotationSpeed = 4000f;
        public int meleeDamage = 10;

        private readonly float DISTANCE_HIT = 2.5f;
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
        private bool _spotted = false;

        private int _attack1Key;
        private int _attackHitKey;

        private void Start()
        {
            _anim = GetComponent<Animator>();
            _player = GameObject.FindWithTag("Player");
            _playerScript = _player.GetComponent<PlayerController>();
            _movement = GetComponent<PAIMovement>();
            _agent = GetComponent<NavMeshAgent>();
            _sensor = GetComponentInChildren<PAIFrontSensor>();
            _enemy = GetComponent<PAIEnemy>();
            _rb = GetComponent<Rigidbody>();

            _attack1Key = Pool.Instance.GetSharedPoolKey("Alien1Attack");
            _attackHitKey = Pool.Instance.GetSharedPoolKey("AlienHitAttack");
        }

        public override void Execute()
        {
            _spotted = true;

            if (_canFollow)
            {
                _agent.speed = speed;
                _movement.GoTo(_player.transform.position);
            }
        }

        public void OnAttack()
        {
            if (Vector3.Distance(transform.position, _player.transform.position) < 3f)
            {
                _playerScript.Hit(meleeDamage);
                SoundManager.Play(_attackHitKey, transform);
                SoundManager.Play(_attack1Key, transform);
            }
        }

        private void Update()
        {
            if (_spotted)
            {
                //transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(_player.transform.position - transform.position, transform.up), Time.deltaTime * rotationSpeed);
            }

            if (!_enemy.IsDead())
            {
                if (_sensor.Hitted(out _lastTagHitted))
                {
                    if (_lastTagHitted == "Player")
                    {
                        CanAttack(true);
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

                    if (Vector3.Dot(transform.forward, _player.transform.position - transform.position) > 1f)
                    {
                        if (!_isAttacking)
                            HitPlayer();
                    }
                    else
                    {
                        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(_player.transform.position - transform.position, transform.up), Time.deltaTime * rotationSpeed);
                    }
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