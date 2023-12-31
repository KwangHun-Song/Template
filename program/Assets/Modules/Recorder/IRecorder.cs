using System;
using Cysharp.Threading.Tasks;

namespace Recorder {
    public interface IRecorder {
        T Get<T>(string key, T defaultValue);
        
        UniTask Preload(string key, IProgress<float> progress);
        
        UniTask<T> GetAsync<T>(string key, T defaultValue, IProgress<float> progress);
        
        bool Set<T>(string key, T value);

        bool HasKey(string key);

        bool Delete(string key);

        void DeleteAll();

        void SaveAll();
    }
}