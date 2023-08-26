using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DrawLine {
    public class GameView : MonoBehaviour, IControllerEvent {
        [SerializeField] private Transform tilesRoot;
        [SerializeField] private GridLayoutGroup tileGrid;
        [SerializeField] private TMP_Text statusText;

        private TileView tileViewMold;
        private TileView TileViewMold => tileViewMold ??= Resources.Load<TileView>(nameof(TileView));
        
        private Controller Controller { get; set; }
        private AnimationQueue AnimationQueue { get; } = new AnimationQueue();
        private List<TileView> TileViews { get; } = new List<TileView>();


        #region IControllerEvent

        public void OnStartGame(Controller controller) {
            Controller = controller;
            statusText.gameObject.SetActive(false);
            
            // 기존 타일뷰들을 모두 지운다.
            foreach (var tileView in TileViews) {
                DestroyImmediate(tileView.gameObject);
            }
            TileViews.Clear();
            
            // 타일뷰를 새로 초기화해서 지정한다.
            foreach (var tile in Controller.Tiles) {
                var tileView = Instantiate(TileViewMold, tilesRoot);
                tileView.Initialize(tile, this);
                TileViews.Add(tileView);
            }
            
            // 그리드의 Width를 적용한다.
            tileGrid.constraintCount = Controller.CurrentLevel.width;
        }

        public void OnClearGame() {
            statusText.gameObject.SetActive(true);
            statusText.text = "Clear!";
        }

        public void OnStopGame() {
            AnimationQueue.Stop();
            statusText.gameObject.SetActive(true);
            statusText.text = "Stopped!";
        }

        public void OnDrawTile(Controller controller, Tile tile, ColorIndex color) {
            var tilesInLine = controller.DrawnLines[color];
            var prevTile = tilesInLine.PrevOrDefault(tile);
            var fromDirection = prevTile == null ? Direction.None : controller.GetDirection(tile, prevTile);
            GetTileView(tile).DrawLine(color, fromDirection, Direction.None);

            // 이전 타일도 다시 그려준다.(연결되도록)
            if (prevTile != null) {
                var thePrevTile = tilesInLine.PrevOrDefault(prevTile);
                var theFromDirection = thePrevTile == null ? Direction.None : controller.GetDirection(prevTile, thePrevTile);
                var toDirection = fromDirection.Opposite();
                
                GetTileView(prevTile).DrawLine(color, theFromDirection, toDirection);
            }
        }

        public void OnEraseTile(Controller controller, Tile tile, ColorIndex originColor) {
            GetTileView(tile).Erase();
            
            // 이전 타일이 있으면 끊어준다.
            var tilesInLine = controller.DrawnLines[originColor];
            var prevTile = tilesInLine.PrevOrDefault(tile);
            if (prevTile != null) {
                var thePrevTile = tilesInLine.PrevOrDefault(prevTile);
                var theFromDirection = thePrevTile == null ? Direction.None : controller.GetDirection(prevTile, thePrevTile);
                
                GetTileView(prevTile).DrawLine(originColor, theFromDirection, Direction.None);
            }
        }

        public void OnClearColor(List<Tile> tilesInLine) {
            AnimationQueue.Enqueue(ShowClearEffectAsync);
            
            async UniTask ShowClearEffectAsync(CancellationToken token) {
                var targetList = tilesInLine.ToList();
                foreach (var tile in targetList) {
                    GetTileView(tile).ShowClearEffectAsync(token).Forget();
                    await UniTask.Delay(100, cancellationToken: token);
                }
            }
        }

        #endregion

        public void Input(InputType inputType, Tile tile) {
            Controller.Input(inputType, tile);
        }

        private TileView GetTileView(Tile tile) => TileViews.SingleOrDefault(t => t.Tile == tile);
    }
}