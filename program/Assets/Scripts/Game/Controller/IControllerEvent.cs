using System.Collections.Generic;

namespace DrawLine {
    public interface IControllerEvent {
        void OnStartGame(Controller controller);
        void OnClearGame();
        void OnStopGame();
    }
}