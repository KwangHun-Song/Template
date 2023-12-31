using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace Recorder {
    public class MemoryRecorder : IRecorder {
        private readonly ConcurrentDictionary<string, object> memory = new();

        public T Get<T>(string key, T defaultValue) {
            if (HasKey(key) == false) return defaultValue;

            if (memory[key] is T typedValue) {
                return typedValue;
            }

            return defaultValue;
        }

        public UniTask Preload(string key, IProgress<float> progress) => UniTask.CompletedTask;

        public UniTask<T> GetAsync<T>(string key, T defaultValue, IProgress<float> progress) => UniTask.FromResult(Get(key, defaultValue));

        public bool Set<T>(string key, T value) {
            memory[key] = value;

            return true;
        }

        public bool HasKey(string key) {
            return memory.ContainsKey(key);
        }

        public bool Delete(string key) {
            return memory.TryRemove(key, out _);
        }

        public void DeleteAll() {
            memory.Clear();
        }

        public void SaveAll() { }
    }
}