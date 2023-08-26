using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DrawLine {
    public class TileView : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerUpHandler {
        [SerializeField] private Image effectImage;
        [SerializeField] private Image point;
        [SerializeField] private LineEnd lineEnd;
        [SerializeField] private LinePass linePass;
        
        public GameView GameView { get; private set; }
        public Tile Tile { get; private set; }

        public void Initialize(Tile tile, GameView gameView) {
            Tile = tile;
            GameView = gameView;
            
            effectImage.color = new Color(1, 1, 1, 0);
            lineEnd.gameObject.SetActive(false);
            linePass.gameObject.SetActive(false);
            if (Tile.Type == TileType.Point) {
                point.gameObject.SetActive(true);
                point.color = Tile.Color.GetUnityColor();
            } else {
                point.gameObject.SetActive(false);
            }
            
            transform.localScale = Vector3.one;
        }

        public void DrawLine(ColorIndex colorIndex, Direction from, Direction to) {
            if (from == Direction.None && to == Direction.None) {
                Erase();
                return;
            }
            
            if (from == Direction.None) {
                linePass.gameObject.SetActive(false);
                lineEnd.gameObject.SetActive(true);
                lineEnd.SetColor(colorIndex);
                lineEnd.SetDirectionFromThis(to);
            } else if (to == Direction.None) {
                linePass.gameObject.SetActive(false);
                lineEnd.gameObject.SetActive(true);
                lineEnd.SetColor(colorIndex);
                lineEnd.SetDirectionFromThis(from);
            } else {
                lineEnd.gameObject.SetActive(false);
                linePass.gameObject.SetActive(true);
                linePass.SetColor(colorIndex);
                linePass.SetDirection(from, to);
            }
        }

        public void Erase() {
            linePass.gameObject.SetActive(false);
            lineEnd.gameObject.SetActive(false);
        }

        public async UniTask ShowClearEffectAsync(CancellationToken token) {
            await effectImage.DOColor(Color.white, 0.3F).ToUniTask(token);
            await effectImage.DOColor(new Color(1, 1, 1, 0), 0.3F).ToUniTask(token);
        }

        public void OnPointerDown(PointerEventData eventData) {
            GameView.Input(InputType.Down, Tile);
        }

        public void OnPointerEnter(PointerEventData eventData) {
            // 드래그 상태로 들어오면 이번 타일로 인풋을 추가한다.
            if (Input.GetMouseButton(0)) {
                GameView.Input(InputType.Down, Tile);
            }
        }

        public void OnPointerUp(PointerEventData eventData) {
            GameView.Input(InputType.Up, Tile);
        }
    }
}