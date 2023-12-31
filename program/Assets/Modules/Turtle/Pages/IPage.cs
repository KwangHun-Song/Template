using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Turtle {
    public interface IPage {
        void OnBeforeEnter<TEnterParam>(TEnterParam param);
        UniTask OnAfterEnterAsync<TEnterParam>(TEnterParam param);
        void OnAfterLeave();

        void Show();
        void Hide();

        GameObject GetGameObject();
        ITransition GetCustomTransition();
    }
}