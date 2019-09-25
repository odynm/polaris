// Polaris - Project VII 2018
// 08/03/18 -> 02/07/18
// Compiled in Unity 2017.2.0f3
// Writed by Mathias Ody (M-Ody)
// mathiasluizody@hotmail.com
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.AI;
using PolarisCore;

namespace PAI
{
    public class PAIView : MonoBehaviour
    {

        [SerializeField]
        private float _visionRange = 5f;
        [SerializeField]
        private float _backVisionRange = 2f;

        private PAIEnemy _master;
        private GameObject _visionObj;
        private bool _foundPlayer;

        private int _foundPlayerKey;
        private int _patrolSoundKey;

        private float _nextPatrolSound;
        private float _patrolSoundTimestamp;

        private Vector3 _headPosition;

        public enum ViewSensorEnum
        {
            Vision
        }

        void OnDrawGizmosSelected()
        {
#if UNITY_EDITOR
            SceneView.onSceneGUIDelegate += OnScene;
#endif
        }
#if UNITY_EDITOR
        private void OnScene(SceneView sceneview)
        {
            Handles.color = Color.red;
            Handles.DrawWireArc(transform.position, transform.up, -transform.right, 180, _visionRange);

            SceneView.onSceneGUIDelegate -= OnScene;
        }
#endif

        void Awake()
        {
            SetupSensorObject(ViewSensorEnum.Vision, _visionRange, ref _visionObj);
            _headPosition = transform.up * 1.8f;
        }

        void Start()
        {
            _master = GetComponent<PAIEnemy>();

            if (gameObject.name.Contains("Enemy1"))
            {
                _foundPlayerKey = Pool.Instance.GetSharedPoolKey("Alien1FoundPlayer");
                _patrolSoundKey = Pool.Instance.GetSharedPoolKey("Alien1Patrol");
            } 
            else if (gameObject.name.Contains("Enemy2"))
            {
                _foundPlayerKey = Pool.Instance.GetSharedPoolKey("Alien2FoundPlayer");
                _patrolSoundKey = Pool.Instance.GetSharedPoolKey("Alien2Patrol");
            }

            _patrolSoundTimestamp = Random.Range(15f, 30f);
            _nextPatrolSound += _patrolSoundTimestamp;
        }

        void Update()
        {
            if (!_foundPlayer)
            {
                if (!_master.IsDead() && Time.timeSinceLevelLoad > _nextPatrolSound)
                {
                    SoundManager.Play(_patrolSoundKey, transform);
                    _nextPatrolSound += _patrolSoundTimestamp;
                }
            }
        }

        public bool Execute()
        {
            return _foundPlayer;
        }

        private void SetupSensorObject(ViewSensorEnum sensorType, float triggerRadius, ref GameObject obj)
        {
            obj = new GameObject(sensorType.ToString());
            obj.transform.parent = transform;
            obj.transform.position = transform.position;
            obj.layer = LayerMask.NameToLayer("EnemiesSensor");
            SphereCollider col = obj.AddComponent<SphereCollider>();
            col.isTrigger = true;
            col.radius = triggerRadius;
            PAISensor script = obj.AddComponent<PAISensor>();
            script.SetSensorType(sensorType);
        }

        public void SensorActivated(ViewSensorEnum sensor, Collider other)
        {
            switch (sensor)
            {
                case ViewSensorEnum.Vision:
                    TriggeredVision(other);
                    break;
                default:
                    break;
            }
        }

        public void SensorActive(ViewSensorEnum sensor, Collider other)
        {
            switch (sensor)
            {
                case ViewSensorEnum.Vision:
                    TriggerStayVision(other);
                    break;
                default:
                    break;
            }
        }

        public void SensorDeactivated(ViewSensorEnum sensor, Collider other)
        {
            switch (sensor)
            {
                case ViewSensorEnum.Vision:
                    ExitVision(other);
                    break;
                default:
                    break;
            }
        }

        private void TriggeredVision(Collider other)
        {

        }

        private void TriggerStayVision(Collider other)
        {
            if (!_foundPlayer && other.tag == "Player")
            {
                float distance = Vector3.Distance(other.transform.position, transform.position);

                //checar se algo obstrui a visão
                RaycastHit rayInfo;
                Vector3 headPos = transform.position + _headPosition;

                if (!Physics.Raycast(headPos, (other.transform.position + _headPosition) - headPos, out rayInfo, distance, LayerMaskData.enemySensor))
                {
                    //pode ver
                    if (IsInFront(other.transform))
                    {
                        _foundPlayer = true;
                        SoundManager.Play(_foundPlayerKey, transform);
                    }
                    //se já pode ver de costas
                    else if (distance < _backVisionRange)
                    {
                        _foundPlayer = true;
                        SoundManager.Play(_foundPlayerKey, transform);
                    }
                }
            }
        }

        private void ExitVision(Collider other)
        {

        }

        private bool IsInFront(Transform other)
        {
            Vector3 heading = Vector3.Normalize(transform.position - other.position);
            return Vector3.Dot(new Vector3 (transform.forward.x, 0f, transform.forward.z), new Vector3 (heading.x, 0f, heading.z)) < 0;
        }
    }
}
