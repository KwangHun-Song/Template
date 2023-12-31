using System;
using Cysharp.Threading.Tasks;

namespace Recorder {
    public class CachingRecorder : IRecorder {
        protected MemoryRecorder CacheRecorder { get; }
        protected IRecorder MainRecorder { get; }

        public CachingRecorder(IRecorder mainRecorder) {
            CacheRecorder = new MemoryRecorder();
            MainRecorder = mainRecorder;
        }
        
        public T Get<T>(string key, T defaultValue) {
            if (HasKey(key) == false) return defaultValue;
            if (CacheRecorder.HasKey(key)) return CacheRecorder.Get(key, defaultValue);

            var loadedValue = MainRecorder.Get(key, defaultValue);
            CacheRecorder.Set(key, loadedValue);
            
            return loadedValue;
        }

        public UniTask Preload(string key, IProgress<float> progress) {
            return MainRecorder.Preload(key, progress);
        }

        public async UniTask<T> GetAsync<T>(string key, T defaultValue, IProgress<float> progress) {
            if (HasKey(key) == false) return defaultValue;
            if (CacheRecorder.HasKey(key)) return CacheRecorder.Get(key, defaultValue);

            var loadedValue = await MainRecorder.GetAsync(key, defaultValue, progress);
            CacheRecorder.Set(key, loadedValue);
            
            return loadedValue;
        }

        public bool Set<T>(string key, T value) {
            return MainRecorder.Set(key, value) && CacheRecorder.Set(key, value);
        }

        public bool HasKey(string key) {
            return CacheRecorder.HasKey(key) || MainRecorder.HasKey(key);
        }

        public bool Delete(string key) {
            if (HasKey(key) == false) return false;
            CacheRecorder.Delete(key);
            MainRecorder.Delete(key);
            return true;
        }

        public void DeleteAll() {
            CacheRecorder.DeleteAll();
            MainRecorder.DeleteAll();
        }

        public void SaveAll() {
            MainRecorder.SaveAll();
        }
    }
}