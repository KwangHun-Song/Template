using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;

namespace DrawLine {
    public class Controller {
        public Controller() { }
        
        public Controller(params IControllerEvent[] listeners) {
            Listeners = listeners.ToList();
        }
        
        private UniTaskCompletionSource<GameResult> gameCompletionSource;

        public Level CurrentLevel { get; private set; }
        public Tile[] Tiles { get; private set; }

        public IReadOnlyList<IControllerEvent> Listeners { get; } = new List<IControllerEvent>();

        #region InGame Property

        #endregion

        public bool OnGoingGame => gameCompletionSource?.Task.Status.IsCompleted() == false;

        public void StartGame(Level level) {
            // 이미 진행 중인 게임이 있는 경우, 해당 게임을 종료한다.
            if (OnGoingGame) StopGame();
            
            CurrentLevel = level;
            Tiles = level.tileModels.Select(tm => new Tile(tm)).ToArray();

            // 게임 시작
            gameCompletionSource = new UniTaskCompletionSource<GameResult>();
            
            // 이벤트 전달
            foreach (var listener in Listeners) listener.OnStartGame(this);
        }

        public void RestartGame() {
            StartGame(CurrentLevel);
        }

        public async UniTask<GameResult> WaitUntilGameEnd() {
            try {
                return await gameCompletionSource.Task;
            } catch (OperationCanceledException) {
                return GameResult.Stop;
            }
        }

        public void ClearGame() {
            gameCompletionSource.TrySetResult(GameResult.Clear);
            // 이벤트 전달
            foreach (var listener in Listeners) listener.OnClearGame();
        }

        public void StopGame() {
            gameCompletionSource.TrySetCanceled();
            // 이벤트 전달
            foreach (var listener in Listeners) listener.OnStopGame();
        }

        public Tile GetTile(int index) => Tiles.SingleOrDefault(t => t.Index == index);
        
        public void Input(InputType inputType, int index) => Input(inputType, GetTile(index));
        
        public void Input(InputType inputType, Tile tile) { }

        public bool IsCleared() {
            return false;
        }
    }
}