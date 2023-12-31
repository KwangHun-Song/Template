using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Pooling {
    public class PoolManager : IPool {
        public Dictionary<string, SubPool> SubPools { get; } = new();
        public Transform PoolRoot { get; }

        public PoolManager(Transform poolRoot) {
            PoolRoot = poolRoot;
        }

        public T Get<T>(string itemName, Transform root) {
            if (SubPools.TryGetValue(itemName, out var subPool) == false) {
                subPool = CreateSubPool(itemName);
                SubPools.Add(itemName, subPool);
            }

            return subPool.Get(root).GetComponent<T>();
        }

        public void Release(GameObject gameObject) {
            if (gameObject == null) {
                return;
            }
            
            var recycler = gameObject.GetComponent<Recycler>();
            if (recycler == null) {
                Object.Destroy(gameObject);
                return;
            }

            if (SubPools.ContainsKey(recycler.ItemName) == false) {
                Object.Destroy(gameObject);
                return;
            }
            
            SubPools[recycler.ItemName].Release(gameObject);
        }

        public UniTask WarmUp(string itemName, int count, IProgress<float> progress) {
            if (SubPools.TryGetValue(itemName, out var subPool) == false) {
                subPool = CreateSubPool(itemName);
                SubPools.Add(itemName, subPool);
            }
            
            for (int i = 0; i < count; i++) {
                subPool.WarmUp(count);
                progress.Report(i / (float)count);
            }
            
            return UniTask.CompletedTask;
        }

        public void Clear(bool clearMold) {
            foreach (var subPool in SubPools.Values) {
                subPool.Clear(clearMold);
            }
        }

        protected SubPool CreateSubPool(string itemName) {
            var subPoolRoot = new GameObject(itemName).transform;
            subPoolRoot.SetParent(PoolRoot);
            var pool = new SubPool(itemName, LoadPrefab(itemName), subPoolRoot);
            return pool;
        }

        protected GameObject LoadPrefab(string itemName) {
            var prefab = Resources.Load(itemName) as GameObject;
            if (prefab == null) return default;
            if (prefab.GetComponent<Recycler>() != null) return prefab;
            
            var recycler = prefab.AddComponent<Recycler>();
            recycler.ItemName = itemName;

            return prefab;
        }
    }
}