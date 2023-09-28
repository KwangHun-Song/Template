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
            return null;
        }
    }
}