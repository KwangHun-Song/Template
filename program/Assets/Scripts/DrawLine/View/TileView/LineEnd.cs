using UnityEngine;
using UnityEngine.UI;

namespace DrawLine {
    public class LineEnd : MonoBehaviour {
        [SerializeField] private Image point;
        [SerializeField] private Image arm;
        
        public void SetColor(ColorIndex colorIndex) {
            point.color = colorIndex.GetUnityColor();
            arm.color = colorIndex.GetUnityColor();
        }

        public void SetDirectionFromThis(Direction direction) {
            switch (direction) {
                case Direction.Right:
                    transform.eulerAngles = Vector3.zero;
                    break;
                case Direction.Down:
                    transform.eulerAngles = Vector3.forward * -90;
                    break;
                case Direction.Left:
                    transform.eulerAngles = Vector3.forward * -180;
                    break;
                case Direction.Up:
                    transform.eulerAngles = Vector3.forward * -270;
                    break;
            }
        }

        public void SetDirectionToThis(Direction direction) {
            switch (direction) {
                case Direction.Right:
                    transform.eulerAngles = Vector3.forward * -180;
                    break;
                case Direction.Down:
                    transform.eulerAngles = Vector3.forward * -270;
                    break;
                case Direction.Left:
                    transform.eulerAngles = Vector3.zero;
                    break;
                case Direction.Up:
                    transform.eulerAngles = Vector3.forward * -90;
                    break;
            }
        }
    }
}