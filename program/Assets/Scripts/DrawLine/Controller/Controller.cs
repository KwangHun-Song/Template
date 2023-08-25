using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Assertions;

namespace DrawLine {
    public enum GameResult { Clear, Stop }
    public enum InputType { Down, Up }
    public enum ColorIndex { None, Red, Orange, Yellow, Green, Blue, Navy, Purple, Brown, Pink, Cyan, SkyBlue }
    
    public class Controller {
        public Controller() { }
        
        public Controller(params IControllerEvent[] listeners) {
            Listeners = listeners.ToList();
        }
        
        private UniTaskCompletionSource<GameResult> gameCompletionSource;

        public Level CurrentLevel { get; private set; }
        public Tile[] Tiles { get; private set; }
        public IEnumerable<Tile> TouchableTiles => Tiles.Where(t => t.Type != TileType.Invisible);

        public IReadOnlyList<IControllerEvent> Listeners { get; } = new List<IControllerEvent>();

        #region InGame Property

        [CanBeNull] public Tile LastDownTile { get; private set; }
        
        private readonly Dictionary<ColorIndex, List<Tile>> drawnLines = new Dictionary<ColorIndex, List<Tile>>();
        
        private IReadOnlyDictionary<ColorIndex, ReadOnlyCollection<Tile>> cachedDrawnLines;
        public IReadOnlyDictionary<ColorIndex, ReadOnlyCollection<Tile>> DrawnLines =>
            cachedDrawnLines ??= drawnLines.ToDictionary(kvp => kvp.Key, kvp => new ReadOnlyCollection<Tile>(kvp.Value));
        
        #endregion

        public bool OnGoingGame => gameCompletionSource?.Task.Status.IsCompleted() == false;

        public void StartGame(Level level) {
            // 이미 진행 중인 게임이 있는 경우, 해당 게임을 종료한다.
            if (OnGoingGame) StopGame();
            
            CurrentLevel = level;
            Tiles = level.tileModels.Select(tm => new Tile(tm)).ToArray();
            LastDownTile = null;
            drawnLines.Clear();

            // 게임 시작
            gameCompletionSource = new UniTaskCompletionSource<GameResult>();
            
            // 이벤트 전달
            foreach (var listener in Listeners) listener.OnStartGame(this);
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

        private bool IsCleared() {
            if (TouchableTiles.Any(t => t.Color == ColorIndex.None)) return false;
            return DrawnLines.Select(kvp => kvp.Key).All(IsConnectedTwoPoints) == true;
        }

        public void Input(InputType inputType, Tile tile) {
            // 터치 불가능한 타일에 한 인풋은 무시한다.
            if (TouchableTiles.Contains(tile) == false) return;
            
            // 손가락을 뗀 인풋의 경우, 마지막 드래그중인 타일만 무시하고 추가 적용은 없다.
            if (inputType == InputType.Up) {
                LastDownTile = null;
                return;
            }

            // 마지막 드래그중인 타일이 없는 경우
            if (LastDownTile == null) {
                // 이 경우, 새로운 드래그 시작만 유효하다.
                if (tile.Type != TileType.Point) return;

                var color = tile.Color; // Point 타일이므로 레벨에서 지정한 색깔이 적용될 것이다.
                // 기존에 그리던 라인이 있던 경우 모두 지운다.
                if (drawnLines.TryGetValue(color, out var tilesInExistedLine)) {
                    foreach (var existTile in tilesInExistedLine) {
                        EraseTile(existTile);
                    }
                }

                DrawTile(tile, color);
                return;
            }
            
            // 마지막 드래그중인 타일이 있는 경우
            var onGoingColor = LastDownTile.Color;
            
            // 마지막 드래그중인 타일의 옆이 아닌 경우, 잘못된 인풋이므로 무시한다.
            if (AreTilesAdjacent(tile, LastDownTile) == false) return;
            
            // 빈칸으로 드래그한 경우 컬러를 그려준다.
            if (tile.Color == ColorIndex.None) {
                DrawTile(tile, onGoingColor);
                return;
            }
            
            // 다른 색깔로 드래그한 경우
            if (tile.Color != onGoingColor) {
                // 다른 색깔의 포인트로 드래그한 경우는 인풋을 무시한다.
                if (tile.Type == TileType.Point) return;
                
                // 포인트가 아닌 그려진 색깔인 경우, 기존에 그려진 라인을 이 타일까지 먼저 지운 후 컬러를 그려준다.
                if (drawnLines.TryGetValue(tile.Color, out var tilesInOtherColorLine)) {
                    foreach (var existTile in tilesInOtherColorLine) {
                        EraseTile(existTile);
                        if (existTile == tile) break; // 인풋한 타일까지만 지운다.
                    }
                }
                
                DrawTile(tile, onGoingColor);
                return;
            }
            
            // 같은 색깔로 드래그한 경우
            var tilesInSameColorLine = drawnLines[onGoingColor];
            // 이미 칠했던 타일에 드래그한 경우
            if (tilesInSameColorLine.Contains(tile)) {
                // 이 타일까지 먼저 지우고 진행한다
                foreach (var existTile in tilesInSameColorLine) {
                    EraseTile(existTile);
                    if (existTile == tile) break; // 인풋한 타일까지만 지운다.
                }
            }
            
            DrawTile(tile, onGoingColor);
            
            // 색깔을 포인트에서 포인트까지 완성한 경우
            if (tile.Type == TileType.Point && IsConnectedTwoPoints(onGoingColor)) {
                LastDownTile = null; // 마지막 드래그중인 타일을 제거한다.(여기서 이어서 드래그할 수 없다.)
                foreach (var listener in Listeners) listener.OnClearColor(tilesInSameColorLine);
            }
            
            // 클리어 체크
            if (IsCleared()) {
                gameCompletionSource.TrySetResult(GameResult.Clear);
                foreach (var listener in Listeners) listener.OnClearGame();
            }
        }

        private void DrawTile(Tile tile, ColorIndex drawColor) {
            tile.Color = drawColor;
            
            if (drawnLines.ContainsKey(drawColor) == false) {
                drawnLines[drawColor] = new List<Tile>();
            }

            if (IsValid() == false) {
                Assert.IsTrue(false, "기존에 있는 타일일 가능성이 있을 수 없음(항상 그 전에 지워져야 한다.)");
                return;
            } 
            
            drawnLines[drawColor].Add(tile);
            
            foreach (var listener in Listeners) listener.OnDrawTile(tile, drawColor);

            bool IsValid() => drawnLines[drawColor].Contains(tile) == false;
        }

        private void EraseTile(Tile tile) {
            var originColor = tile.Color;
            tile.Color = ColorIndex.None;

            if (IsValid() == false) {
                Assert.IsTrue(false, "지우려는 타일은 항상 그려진 타일이어야 함.");
                return;
            }

            drawnLines[originColor].Remove(tile);
            
            foreach (var listener in Listeners) listener.OnEraseTile(tile);

            bool IsValid() => drawnLines.ContainsKey(originColor)
                              && drawnLines[originColor] != null 
                              && drawnLines[originColor].Contains(tile);
        }

        private bool IsConnectedTwoPoints(ColorIndex color) {
            if (drawnLines.TryGetValue(color, out var tilesInLie) == false) return false;

            return tilesInLie.Count(t => t.Type == TileType.Point) == 2;
        }

        private (int x, int y) GetCo(Tile tile) {
            return (tile.Index % CurrentLevel.width, tile.Index / CurrentLevel.width);
        }

        private bool AreTilesAdjacent(Tile tileA, Tile tileB) {
            const int AdjacentDifference = 1;
            var aCoordi = GetCo(tileA);
            var bCoordi = GetCo(tileB);

            // 대각선은 체크하지 않고 오직 가로, 세로에 인접했는지만 체크한다.
            return Mathf.Abs(aCoordi.x - bCoordi.x) + Mathf.Abs(aCoordi.y - bCoordi.y) == AdjacentDifference;
        }
    }
}