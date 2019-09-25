// Polaris - Project VII 2018
// 08/03/18 -> 02/07/18
// Compiled in Unity 2017.2.0f3
// Writed by Mathias Ody (M-Ody)
// mathiasluizody@hotmail.com
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace PAI
{
    public class PAIMovement : MonoBehaviour
    {
        private NavMeshAgent _nav;
        private PAIView _view;

        private Vector3 _target;

        void Start()
        {
            _nav = GetComponent<NavMeshAgent>();
            _view = GetComponent<PAIView>();
        }

        public void GoTo(Vector3 pos)
        {
            _target = pos;
            _nav.SetDestination(_target);
        }
    }
}