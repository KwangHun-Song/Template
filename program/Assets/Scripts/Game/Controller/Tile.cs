namespace DrawLine {
    public class Tile {
        public TileModel Model { get; }

        public int Index => Model.index;
        
        public Tile(TileModel model) {
            Model = model;
        }
    }
}