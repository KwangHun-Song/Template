using System;
using UnityEngine;
using UnityEngine.UI;

namespace DrawLine {
    public class LinePass : MonoBehaviour {
        [SerializeField] private Image joint;
        [SerializeField] private Image armFrom;
        [SerializeField] private Image armTo;
        
        public void SetColor(ColorIndex colorIndex) {
            joint.color = colorIndex.GetUnityColor();
            armFrom.color = colorIndex.GetUnityColor();
            armTo.color = colorIndex.GetUnityColor();
        }

        public void SetDirection(Direction from, Direction to) {
            switch (from) {
                case Direction.Right:
                    armFrom.transform.eulerAngles = Vector3.zero;
                    break;
                case Direction.Down:
                    armFrom.transform.eulerAngles = Vector3.forward * -90;
                    break;
                case Direction.Left:
                    armFrom.transform.eulerAngles = Vector3.forward * -180;
                    break;
                case Direction.Up:
                    armFrom.transform.eulerAngles = Vector3.forward * -270;
                    break;
            }
            
            switch (to) {
                case Direction.Right:
                    armTo.transform.eulerAngles = Vector3.zero;
                    break;
                case Direction.Down:
                    armTo.transform.eulerAngles = Vector3.forward * -90;
                    break;
                case Direction.Left:
                    armTo.transform.eulerAngles = Vector3.forward * -180;
                    break;
                case Direction.Up:
                    armTo.transform.eulerAngles = Vector3.forward * -270;
                    break;
            }
        }
    }
}