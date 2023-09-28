using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;

namespace DrawLine {
    public static class DotweenExtension {
        public static UniTask ToUniTask(this Tween tween, CancellationToken cancellationToken = default) {
            if (tween == null || tween.IsComplete() || cancellationToken.IsCancellationRequested)
                return UniTask.CompletedTask;

            var source = new UniTaskCompletionSource<bool>();

            tween.OnComplete(() => { source.TrySetResult(true); });
            cancellationToken.Register(() => {
                tween.Kill();
                source.TrySetCanceled();
            });

            return source.Task;
        }
    }
}