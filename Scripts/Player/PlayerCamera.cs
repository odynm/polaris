// Polaris - Project VII 2018
// 08/03/18 -> 02/07/18
// Compiled in Unity 2017.2.0f3
// Writed by Mathias Ody (M-Ody)
// mathiasluizody@hotmail.com
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public GameObject o;
    private float speed = 5f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update ()
    {
        o.transform.Rotate(-Input.GetAxis("Mouse Y") * 1f, 0f, 0f);
        transform.Rotate(0f, Input.GetAxis("Mouse X") * 1f,0f);

        if (Input.GetKey(KeyCode.W))
        {
            transform.position += o.transform.forward * speed * Time.deltaTime;
        }

        else if  (Input.GetKey(KeyCode.S))
        {
            transform.position -= o.transform.forward * speed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.position += o.transform.right * speed * Time.deltaTime;
        }

        else if (Input.GetKey(KeyCode.A))
        {
            transform.position -= o.transform.right * speed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.E))
        {
            transform.position += Vector3.up * speed * Time.deltaTime;
        }

        else if (Input.GetKey(KeyCode.Q))
        {
            transform.position -= Vector3.up * speed * Time.deltaTime;
        }

        speed += Input.mouseScrollDelta.y * 0.3f;
    }
}
