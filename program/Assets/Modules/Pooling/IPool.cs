using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Pooling {
    public interface IPool {
        T Get<T>(string itemName, Transform parent);
        void Release(GameObject gameObject);
        UniTask WarmUp(string itemName, int count, IProgress<float> progress);
        void Clear(bool clearMold);
    }
}