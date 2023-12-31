using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Pooling {
    /// <summary>
    /// 각 오브젝트들에 대한 개개의 풀입니다.
    /// </summary>
    public class SubPool {
        public readonly string itemName;
        public readonly GameObject originalPrefab;
        public readonly Transform poolRoot;
        internal readonly Queue<GameObject> released = new Queue<GameObject>();
        
        public SubPool(string itemName, GameObject originalPrefab, Transform poolRoot) {
            this.itemName = itemName;
            this.originalPrefab = originalPrefab;
            this.poolRoot = poolRoot;
        }

        public GameObject Get(Transform root = null, bool forceInstantiate = false) {
            GameObject gameObject;
            if (released.Any() && forceInstantiate == false) {
                gameObject = released.Dequeue();
                gameObject.transform.SetParent(root);
            } else {
                gameObject = Object.Instantiate(originalPrefab, root);
            }
            
            if (root) gameObject.transform.localScale = Vector3.one;
            return gameObject;
        }

        public void Release(GameObject gameObject) {
            gameObject.SetActive(false);
            gameObject.transform.SetParent(poolRoot);
            released.Enqueue(gameObject);
        }

        public void WarmUp(int count) {
            for (int i = 0; i < count; i++) {
                Release(Get(forceInstantiate: true));
            }
        }

        public void Clear(bool clearMold) {
            while (released.Any()) {
                Object.Destroy(released.Dequeue());
            }

            if (clearMold) {
                Object.Destroy(originalPrefab);
            }
        }
    }
}