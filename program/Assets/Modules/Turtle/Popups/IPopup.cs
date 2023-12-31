using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Turtle {
    public interface IPopup {
        void OnBeforeEnter<TParam>(TParam param);
        UniTask OnAfterEnterAsync<TEnterParam>(TEnterParam param);
        void OnAfterLeave();

        void Open(int popupOpenCount);
        void Close(object param);

        void Show();
        void Hide();

        GameObject GetGameObject();
        ITransition GetCustomTransition();
        UniTaskCompletionSource<object> GetPopupCompletionSource();
    }
}