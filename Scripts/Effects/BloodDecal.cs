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
    public class BloodDecal : MonoBehaviour 
    {
        private int _bloodSpashKey;

        private ParticleSystem _particle;
        private RaycastHit _hit;
        private bool _firstEnabled = true;

        private void Awake()
        {
            _particle = GetComponent<ParticleSystem>();
        }

        public void Reset()
        {
            _particle.Clear();
            _particle.Play();
        }

        private void OnEnable()
        {
            if (!_firstEnabled)
            {
                if (_bloodSpashKey == 0)
                {
                    _bloodSpashKey = Pool.Instance.GetSharedPoolKey("BloodSplash");
                }
                
                if (Physics.Raycast(transform.position, transform.forward - transform.up, out _hit, 2.5f, LayerMaskData.recieveBloodDecal))
                {
                    PlaceDecal(_bloodSpashKey, _hit.point, _hit.normal);
                }
            }
            else
            {
                _firstEnabled = false;
            }
        }

        private void PlaceDecal(int key, Vector3 position, Vector3 normal)
        {
            var obj = Pool.Instance.Get(_bloodSpashKey);
            obj.transform.position = position;
            obj.transform.rotation = Quaternion.LookRotation(-normal);
        }
    }
}