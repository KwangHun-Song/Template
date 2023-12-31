using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Diagnostics;

namespace Utility {
    /// <summary>
    /// 원하는 동작들을 순서대로 실행할 수 있는 연출 큐.
    /// 인스턴스를 만들어서 Enqueue 함수의 파라미터로 넣으면 자동으로 큐에 삽입되고, 큐는 순서대로 실행된다.
    /// Cancel() 함수를 부르면 현재 태스크에 캔슬토큰에 캔슬을 전달한 후 큐를 멈춘다.
    /// </summary>
    public class TaskQueue {
        public enum Result { None, Success, Stop, Canceled, Error }
        
        public bool OnQueueing => TokenSource != null;
        private CancellationTokenSource TokenSource { get; set; }

        private readonly Queue<object> queue = new Queue<object>();

        public void Enqueue(Func<CancellationToken, UniTask<Result>> task) {
            queue.Enqueue(task);
            TryTriggerQueueAsync().Forget();
        }
        
        public void Enqueue(Func<CancellationToken, UniTask> task) {
            queue.Enqueue(task);
            TryTriggerQueueAsync().Forget();
        }
        
        public void Enqueue(Func<UniTask> task) {
            queue.Enqueue(task);
            TryTriggerQueueAsync().Forget();
        }
        
        public void Enqueue(Action task) {
            queue.Enqueue(task);
            TryTriggerQueueAsync().Forget();
        }

        public void Cancel() {
            if (OnQueueing == false) return;

            TokenSource?.Cancel();
        }

        private async UniTask TryTriggerQueueAsync() {
            if (OnQueueing) return;
            TokenSource = new CancellationTokenSource();

            while (queue.Any()) {
                try {
                    var result = Result.Success;
                    
                    switch (queue.Dequeue()) {
                        case Func<CancellationToken, UniTask<Result>> taskWithResultAndToken:
                            result = await taskWithResultAndToken.Invoke(TokenSource.Token);
                            break;
                        case Func<CancellationToken, UniTask> taskWithToken:
                            await taskWithToken.Invoke(TokenSource.Token);
                            break;
                        case Func<UniTask> normalTask:
                            await normalTask.Invoke();
                            break;
                        case Action action:
                            action.Invoke();
                            break;
                        default:
                            result = Result.Error; // 이 분기를 탈 가능성이 없습니다.
                            break;
                    }
                    
                    if (TokenSource.IsCancellationRequested) break;
                    if (result is Result.Stop or Result.Canceled or Result.Error) break;
                    
                } catch (OperationCanceledException) {
                    break;
                } catch (Exception e) {
                    Debugger.Assert($"in TaskQueue, {e.ToString()}");
                    break;
                }
            }

            TokenSource = null;
        }
    }
}





