using Core;

namespace Utility {
    public class Define {
        public const bool IsDeploy =
#if DEPLOY
            true;
#else
            false;
#endif
        
        public const bool IsStandalone =
#if UNITY_STANDALONE
            true;
#else
            false;
#endif

        public const bool IsStandaloneDevice =
#if !UNITY_EDITOR && UNITY_STANDALONE
            true;
#else
            false;
#endif

        public const bool IsEditor =
#if UNITY_EDITOR
            true;
#else
            false;
#endif

        public const bool IsAndroid =
#if UNITY_ANDROID
            true;
#else
            false;
#endif

        public const bool IsAndroidDevice =
#if !UNITY_EDITOR && UNITY_ANDROID
            true;
#else
            false;
#endif

        public const bool IsIOS =
#if UNITY_IOS
            true;
#else
            false;
#endif

        public const bool IsIOSDevice =
#if !UNITY_EDITOR && UNITY_IOS
            true;
#else
            false;
#endif

        public static bool IsDevelop() => IsEditor || Profile.EnableCheat() || !IsDeploy; 
    }
}