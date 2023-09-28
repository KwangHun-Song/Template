using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DrawLine {
    public class GameView : MonoBehaviour, IControllerEvent {
        public Controller Controller { get; private set; }
        
        public void Input(InputType inputType, Tile tile) {
            Controller.Input(inputType, tile);
        }

        #region IControllerEvent

        public void OnStartGame(Controller controller) { }

        public void OnClearGame() { }

        public void OnStopGame() { }

        #endregion
    }
}