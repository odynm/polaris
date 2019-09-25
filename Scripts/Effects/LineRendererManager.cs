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
    public class LineRendererManager : ScriptableObject
    {
        private static LineRendererManager _instance;

        public static LineRendererManager Instance
        {
            get
            {
                if (!_instance)
                    _instance = CreateInstance<LineRendererManager>();
                return _instance;
            }
        }

        private Laser _laser;

        public void RenderShotLine(int lineKey, Vector3 initialRayPos, Vector3 finalRayPos, float fadeRate)
        {
            _laser = Pool.Instance.Get(lineKey).GetComponent<Laser>();
            _laser.Activate(initialRayPos,finalRayPos,fadeRate);
        }
    }
}
