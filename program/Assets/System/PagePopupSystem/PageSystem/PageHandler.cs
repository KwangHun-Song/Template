using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace PagePopupSystem {
    public abstract class PageHandler<T> : MonoBehaviour where T : Enum {

        public abstract T GetPageType();
        
        public virtual void OnWillEnter(object param) { }
        public virtual UniTask OnDidEnterAsync(object param) => UniTask.CompletedTask;
        public virtual void OnWillLeave() { }
        public virtual void OnDidLeave() { }

        public void ChangeTo(T pageType, object inParam = null) {
            PageManager<T>.ChangeTo(pageType, inParam).Forget();
        }
    }
}