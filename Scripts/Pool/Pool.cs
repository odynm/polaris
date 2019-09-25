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
    public class Pool : ScriptableObject
    {
        public static Pool _instance;

        public static Pool Instance
        {
            get
            {
                if (!_instance)
                    _instance = CreateInstance<Pool>();
                return _instance;
            }
        }

        private class SimplePool
        {
            public int next;
            public int size;
            public List<GameObject> pool;
        }

        private int _poolSize;
        private List<SimplePool> _pool;
        private Dictionary<string,int> _sharedPoolKeys;
        private float _decalsOffset = 0.0001f;

        public Pool()
        {
            _pool = new List<SimplePool>();
            _sharedPoolKeys = new Dictionary<string,int>();
        }

        public void Setup()
        {
            _decalsOffset = 0.0001f;
        }

        /// <summary>
        /// Simplest pool type. Iterates through objects releasing them until the end and starts again.
        /// </summary>
        /// <returns>Returns the key to the pool</returns>
        public int AddSimplePoolObject(GameObject obj, int amount, Transform parent = null, bool isActive = false)
        {
            var simplePool = new SimplePool();
            simplePool.pool = new List<GameObject>();
            simplePool.size = amount;

            for (int i = 0; i < amount; i++)
            {
                obj.SetActive(isActive);
                var temp = Instantiate(obj);
                if (parent != null)
                {
                    temp.transform.SetParent(parent);
                    temp.transform.position = parent.transform.position;
                    temp.transform.rotation = parent.transform.rotation;
                }
                simplePool.pool.Add(temp);
            }
            _pool.Add(simplePool);

            return _poolSize++;
        }

        public int AddSimplePoolObjectDecal(GameObject obj, int amount, bool randomRotation = false)
        {
            var simplePool = new SimplePool();
            simplePool.pool = new List<GameObject>();
            simplePool.size = amount;

            for (int i = 0; i < amount; i++)
            {
                var temp = Instantiate(obj);
                var childs = temp.transform.GetComponentsInChildren<Transform>();
                foreach (var t in childs)
                {
                    t.Translate(new Vector3(0f, 0f, _decalsOffset));
                    if (randomRotation)
                        t.Rotate(temp.transform.forward, Random.Range(0f, 360f));
                }
                _decalsOffset -= 0.0001f; //avoid z fighting

                temp.SetActive(false);
                simplePool.pool.Add(temp);
            }
            _pool.Add(simplePool);

            return _poolSize++;
        }

        public void AddSharedPool(string sharedPoolName,GameObject obj, int amount, bool isActive = false)
        {
            if (_sharedPoolKeys.ContainsKey(sharedPoolName)) Debug.Log(sharedPoolName);
            _sharedPoolKeys.Add (sharedPoolName,AddSimplePoolObject (obj, amount, null, isActive));
        }

        public void CleanSharedPool()
        {
            _sharedPoolKeys.Clear();
        }

        public void AddSharedPoolDecal(string sharedPoolName, GameObject obj, int amount, bool randomizeRotation = false)
        {
            _sharedPoolKeys.Add(sharedPoolName, AddSimplePoolObjectDecal(obj, amount, true));
        }

        public int GetSharedPoolKey(string sharedPoolName)
        {
            return _sharedPoolKeys [sharedPoolName];
        }

        public GameObject Get(int key)
        {
            if (_pool[key].next == _pool[key].size)
                _pool[key].next = 0;
            _pool[key].pool[_pool[key].next].SetActive(true);
            return _pool[key].pool[_pool[key].next++];
        }

        public GameObject GetNotActivated(int key)
        {
            if (_pool[key].next == _pool[key].size)
                _pool[key].next = 0;
            return _pool[key].pool[_pool[key].next++];
        }
    }
}