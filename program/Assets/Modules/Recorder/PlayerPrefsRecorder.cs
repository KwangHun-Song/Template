using System;
using Cysharp.Threading.Tasks;
using Diagnostics;
using Newtonsoft.Json;
using UnityEngine;
using Utility;

namespace Recorder {
    public class PlayerPrefsRecorder : IRecorder {
        public T Get<T>(string key, T defaultValue) {
            if (HasKey(key) == false) return defaultValue;

            if (defaultValue is bool) return (T)(object)(PlayerPrefs.GetInt(key, 0) == 1);
            if (defaultValue is int) return (T)(object)PlayerPrefs.GetInt(key, 0);
            if (defaultValue is float) return (T)(object)PlayerPrefs.GetFloat(key, 0);
            if (defaultValue is string) return (T)(object)PlayerPrefs.GetString(key, "");
            if (defaultValue is DateTime) return (T)(object)PlayerPrefs.GetString(key, "").ToExactTime();

            try {
                return JsonConvert.DeserializeObject<T>(PlayerPrefs.GetString(key, ""));
            } catch (JsonSerializationException) {
                Debugger.Assert($"PlayerPrefsRecorder.Get: Invalid type {typeof(T)}");
                return defaultValue;
            }
        }

        // 플레이어프랩스는 프리로드가 필요하지 않음
        public UniTask Preload(string key, IProgress<float> progress) => UniTask.CompletedTask;

        // 플레이어프랩스는 비동기 로드를 지원하지 않음
        public UniTask<T> GetAsync<T>(string key, T defaultValue, IProgress<float> progress) => UniTask.FromResult(Get(key, defaultValue));

        public bool Set<T>(string key, T value) {
            if (value is bool boolValue) PlayerPrefs.SetInt(key, boolValue ? 1 : 0);
            if (value is int intValue) PlayerPrefs.SetInt(key, intValue);
            if (value is float floatValue) PlayerPrefs.SetFloat(key, floatValue);
            if (value is string stringValue) PlayerPrefs.SetString(key, stringValue);
            if (value is DateTime dateTimeValue) PlayerPrefs.SetString(key, dateTimeValue.ToExactString());
            
            try {
                PlayerPrefs.SetString(key, JsonConvert.SerializeObject(value));
            } catch (JsonSerializationException) {
                Debugger.Assert($"PlayerPrefsRecorder.Set: Invalid type {typeof(T)}");
                return false;
            }

            return true;
        }

        public bool HasKey(string key) {
            return PlayerPrefs.HasKey(key);
        }

        public bool Delete(string key) {
            if (!HasKey(key)) return false;
            PlayerPrefs.DeleteKey(key);
            return true;
        }

        public void DeleteAll() {
            PlayerPrefs.DeleteAll();
        }

        public void SaveAll() {
            PlayerPrefs.Save();
        }
    }
}