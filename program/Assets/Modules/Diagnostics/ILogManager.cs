namespace Diagnostics {
    public interface ILogManager {
        void Log(object message, LogType logType = LogType.None, LogColor logColor = LogColor.None);
    }
}