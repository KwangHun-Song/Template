using System;

namespace DrawLine {
    public static class TileUtility {
        public static Direction GetDirection(this Tile from, Tile to, int width) {
            // Calculate the row and column for both tiles
            int fromRow = from.Index / width;
            int fromCol = from.Index % width;

            int toRow = to.Index / width;
            int toCol = to.Index % width;

            // Check the direction
            if (fromRow == toRow) {
                if (fromCol + 1 == toCol)
                    return Direction.Right;
                if (fromCol - 1 == toCol)
                    return Direction.Left;
            } else if (fromCol == toCol) {
                if (fromRow + 1 == toRow)
                    return Direction.Up; // As per the given description, Up is increasing in row value
                if (fromRow - 1 == toRow)
                    return Direction.Down; // Down is decreasing in row value
            }

            return Direction.None; // If the tiles aren't adjacent or in the same direction
        }

        public static Direction Opposite(this Direction direction) {
            return direction switch {
                Direction.None => Direction.None,
                Direction.Right => Direction.Left,
                Direction.Down => Direction.Up,
                Direction.Left => Direction.Right,
                Direction.Up => Direction.Down,
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            };
        }
    }
}