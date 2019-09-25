// Polaris - Project VII 2018
// 08/03/18 -> 02/07/18
// Compiled in Unity 2017.2.0f3
// Writed by Mathias Ody (M-Ody)
// mathiasluizody@hotmail.com
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PolarisCore;

public class ExplosiveBarrel : MonoBehaviour 
{
    private int _explosionKey;
    private int _burnKey;

    private bool _alreadyExploded;

    private void Start()
    {
        _explosionKey = Pool.Instance.GetSharedPoolKey("Explosion");
        _burnKey = Pool.Instance.GetSharedPoolKey("Burn");
    }

    public void Explode()
    {
        if (!_alreadyExploded)
        {
            _alreadyExploded = true;

            var explosion = Pool.Instance.GetNotActivated(_explosionKey);
            explosion.transform.position = transform.position;
            explosion.transform.rotation = Quaternion.Euler(transform.up);
            explosion.SetActive(true);

            var burn = Pool.Instance.Get(_burnKey);
            burn.transform.position = transform.position;
            burn.transform.rotation = Quaternion.Euler(transform.up);

            gameObject.SetActive(false);
        }
    }
}
