using Core;
using UnityEngine;
using Utility;

namespace Diagnostics {
    public enum LogType {
        None,
        Debug,
        Info,
        Warning,
        Error
    }

    public enum LogColor {
        None,
        White,
        Cyan,
        Lime,
        Yellow,
        Orange,
        Purple,
        Red
    }
    
    public class LogManager : ILogManager {
        public LogType DefaultLogType { get; set; } = LogType.Info;
        public LogColor DefaultLogColor { get; set; } = LogColor.White;
        
        public void Log(object message, LogType logType = LogType.None, LogColor logColor = LogColor.None) {
            var messageStr = $"{message}";
            
            // 값이 정해지지 않고 들어왔다면 기본값을 지정해준다.
            if (logType == LogType.None) {
                logType = DefaultLogType;
            }

            if (logColor == LogColor.None) {
                logColor = DefaultLogColor;
            }
            
            if (Profile.EnableCheat() || Define.IsEditor) {
                messageStr = $"<color=#{GetHtmlColor(logColor)}>{messageStr}</color>";
            }

            switch (logType) {
                case LogType.Debug:
                    if (Profile.EnableDetailLog()) Debug.Log(messageStr);
                    break;
                case LogType.Info:
                    Debug.Log(messageStr);
                    break;
                case LogType.Warning:
                    Debug.LogWarning(messageStr);
                    break;
                case LogType.Error:
                    Debug.LogError(messageStr);
                    break;
            }
        }

        private string GetHtmlColor(LogColor logColor) {
            return logColor switch {
                LogColor.White => "FFFFFF",
                LogColor.Cyan => "00FFFF",
                LogColor.Lime => "00FF22",
                LogColor.Yellow => "FFFF00",
                LogColor.Orange => "FAD656",
                LogColor.Purple => "C99BFF",
                LogColor.Red => "D77265",
                _ => "FFFFFF"
            };
        }
    }
}