// Polaris - Project VII 2018
// 08/03/18 -> 02/07/18
// Compiled in Unity 2017.2.0f3
// Writed by Mathias Ody (M-Ody)
// mathiasluizody@hotmail.com
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PolarisCore;

public class Spit : MonoBehaviour 
{
    public int spitDamage;

    private int _goopKey;
    private int _goopSoundKey;
    private int _goopSoundHitPlayerKey;

    private List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();
    private ParticleSystem _particles;

    private void Awake()
    {
        _particles = GetComponent<ParticleSystem>();
    }

    private void Start()
    {
        _goopKey = Pool.Instance.GetSharedPoolKey("EnemyGoopDecal");
        _goopSoundKey = Pool.Instance.GetSharedPoolKey("EnemyGoopSound");
        _goopSoundHitPlayerKey = Pool.Instance.GetSharedPoolKey("EnemyGoopPlayerSound");
    }

    private void OnParticleCollision(GameObject other)
    {
        if (((1 << other.transform.gameObject.layer) & LayerMaskData.player) != 0)
        {
            other.GetComponent<PlayerController>().Hit(spitDamage);
            gameObject.SetActive(false);
            SoundManager.Play(_goopSoundHitPlayerKey, transform);
        }
        else if (((1 << other.transform.gameObject.layer) & LayerMaskData.recieveBloodDecal) != 0)
        {
            gameObject.SetActive(false);
            AddDecal(ref other);
            SoundManager.Play(_goopSoundKey, transform);
        }
        else
        {
            gameObject.SetActive(false);
            SoundManager.Play(_goopSoundKey, transform);
        }
    }

    private void AddDecal(ref GameObject other)
    {
        _particles.GetCollisionEvents(other, collisionEvents);
        var decal = Pool.Instance.Get(_goopKey);
        decal.transform.position = collisionEvents[0].intersection;
        decal.transform.rotation = Quaternion.LookRotation(-collisionEvents [0].normal);
    }
}
