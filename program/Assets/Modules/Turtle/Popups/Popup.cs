using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Turtle {
    public abstract class Popup : MonoBehaviour, IPopup ,IAsyncInitializer {
        protected const int PopupSortingOrderInterval = 1000;
        
        protected UniTaskCompletionSource<object> PopupCompletionSource { get; set; }
        
        private Canvas canvas;
        protected Canvas Canvas => canvas ??= GetComponentInChildren<Canvas>(true); 
        
        public virtual void OnBeforeEnter<TParam>(TParam param) { }
        public virtual UniTask OnAfterEnterAsync<TEnterParam>(TEnterParam param) => UniTask.CompletedTask;
        public virtual void OnAfterLeave() { }

        public virtual void Open(int popupOpenCount) {
            // 팝업의 종료를 알리고 파라미터를 전해야 하므로 필수
            PopupCompletionSource = new UniTaskCompletionSource<object>();
            
            // 팝업이 겹쳐질 때마다 소팅오더가 더해져서 나중에 켜진 팝업이 항상 위에 뜬다.
            Canvas.sortingOrder = PopupSortingOrderInterval * popupOpenCount;
            
            // TODO : 커튼 띄우기
        }

        public virtual void Close() => Close(null);
        
        public virtual void Close(object param) {
            // TODO : 커튼 숨기기

            PopupCompletionSource.TrySetResult(param);
            PopupCompletionSource = null;
        }

        public virtual void Show() => GetGameObject().SetActive(true);
        public virtual void Hide() => GetGameObject().SetActive(false);

        public virtual GameObject GetGameObject() => gameObject;
        public virtual ITransition GetCustomTransition() => null;

        public virtual UniTaskCompletionSource<object> GetPopupCompletionSource() => PopupCompletionSource;

        #region IAsyncInitializer

        public virtual UniTask OnEnterAsync() => UniTask.CompletedTask;

        #endregion
    }
}