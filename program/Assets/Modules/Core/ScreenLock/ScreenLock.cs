using UnityEngine.EventSystems;
using System;
using System.Diagnostics;
using System.Text;
using Diagnostics;
using Utility;
using Debugger = Diagnostics.Debugger;

namespace Core.ScreenLock {
    public class ScreenLock : IDisposable {
        public ScreenLock() => Lock();
        public void Dispose() => UnLock();

        private static int lockCount;
        private static int LockCount {
            get => lockCount.ClampMin(0);
            set => lockCount = value.ClampMin(0);
        }

        public static bool IsLock() => LockCount > 0;

        public static void Lock() {
            if (EventSystem.current) {
                EventSystem.current.gameObject.SetActive(false);
            }

            if (Profile.EnableDetailLog()) {
                var stackTrace = new StackTrace();
                var builder = new StringBuilder();
                builder.AppendLine("@@ ScreenLock Called! @@");
                for (int frameCount = 1; frameCount < Math.Min(10, stackTrace.FrameCount); frameCount++) {
                    var frame = stackTrace.GetFrame(frameCount);
                    var methodBase = frame.GetMethod();
                    var classType = methodBase.DeclaringType?.Name ?? "Anonymous";
                    builder.AppendLine($" >> {classType}.{methodBase.Name}");
                }
                Debugger.Debug(builder.ToString(), LogColor.Purple);
            }

            LockCount++;
        }

        public static void UnLock() {
            LockCount--;
            
            if (lockCount <= 0 && EventSystem.current) {
                EventSystem.current.gameObject.SetActive(true);
            }
        }

        public static void UnlockForce() {
            while (IsLock() == false) {
                UnLock();
            }
        }
    }
}