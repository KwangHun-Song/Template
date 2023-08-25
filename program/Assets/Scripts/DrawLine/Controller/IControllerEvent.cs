using System.Collections.Generic;

namespace DrawLine {
    public interface IControllerEvent {
        void OnStartGame(Controller controller);
        void OnClearGame();
        void OnStopGame();

        void OnDrawTile(Tile tile, ColorIndex color);
        void OnEraseTile(Tile tile);
        void OnClearColor(List<Tile> tilesInLine);
    }
}