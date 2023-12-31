using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;

namespace Turtle {
    /// <summary>
    /// 팝업은 페이지 위에 뜨는 오브젝트로 겹칠 수 있으며, 스택 형식으로 늦게 연 팝업이 먼저 닫힌다. 
    /// </summary>
    public class PopupManager : IPopupManager {
        [CanBeNull] public IPopup CurrentPopup => PopupStack.Peek();
        [CanBeNull] public ITransition GlobalTransition { get; set; }
        [NotNull] public Stack<IPopup> PopupStack { get; } = new Stack<IPopup>();
        [NotNull] public IPopupInstanceController InstanceController { get; private set; }

        public async UniTask<TLeaveParam> ShowPopupAsync<TEnterParam, TLeaveParam>(string popupName, TEnterParam enterParam) {
            var popup = await InstanceController.Get(popupName);
            
            PopupStack.Push(CurrentPopup);
            
            popup.Open(PopupStack.Count);
            popup.Show();
            popup.OnBeforeEnter(enterParam);

            var onEnterAsync = popup is IAsyncInitializer ai ? ai.OnEnterAsync() : UniTask.CompletedTask;
            await UniTask.WhenAll(GetTransition(popup).OnTransitionEnter(), onEnterAsync);
            
            await popup.OnAfterEnterAsync(enterParam);

            var result = await popup.GetPopupCompletionSource().Task;
            var leaveParam = result is TLeaveParam casted ? casted : default;

            await GetTransition(popup).OnTransitionOut();
            popup.Hide();
            popup.OnAfterLeave();

            PopupStack.Pop();
            InstanceController.Abandon(popup);

            return leaveParam;
        }

        public void CloseCurrentPopup() {
            CurrentPopup?.Close(null);
        }

        private ITransition GetTransition(IPopup popup) {
            return popup.GetCustomTransition() ?? GlobalTransition ?? NoTransition.Instance;
        }
    }
}