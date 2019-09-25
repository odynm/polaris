// Polaris - Project VII 2018
// 08/03/18 -> 02/07/18
// Compiled in Unity 2017.2.0f3
// Writed by Mathias Ody (M-Ody)
// mathiasluizody@hotmail.com
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PAI;
using EZCameraShake;

namespace PolarisCore
{
    public class Explosion : MonoBehaviour 
    {
        public float range = 8f;
        public float explosionForce = 1000f;
        public float explosionDamage = 200f;

        private Rigidbody _rb;
        private PAIEnemy _enemy;
        private PlayerController _player;
        private RaycastHit _hit;

        private int _explosionSoundKey;

        private ParticleSystem [] _particles = new ParticleSystem[4];

        private Vector3 _halfUp;

        private void Awake()
        {
            _particles = GetComponentsInChildren<ParticleSystem>();
            _halfUp = Vector3.up * 0.5f;
        }

        private void OnEnable()
        {
            if (_explosionSoundKey == 0)
                _explosionSoundKey = Pool.Instance.GetSharedPoolKey("ExplosionSound");

            foreach (var particle in _particles)
            {
                particle.gameObject.SetActive(true);
            }

            var colliders = Physics.OverlapSphere (transform.position, range, LayerMaskData.recieveExplosionCheck);
            //FLECK ESTEVE AQUI
            CameraShaker.Instance.ShakeOnce(3f, 3f, 0.2f, 2f);
            SoundManager.Play(_explosionSoundKey, transform);

            if (colliders.Length > 0)
            {
                foreach (var c in colliders)
                {
                    //Checar se bate em paredes antes de alcançar objeto
                    if (Physics.Raycast(transform.position + _halfUp, (c.transform.position - transform.position), out _hit, range, 
                        LayerMaskData.barriers | LayerMaskData.enemy | LayerMaskData.player | LayerMaskData.dynamicFly |LayerMaskData.dynamic))
                    {
                        if (((1 << _hit.transform.gameObject.layer) & LayerMaskData.barriers) != 0)
                        {
                            continue;
                        }

                        if (((1 << _hit.transform.gameObject.layer) & LayerMaskData.enemy) != 0)
                        {
                            //se bater em outro inimigo primeiro E estiver à mais de +/- 2/3 de distância, não receberá dano
                            if (_hit.transform.gameObject.GetInstanceID() != c.gameObject.GetInstanceID()
                                && Vector3.Distance(c.transform.position, transform.position) > (0.7f * range))
                            {
                                continue;
                            }
                            
                            if ((_enemy = c.GetComponent<PAIEnemy>()) != null)
                                _enemy.ExplosionHit((int)(explosionDamage / Vector3.Distance(transform.position,c.transform.position)));
                            
                            if ((_rb = c.GetComponent<Rigidbody>()) != null)
                                _rb.AddExplosionForce (explosionForce, transform.position, range);
                        }

                        if (((1 << _hit.transform.gameObject.layer) & LayerMaskData.dynamicFly) != 0)
                        {
                            if ((_rb = c.GetComponent<Rigidbody>()) != null)
                            {
                                _rb.AddExplosionForce (explosionForce, transform.position, range);
                            }
                        }

                        if (((1 << _hit.transform.gameObject.layer) & LayerMaskData.player) != 0)
                        {
                            if ((_player = c.GetComponentInParent<PlayerController>()) != null)
                                _player.Hit((int)(70f / Vector3.Distance(transform.position, c.transform.position)));
                        }

                        if (((1 << _hit.transform.gameObject.layer) & LayerMaskData.dynamic) != 0 && c.gameObject.tag == "ExplosiveBarrel")
                            c.gameObject.GetComponent<ExplosiveBarrel>().Explode();
                    }
                }
            }
        }

        private void OnDisable()
        {
            foreach (var particle in _particles)
            {
                particle.Stop();
            }
        }
    }
}
