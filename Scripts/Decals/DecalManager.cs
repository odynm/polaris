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
    public class DecalManager : ScriptableObject
    {
        private static DecalManager _instance;

        public static DecalManager Instance
        {
            get
            {
                if (!_instance)
                    _instance = CreateInstance<DecalManager>();
                return _instance;
            }
        }

        public void PlaceSimpleDecal(int decalKey, int dynamicDecalKey,ref RaycastHit hit, ref float decalSize)
        {
            if (((1 << hit.transform.gameObject.layer) & LayerMaskData.recieveChildDecal) != 0)
            {
                var temp = Pool.Instance.Get (dynamicDecalKey);
                temp.transform.rotation = Quaternion.LookRotation (-hit.normal);
                temp.transform.position = FindInboundsPlacingPos (hit.point, hit.collider.bounds, decalSize, hit.normal) + (hit.normal * 0.00001f);
                temp.transform.SetParent (hit.transform);
            }
            else
            {
                var temp = Pool.Instance.Get (decalKey);
                temp.transform.rotation = Quaternion.LookRotation (-hit.normal);
                temp.transform.position = FindInboundsPlacingPos (hit.point, hit.collider.bounds, decalSize, hit.normal) + (hit.normal * 0.00001f);
            }
        }

        public void PlaceSimpleDecalWithParticles(int decalKey, int dynamicDecalKey, ref RaycastHit hit, ref float decalSize)
        {
            if (((1 << hit.transform.gameObject.layer) & LayerMaskData.recieveChildDecal) != 0)
            {
                var temp = Pool.Instance.Get(dynamicDecalKey);
                temp.transform.rotation = Quaternion.LookRotation(-hit.normal);
                temp.transform.position = hit.point + hit.normal * 0.0001f;
                temp.transform.SetParent(hit.transform);
                temp.GetComponent<ParticleSystem>().Play();
            }
            else
            {
                var temp = Pool.Instance.Get(decalKey);
                temp.transform.rotation = Quaternion.LookRotation(-hit.normal);
                temp.transform.position = hit.point + hit.normal * 0.0001f;
                temp.GetComponent<ParticleSystem>().Play();
            }
        }

        private Vector3 FindInboundsPlacingPos(Vector3 point, Bounds bounds, float decalSize, Vector3 normal)
        {
            if (Mathf.Abs(normal.z) > 0.0001f)
            {
                if (bounds.max.x != point.x)
                {
                    if (bounds.max.x < point.x + decalSize)
                        point.x -= (point.x + decalSize) - bounds.max.x;
                }
                if (bounds.min.x != point.x)
                {
                    if (bounds.min.x > point.x - decalSize)
                        point.x += bounds.min.x - (point.x - decalSize);
                }
            }

            if (Mathf.Abs(normal.x) > 0.0001f)
            {
                if (bounds.max.z != point.z)
                {
                    if (bounds.max.z < point.z + decalSize)
                        point.z -= (point.z + decalSize) - bounds.max.z;
                }
                if (bounds.min.z != point.z)
                {
                    if (bounds.min.z > point.z - decalSize)
                        point.z += bounds.min.z - (point.z - decalSize);
                }
            }

            if (bounds.max.y!= point.y)
            {
                if (bounds.max.y < point.y + decalSize)
                    point.y -= (point.y + decalSize) - bounds.max.y;
            }        
            if (bounds.min.y!= point.y)
            {
                if (bounds.min.y > point.y - decalSize)
                    point.y += bounds.min.y - (point.y - decalSize);
            }

            return point;
        }
    }
}
