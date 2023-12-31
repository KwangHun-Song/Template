using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Pooling {
    public static class Pool {
        private static IPool poolManager;
        public static IPool PoolManager => poolManager ??= CreateDefaultPoolManager();

        /// <summary>
        /// 커스텀으로 풀매니저를 만들어서 적용할 수 있다. 초기화하지 않으면 기본 풀매니저가 생성된다.
        /// </summary>
        public static void Initialize(IPool customPoolManager) {
            poolManager = customPoolManager;
        }

        /// <summary>
        /// 풀에서 아이템을 가져온다. 없으면 생성한다.
        /// </summary>
        public static T Get<T>(string itemName, Transform root = null) => PoolManager.Get<T>(itemName, root);

        /// <summary>
        /// 풀로 아이템을 반납한다.
        /// </summary>
        public static void Release(GameObject gameObject) => PoolManager.Release(gameObject);

        /// <summary>
        /// 풀에 필요한 만큼 아이템을 생성해둔다.
        /// </summary>
        public static UniTask WarmUp(string itemName, int count, IProgress<float> progress = null) 
            => PoolManager.WarmUp(itemName, count, progress);

        /// <summary>
        /// 풀에 있는 모든 아이템을 제거한다.
        /// </summary>
        /// <param name="clearMold">원본 프리팹을 포함해서 제거할 지 여부</param>
        public static void Clear(bool clearMold = false) => PoolManager.Clear(clearMold);

        private static IPool CreateDefaultPoolManager() {
            var poolRoot = new GameObject("Pool");
            Object.DontDestroyOnLoad(poolRoot);

            return new PoolManager(poolRoot.transform);
        }
    }
}