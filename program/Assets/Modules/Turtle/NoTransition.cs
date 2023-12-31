using Cysharp.Threading.Tasks;

namespace Turtle {
    public class NoTransition : ITransition {
        private static NoTransition instance;
        public static NoTransition Instance => instance ??= new NoTransition();
        
        public UniTask OnTransitionEnter() => UniTask.CompletedTask;
        public UniTask OnTransitionOut() => UniTask.CompletedTask;
    }
}