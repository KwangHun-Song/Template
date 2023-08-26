using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace DrawLine.Test.Editor {
    public class DrawLineTest {
        [Test]
        public async void TestDrawLine() {
            var controller = new Controller();
            var level = new Level {
                width = 3,
                height = 3,
                tileModels = new List<TileModel> {
                    new TileModel { index = 0, tileType = TileType.Point, answerColor = ColorIndex.Red },
                    new TileModel { index = 1, tileType = TileType.Normal, answerColor = ColorIndex.Yellow },
                    new TileModel { index = 2, tileType = TileType.Point, answerColor = ColorIndex.Yellow },
                    new TileModel { index = 3, tileType = TileType.Point, answerColor = ColorIndex.Red },
                    new TileModel { index = 4, tileType = TileType.Point, answerColor = ColorIndex.Yellow },
                    new TileModel { index = 5, tileType = TileType.Point, answerColor = ColorIndex.Blue },
                    new TileModel { index = 6, tileType = TileType.Point, answerColor = ColorIndex.Blue },
                    new TileModel { index = 7, tileType = TileType.Normal, answerColor = ColorIndex.Blue },
                    new TileModel { index = 8, tileType = TileType.Normal, answerColor = ColorIndex.Blue },
                }
            };

            controller.StartGame(level);

            // 빨간색 라인을 완성한다.
            controller.Input(InputType.Down, 0);
            controller.Input(InputType.Down, 3);
            controller.InputUp();

            Assert.IsTrue(controller.IsConnectedTwoPoints(ColorIndex.Red));

            // 다시 0번을 클릭하면 완성된 포인트는 취소되어있다.
            controller.Input(InputType.Down, 0);
            controller.InputUp();

            Assert.IsFalse(controller.IsConnectedTwoPoints(ColorIndex.Red));

            // 파란색 라인을 완성한다.
            controller.Input(InputType.Down, 6);
            controller.Input(InputType.Down, 7);
            controller.Input(InputType.Down, 8);
            controller.Input(InputType.Down, 5);
            controller.InputUp();

            Assert.IsTrue(controller.IsConnectedTwoPoints(ColorIndex.Blue));

            // 노란색으로 침범한다. 파란색은 끊겨 있다.
            controller.Input(InputType.Down, 4);
            controller.Input(InputType.Down, 7);
            controller.InputUp();

            Assert.IsFalse(controller.IsConnectedTwoPoints(ColorIndex.Blue));

            // 노란색으로 그리다가 1번에서 손을 뗀다.
            controller.Input(InputType.Down, 4);
            controller.Input(InputType.Down, 1);
            controller.InputUp();

            // 손을 뗀 곳에서 이어서 그릴 수 있다.
            controller.Input(InputType.Down, 1);
            controller.Input(InputType.Down, 2);

            Assert.IsTrue(controller.IsConnectedTwoPoints(ColorIndex.Yellow));

            // 이미 포인트로 이은 후에는 다른 곳으로 더 드래그할 수 없다.
            controller.Input(InputType.Down, 5);
            controller.InputUp();

            Assert.IsTrue(controller.IsConnectedTwoPoints(ColorIndex.Yellow));

            // 인접하지 않은 타일로 드래그할 수 없다.
            controller.Input(InputType.Down, 5);
            controller.Input(InputType.Down, 7);
            controller.InputUp();

            Assert.AreEqual(1, controller.DrawnLines[ColorIndex.Blue].Count);

            // 빨간색 라인을 완성한다.
            controller.Input(InputType.Down, 3);
            controller.Input(InputType.Down, 0);
            controller.InputUp();

            Assert.IsTrue(controller.IsConnectedTwoPoints(ColorIndex.Red));

            // 파란색 라인을 완성한다.
            controller.Input(InputType.Down, 5);
            controller.Input(InputType.Down, 8);
            controller.Input(InputType.Down, 7);
            controller.Input(InputType.Down, 6);
            controller.InputUp();

            Assert.IsTrue(controller.IsConnectedTwoPoints(ColorIndex.Blue));

            // 클리어되었을 것이다.
            Assert.IsTrue(controller.IsCleared());
            Assert.AreEqual(GameResult.Clear, await controller.WaitUntilGameEnd());
        }

        [Test]
        public void TestInitialGameState() {
            var controller = new Controller();
            var level = new Level {
                width = 3,
                height = 3,
                tileModels = new List<TileModel> {
                    new TileModel { index = 0, tileType = TileType.Point, answerColor = ColorIndex.Red },
                    new TileModel { index = 1, tileType = TileType.Normal, answerColor = ColorIndex.Yellow },
                    new TileModel { index = 2, tileType = TileType.Point, answerColor = ColorIndex.Yellow },
                    new TileModel { index = 3, tileType = TileType.Point, answerColor = ColorIndex.Red },
                    new TileModel { index = 4, tileType = TileType.Point, answerColor = ColorIndex.Yellow },
                    new TileModel { index = 5, tileType = TileType.Point, answerColor = ColorIndex.Blue },
                    new TileModel { index = 6, tileType = TileType.Point, answerColor = ColorIndex.Blue },
                    new TileModel { index = 7, tileType = TileType.Normal, answerColor = ColorIndex.Blue },
                    new TileModel { index = 8, tileType = TileType.Normal, answerColor = ColorIndex.Blue },
                }
            };

            controller.StartGame(level);

            // 게임 시작 시 모든 라인이 연결되지 않은 상태 확인
            Assert.IsFalse(controller.IsConnectedTwoPoints(ColorIndex.Red));
            Assert.IsFalse(controller.IsConnectedTwoPoints(ColorIndex.Blue));
            Assert.IsFalse(controller.IsConnectedTwoPoints(ColorIndex.Yellow));

            // DrawnLines의 개수가 0인지 확인
            Assert.AreEqual(0, controller.DrawnLines[ColorIndex.Red].Count);
            Assert.AreEqual(0, controller.DrawnLines[ColorIndex.Blue].Count);
            Assert.AreEqual(0, controller.DrawnLines[ColorIndex.Yellow].Count);
        }

        [Test]
        public void TestGameRestart() {
            var controller = new Controller();
            var level = new Level {
                width = 3,
                height = 3,
                tileModels = new List<TileModel> {
                    new TileModel { index = 0, tileType = TileType.Point, answerColor = ColorIndex.Red },
                    new TileModel { index = 1, tileType = TileType.Normal, answerColor = ColorIndex.Yellow },
                    new TileModel { index = 2, tileType = TileType.Point, answerColor = ColorIndex.Yellow },
                    new TileModel { index = 3, tileType = TileType.Point, answerColor = ColorIndex.Red },
                    new TileModel { index = 4, tileType = TileType.Point, answerColor = ColorIndex.Yellow },
                    new TileModel { index = 5, tileType = TileType.Point, answerColor = ColorIndex.Blue },
                    new TileModel { index = 6, tileType = TileType.Point, answerColor = ColorIndex.Blue },
                    new TileModel { index = 7, tileType = TileType.Normal, answerColor = ColorIndex.Blue },
                    new TileModel { index = 8, tileType = TileType.Normal, answerColor = ColorIndex.Blue },
                }
            };

            controller.StartGame(level);

            // 빨간색 라인을 일부 완성한다.
            controller.Input(InputType.Down, 0);
            controller.Input(InputType.Down, 3);
            controller.InputUp();

            // 여기서 게임을 재시작한다.
            controller.RestartGame();

            // 모든 연결 상태가 초기화되어야 한다.
            Assert.IsFalse(controller.IsConnectedTwoPoints(ColorIndex.Red));
            Assert.IsFalse(controller.IsConnectedTwoPoints(ColorIndex.Yellow));
            Assert.IsFalse(controller.IsConnectedTwoPoints(ColorIndex.Blue));

            // DrawnLines의 모든 요소도 초기화되어야 한다.
            foreach (ColorIndex color in Enum.GetValues(typeof(ColorIndex))) {
                if (controller.DrawnLines.ContainsKey(color) == false) continue;
                Assert.AreEqual(0, controller.DrawnLines[color].Count);
            }
        }
    }
}