using Cysharp.Threading.Tasks;
using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using DG.Tweening;
using DrawLine;
using UnityEngine.SceneManagement;

namespace PagePopupSystem {
    /// <summary>
    /// PopupManager는 게임 내의 팝업들을 관리하며, 팝업의 등장 및 퇴장 로직을 처리합니다.
    /// T는 팝업 유형을 나타내는 Enum입니다.
    /// </summary>
    public class PopupManager<T> : MonoBehaviour where T : Enum {
        private const int PopupSortingOrderGap = 1000;

        private static Dictionary<T, PopupHandler<T>> popups;
        private static Dictionary<T, PopupHandler<T>> Popups => popups ??= GetPopupsFromActiveScene();

        private static readonly Stack<PopupHandler<T>> popupStack = new Stack<PopupHandler<T>>();

        public static PopupHandler<T> CurrentPopup => popupStack.Any() == false ? null : popupStack.Peek();

        public static async UniTask<TResult> ShowAsync<TResult>(T popupType, object param = null, bool showImmediately = false) {
            if (Popups.TryGetValue(popupType, out var popup) == false) {
                Debug.LogWarning($"Popup with type {popupType} not found.");
                return default;
            }

            if (popupStack.Any()) {
                popup.SetSortingOrder(CurrentPopup.GetSortingOrder() + PopupSortingOrderGap);
            }

            popupStack.Push(popup);
            
            popup.gameObject.SetActive(true);
            popup.animationTarget.localScale = Vector3.zero;

            popup.OnWillEnter(param);
            
            if (showImmediately) {
                popup.animationTarget.transform.localScale = Vector3.one;
            } else {
                await popup.animationTarget.DOScale(Vector3.one, 0.3F).SetEase(Ease.OutBack).ToUniTask();
            }
            
            popup.OnDidEnterAsync(param).Forget();
            popup.popupTask = new UniTaskCompletionSource<object>();

            var result = await popup.popupTask.Task ?? UniTask.CompletedTask;
            
            popup.OnWillLeave();
            await popup.animationTarget.DOScale(Vector3.zero, 0.15F).SetEase(Ease.InBack).ToUniTask();
            popup.OnDidLeave();

            popup.gameObject.SetActive(false);

            if (popupStack.Count > 0 && popupStack.Peek() == popup) {
                popupStack.Pop();
            } else {
                Debug.LogWarning($"Unexpected popup state. {popupType} is not on top of the stack.");
            }

            if (result is TResult convertedResult) {
                return convertedResult;
            } else {
                return default;
            }
        }

        private static Dictionary<T, PopupHandler<T>> GetPopupsFromActiveScene() {
            return SceneManager.GetActiveScene()
                .GetRootGameObjects()
                .SelectMany(rootGo => rootGo.GetComponentsInChildren<PopupHandler<T>>(true))
                .ToDictionary(popup => (T)Enum.Parse(typeof(T), popup.GetPopupType().ToString()));
        }

        #region 신 변경 처리


        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        public static void InitializePages() {
            SceneManager.activeSceneChanged += OnActiveSceneChanged;
            Application.quitting += OnApplicationQuitting;
        }

        private static void OnActiveSceneChanged(Scene oldScene, Scene newScene) {
            popups = GetPopupsFromActiveScene();
            foreach (var popup in Popups.Values) {
                popup.gameObject.SetActive(false);
            }
        }

        private static void OnApplicationQuitting() {
            SceneManager.activeSceneChanged -= OnActiveSceneChanged;
            Application.quitting -= OnApplicationQuitting;
        }

        #endregion
    }
}