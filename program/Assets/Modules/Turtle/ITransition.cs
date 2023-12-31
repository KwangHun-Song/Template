using Cysharp.Threading.Tasks;

namespace Turtle {
    public interface ITransition {
        UniTask OnTransitionEnter();
        UniTask OnTransitionOut();
    }
}