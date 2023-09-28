using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DrawLine {
    public class TileView : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerUpHandler {
        public GameView GameView { get; private set; }
        public Tile Tile { get; private set; }

        public void Initialize(Tile tile, GameView gameView) { }

        public void OnPointerDown(PointerEventData eventData) { }

        public void OnPointerEnter(PointerEventData eventData) { }

        public void OnPointerUp(PointerEventData eventData) { }
    }
}