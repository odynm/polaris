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
public class PreSensor : MonoBehaviour {

    private Door _door;

    void Start () 
    {
        _door = GetComponentInParent<Door>();
    }
    
    void OnTriggerEnter (Collider other) 
    {
        if (((1 << other.gameObject.layer) & LayerMaskData.player) != 0)
            _door.PreSensorActivated();
    }
}
}
