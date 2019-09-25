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
    public class Laser : MonoBehaviour
    {
        private LineRenderer _line;
        private Vector3 _initialRayPos, _finalRayPos;
        private float _rayT, _fadeRate;

        private void Awake()
        {
            _line = GetComponent<LineRenderer>();
        }

        private void Update()
        {
            if (_rayT != -1f)
            {
                _rayT += _fadeRate * Time.deltaTime;
                _line.SetPosition(0, Vector3.Lerp(_initialRayPos, _finalRayPos, _rayT));
                if (_rayT >= 1f)
                {
                    gameObject.SetActive(false);
                    _rayT = -1f;
                }
            }
        }

        public void Activate(Vector3 initialRayPos, Vector3 finalRayPos, float fadeRate)
        {

            _initialRayPos = initialRayPos;
            _finalRayPos = finalRayPos;
            _fadeRate = fadeRate;
            _rayT = 0f;
            _line.SetPosition(0, _initialRayPos);
            _line.SetPosition(1, _finalRayPos);
        }
    }
}
