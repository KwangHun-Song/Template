using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PagePopupSystem {
    /// <summary>
    /// PageManager는 게임의 다양한 페이지 상태를 관리하며, 페이지 전환 로직을 핸들링합니다.
    /// T는 페이지 유형을 나타내는 Enum입니다.
    /// </summary>
    public static class PageManager<T> where T : Enum {
        private static Dictionary<T, PageHandler<T>> pages;
        public static T CurrentPageType { get; private set; }
        public static bool OnTransitionAnimation { get; private set; }
        public static event Action<T> OnPageChanged;

        private static Dictionary<T, PageHandler<T>> Pages => pages ??= GetPagesFromActiveScene();

        public static void ChangeImmediately(T pageType, object param = null) {
            if (pages.TryGetValue(pageType, out var nextPage) == false) {
                throw new ArgumentException($"Page with type {pageType} not found.");
            }

            if (!Equals(CurrentPageType, default(T))) {
                var currentPage = Pages[CurrentPageType];
                currentPage.OnWillLeave();
                currentPage.OnDidLeave();
                currentPage.gameObject.SetActive(false);
            }

            nextPage.gameObject.SetActive(true);
            nextPage.OnWillEnter(param);
            nextPage.OnDidEnterAsync(pageType).Forget();
            CurrentPageType = pageType;
        }

        public static async UniTask ChangeTo(T pageType, object param = null) {
            if (Equals(pageType, default(T))) return;
            if (pages.TryGetValue(pageType, out var nextPage) == false) {
                throw new ArgumentException($"Page with type {pageType} not found.");
            }

            if (!Equals(CurrentPageType, default(T))) {
                var currentPage = Pages[CurrentPageType];
                currentPage.OnWillLeave();
                OnTransitionAnimation = true;
                await FadeOutHelper.FadeOut();
                currentPage.OnDidLeave();
                currentPage.gameObject.SetActive(false);
            }

            nextPage.gameObject.SetActive(true);
            nextPage.OnWillEnter(param);
            CurrentPageType = pageType;

            OnPageChanged?.Invoke(CurrentPageType);

            await FadeOutHelper.FadeIn();
            OnTransitionAnimation = false;
            await nextPage.OnDidEnterAsync(param);
        }

        public static void RemovePage(T pageType) => Pages.Remove(pageType);

        private static Dictionary<T, PageHandler<T>> GetPagesFromActiveScene() {
            return SceneManager.GetActiveScene()
                .GetRootGameObjects()
                .SelectMany(rootGo => rootGo.GetComponentsInChildren<PageHandler<T>>(true))
                .ToDictionary(page => (T)Enum.Parse(typeof(T), page.GetPageType().ToString()));
        }

        #region 신 변경 처리


        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        public static void InitializePages() {
            SceneManager.activeSceneChanged += OnActiveSceneChanged;
            Application.quitting += OnApplicationQuitting;
        }

        private static void OnActiveSceneChanged(Scene oldScene, Scene newScene) {
            pages = GetPagesFromActiveScene();
            
            foreach (var page in Pages.Values) {
                page.gameObject.SetActive(false);
            }
        }

        private static void OnApplicationQuitting() {
            SceneManager.activeSceneChanged -= OnActiveSceneChanged;
            Application.quitting -= OnApplicationQuitting;
        }

        #endregion
    }
}