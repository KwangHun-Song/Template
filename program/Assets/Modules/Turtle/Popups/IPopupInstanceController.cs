using Cysharp.Threading.Tasks;

namespace Turtle {
    public interface IPopupInstanceController {
        public UniTask<IPopup> Get(string popupName);
        public void Abandon(IPopup popup);
    }
}