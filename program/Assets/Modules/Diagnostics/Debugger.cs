using System;

namespace Diagnostics {
    public static class Debugger {
        // LogManager 인스턴스를 저장하는 속성
        private static ILogManager logManager;
        public static ILogManager LogManager => logManager ??= new LogManager();

        // LogManager을 외부에서 주입하는 초기화 메서드
        public static void Initialize(ILogManager customLogManager) {
            logManager = customLogManager;
        }

        // 기본적인 로그 메서드들을 구현
        public static void Log(object message, LogType logType = LogType.Info, LogColor logColor = LogColor.White) {
            LogManager.Log(message, logType, logColor);
        }

        public static void Debug(object message, LogColor logColor = LogColor.White) {
            LogManager.Log(message, LogType.Debug, logColor);
        }

        public static void Info(object message, LogColor logColor = LogColor.White) {
            LogManager.Log(message, LogType.Info, logColor);
        }

        public static void Warning(object message, LogColor logColor = LogColor.White) {
            LogManager.Log(message, LogType.Warning, logColor);
        }

        public static void Error(object message, LogColor logColor = LogColor.White) {
            LogManager.Log(message, LogType.Error, logColor);
        }

        // 조건을 만족하지 않을 때 에러를 발생시키는 Assert 메서드
        public static void Assert(Func<bool> condition, object message) {
            if (!condition.Invoke()) {
                Error($"Assertion failed: {message}", LogColor.Red);
            }
        }
        
        public static void Assert(bool condition, object message) {
            if (!condition) {
                Error($"Assertion failed: {message}", LogColor.Red);
            }
        }

        public static void Assert(object message) => Assert(false, message);
    }
}