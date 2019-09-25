// Polaris - Project VII 2018
// 08/03/18 -> 02/07/18
// Compiled in Unity 2017.2.0f3
// Writed by Mathias Ody (M-Ody)
// mathiasluizody@hotmail.com
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PAI
{
    public class PAISensor : MonoBehaviour {

        private PAIView view;
        private PAIView.ViewSensorEnum sensorType;

        private Collider otherCol;

        void Start()
        {
            view = transform.parent.gameObject.GetComponent<PAIView>();
        }

        void OnTriggerEnter(Collider other)
        {   
            otherCol = other;
            view.SensorActivated(sensorType, other);
        }

        void FixedUpdate()
        {
            if (otherCol != null)
                view.SensorActive(sensorType, otherCol);
        }

        /*void OnTriggerStay(Collider other)
        {
            view.SensorActive(sensorType, other);
        }*/

        void OnTriggerExit(Collider other)
        {
            otherCol = null;
            view.SensorDeactivated(sensorType, other);
        }

        public void SetSensorType(PAIView.ViewSensorEnum sensorType)
        {
            this.sensorType = sensorType;
        }
    }
}