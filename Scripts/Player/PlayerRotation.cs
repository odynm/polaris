// Polaris - Project VII 2018
// 08/03/18 -> 02/07/18
// Compiled in Unity 2017.2.0f3
// Writed by Mathias Ody (M-Ody)
// mathiasluizody@hotmail.com
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotation : MonoBehaviour {

    private Vector3 currentLocal;

    private void Start()
    {
        currentLocal = transform.localEulerAngles; ;
    }

    public void RotateHead (float pitch)
    {
        currentLocal.x = Mathf.Clamp(currentLocal.x -= pitch, -80f,80f);
        transform.localEulerAngles = currentLocal;
    }
}
