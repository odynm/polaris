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
    public class PAIRoaming : PAIState
    {
        private Rigidbody rb;

        private PAIMovement movement;
        private PAIFrontSensor sensor;

        private bool canRoamArround;
        private Vector2[] roamBounds;

        private Vector3 target;

        private string lastTagHitted = "";

        private void Awake()
        {
            sensor = GetComponentInChildren<PAIFrontSensor>();
            movement = GetComponent<PAIMovement>();
            rb = GetComponent<Rigidbody>();
            target = transform.position;
        }

        public override void Execute()
        {
            if (canRoamArround)
            {
                if (rb.velocity.magnitude < 0.1f)
                {
                    RandomizeTarget();
                }
                /*if (Mathf.Approximately(transform.position.x, target.x) && Mathf.Approximately(transform.position.y, target.y))
                {
                    RandomizeTarget();
                }

                if (sensor.Hitted(out lastTagHitted))
                {
                    if (lastTagHitted == "EnemyFrontSensor")
                    {
                        //StepBackWhenBlocked();
                        RandomizeTarget();
                    }
                }*/
            }
        }

        public void CanRoam(bool can)
        {
            canRoamArround = can;
            RandomizeTarget();
        }

        public void Setup(Vector2[] points, bool isPrioritary)
        {
            if (roamBounds != null && !isPrioritary) { return; }
            roamBounds = points;
        }

        private void RandomizeTarget()
        {
            if (roamBounds != null)
            {
                float x;
                float z;
                while (true)
                {
                    x = Random.Range(roamBounds[0].x, roamBounds[1].x);
                    z = Random.Range(roamBounds[0].y, roamBounds[1].y);
                    target = new Vector3(x, transform.position.y, z);

                    NavMeshHit hit;
                    if (!NavMesh.Raycast(transform.position, target, out hit, NavMesh.AllAreas))
                    {
                        break;
                    }
                }

                movement.GoTo(target);
            }
        }

        private void StepBackWhenBlocked()
        {
            if (roamBounds != null)
            {
                while (true)
                {
                    target = transform.position - transform.forward;

                    NavMeshHit hit;
                    if (!NavMesh.Raycast(transform.position, target, out hit, NavMesh.AllAreas))
                    {
                        break;
                    }
                }

                movement.GoTo(target);
            }
        }
    }
}