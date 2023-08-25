namespace DrawLine {
    public class Tile {
        public TileModel Model { get; }

        public int Index => Model.index;
        public TileType Type => Model.tileType;

        private ColorIndex drawnColor;
        /// <summary>
        /// 인풋으로 인해 그려진 컬러.
        /// </summary>
        public ColorIndex Color {
            get {
                if (Model.tileType == TileType.Point) return Model.answerColor;
                if (Model.tileType == TileType.Invisible) return ColorIndex.None;
                return drawnColor;
            }

            set => drawnColor = value;
        }
        
        public Tile(TileModel model) {
            Model = model;
            Color = ColorIndex.None;
        }
    }
}