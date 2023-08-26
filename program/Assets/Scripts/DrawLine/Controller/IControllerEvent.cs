using System.Collections.Generic;

namespace DrawLine {
    public interface IControllerEvent {
        void OnStartGame(Controller controller);
        void OnClearGame();
        void OnStopGame();

        void OnDrawTile(Controller controller, Tile tile, ColorIndex color);
        void OnEraseTile(Controller controller, Tile tile, ColorIndex originColor);
        void OnClearColor(List<Tile> tilesInLine);
    }
}