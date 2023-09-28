using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace DrawLine {
    public class AnimationQueue {
        public bool OnAnimation { get; private set; }
        
        private Queue<Func<CancellationToken, UniTask>> Queue { get; } = new Queue<Func<CancellationToken, UniTask>>();
        private CancellationTokenSource TokenSource { get; set; }

        public void Enqueue(Func<CancellationToken, UniTask> anim) {
            Queue.Enqueue(anim);
            TryTriggerAnimationAsync().Forget();
        }

        public void Stop() {
            TokenSource?.Cancel();
        }

        private async UniTask TryTriggerAnimationAsync() {
            if (OnAnimation) return;
            OnAnimation = true;
            TokenSource ??= new CancellationTokenSource();
            
            while (Queue.Any()) {
                if (TokenSource.IsCancellationRequested) break;
                await Queue.Dequeue().Invoke(TokenSource.Token);
            }

            TokenSource = null;
            OnAnimation = false;
        }
    }
}