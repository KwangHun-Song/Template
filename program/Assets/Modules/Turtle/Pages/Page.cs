using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Turtle {
    public abstract class Page : MonoBehaviour, IPage, IAsyncInitializer {
        public virtual void OnBeforeEnter<TEnterParam>(TEnterParam param) { }
        public virtual UniTask OnAfterEnterAsync<TEnterParam>(TEnterParam param) => UniTask.CompletedTask;
        public virtual void OnAfterLeave() { }

        public virtual void Show() => GetGameObject().SetActive(true);
        public virtual void Hide() => GetGameObject().SetActive(false);

        public virtual GameObject GetGameObject() => gameObject;

        public virtual ITransition GetCustomTransition() => null;

        #region IAsyncInitializer

        public UniTask OnEnterAsync() => UniTask.CompletedTask;

        #endregion
    }
}