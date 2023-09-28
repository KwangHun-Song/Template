using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace PagePopupSystem {
    public abstract class PopupHandler<T> : MonoBehaviour where T : Enum {
        [SerializeField] internal Transform animationTarget;

        internal UniTaskCompletionSource<object> popupTask;

        private Canvas canvas;
        private Canvas Canvas => canvas ??= GetComponentInChildren<Canvas>(true);

        public abstract T GetPopupType();

        public virtual void OnWillEnter(object param) { }
        public UniTask OnDidEnterAsync(object param) => UniTask.CompletedTask;
        public virtual void OnWillLeave() { }
        public virtual void OnDidLeave() { }

        public virtual void OnClickOk() => Close(true);

        public virtual void OnClickClose() => Close(false);

        public void Close() => Close(null);
        public void Close(object result) => popupTask.TrySetResult(result);

        internal void SetSortingOrder(int sortingOrder) => Canvas.sortingOrder = sortingOrder;
        internal int GetSortingOrder() => Canvas.sortingOrder;
    }
}