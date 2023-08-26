using System.Collections.Generic;
using DrawLine;
using UnityEngine;

namespace ProjectDrawLine {
    public class PlayPage : MonoBehaviour {
        [SerializeField] private GameView gameView;
        
        public async void OnClickPlay(int levelIndex) {
            var level = GetLevel(levelIndex);
            if (level == null) {
                Debug.Log("레벨이 없습니다.");
                return;
            }
            var controller = new Controller(gameView);
            
            controller.StartGame(level);
            var result = await controller.WaitUntilGameEnd();
            Debug.Log(result);
        }

        private Level GetLevel(int levelIndex) {
            if (levelIndex == 0) {
                return new Level {
                    width = 4,
                    tileModels = new List<TileModel> {
                        new TileModel { index = 0, answerColor = ColorIndex.None, tileType = TileType.Normal },
                        new TileModel { index = 1, answerColor = ColorIndex.None, tileType = TileType.Normal },
                        new TileModel { index = 2, answerColor = ColorIndex.None, tileType = TileType.Normal },
                        new TileModel { index = 3, answerColor = ColorIndex.Yellow, tileType = TileType.Point },
                        new TileModel { index = 4, answerColor = ColorIndex.None, tileType = TileType.Normal },
                        new TileModel { index = 5, answerColor = ColorIndex.Blue, tileType = TileType.Point },
                        new TileModel { index = 6, answerColor = ColorIndex.Blue, tileType = TileType.Point },
                        new TileModel { index = 7, answerColor = ColorIndex.Red, tileType = TileType.Point },
                        new TileModel { index = 8, answerColor = ColorIndex.None, tileType = TileType.Normal },
                        new TileModel { index = 9, answerColor = ColorIndex.None, tileType = TileType.Normal },
                        new TileModel { index = 10, answerColor = ColorIndex.Yellow, tileType = TileType.Point },
                        new TileModel { index = 11, answerColor = ColorIndex.None, tileType = TileType.Normal },
                        new TileModel { index = 12, answerColor = ColorIndex.Red, tileType = TileType.Point },
                        new TileModel { index = 13, answerColor = ColorIndex.None, tileType = TileType.Normal },
                        new TileModel { index = 14, answerColor = ColorIndex.None, tileType = TileType.Normal },
                        new TileModel { index = 15, answerColor = ColorIndex.None, tileType = TileType.Normal },
                    }
                };
            }
            
            if (levelIndex == 1) {
                return new Level {
                    width = 4,
                    tileModels = new List<TileModel> {
                        new TileModel { index = 0, answerColor = ColorIndex.None, tileType = TileType.Normal },
                        new TileModel { index = 1, answerColor = ColorIndex.None, tileType = TileType.Normal },
                        new TileModel { index = 2, answerColor = ColorIndex.None, tileType = TileType.Normal },
                        new TileModel { index = 3, answerColor = ColorIndex.None, tileType = TileType.Normal },
                        new TileModel { index = 4, answerColor = ColorIndex.None, tileType = TileType.Normal },
                        new TileModel { index = 5, answerColor = ColorIndex.Navy, tileType = TileType.Point },
                        new TileModel { index = 6, answerColor = ColorIndex.Brown, tileType = TileType.Point },
                        new TileModel { index = 7, answerColor = ColorIndex.None, tileType = TileType.Normal },
                        new TileModel { index = 8, answerColor = ColorIndex.None, tileType = TileType.Normal },
                        new TileModel { index = 9, answerColor = ColorIndex.Navy, tileType = TileType.Point },
                        new TileModel { index = 10, answerColor = ColorIndex.None, tileType = TileType.Normal },
                        new TileModel { index = 11, answerColor = ColorIndex.None, tileType = TileType.Normal },
                        new TileModel { index = 12, answerColor = ColorIndex.Purple, tileType = TileType.Point },
                        new TileModel { index = 13, answerColor = ColorIndex.Brown, tileType = TileType.Point },
                        new TileModel { index = 14, answerColor = ColorIndex.None, tileType = TileType.Normal },
                        new TileModel { index = 15, answerColor = ColorIndex.Purple, tileType = TileType.Point },
                    }
                };
            }

            return null;
        }
    }
}