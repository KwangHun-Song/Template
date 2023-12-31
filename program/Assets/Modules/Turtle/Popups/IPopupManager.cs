using Cysharp.Threading.Tasks;

namespace Turtle {
    public interface IPopupManager {
        IPopup CurrentPopup { get; }
        ITransition GlobalTransition { get; set; }
        IPopupInstanceController InstanceController { get; }

        UniTask<TLeaveParam> ShowPopupAsync<TEnterParam, TLeaveParam>(string popupName, TEnterParam enterParam);
        void CloseCurrentPopup();
    }
}