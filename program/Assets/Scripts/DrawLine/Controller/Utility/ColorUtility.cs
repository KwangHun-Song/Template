using System;
using UnityEngine;

namespace DrawLine {
    public static class ColorUtility {
        public static Color GetUnityColor(this ColorIndex colorIndex) {
            return colorIndex switch {
                ColorIndex.None => Color.clear,
                ColorIndex.Red => Color.red,
                ColorIndex.Orange => new Color(1, 0.5F, 0, 1),
                ColorIndex.Yellow => Color.yellow,
                ColorIndex.Green => Color.green,
                ColorIndex.Blue => Color.blue,
                ColorIndex.Navy => new Color(0, 0, 0.5F, 1),
                ColorIndex.Purple => Color.magenta,
                ColorIndex.Brown => new Color(0.5F, 0, 0, 1),
                ColorIndex.Pink => new Color(1, 0.5F, 0.5F, 1),
                ColorIndex.Cyan => Color.cyan,
                _ => throw new ArgumentOutOfRangeException(nameof(colorIndex), colorIndex, null)
            };
        }
    }
}