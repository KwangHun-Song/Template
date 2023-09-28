using UnityEngine.EventSystems;

namespace System.ScreenLock {
    /// <summary>
    /// 이벤트 시스템의 상호작용을 일시 중지하고 재개하는 클래스입니다.
    /// 클래스의 수명 주기 동안 EventSystem의 상호작용을 비활성화합니다.
    /// using 구문을 통해 이 클래스를 사용하면 해당 범위 내에서만 EventSystem이 비활성화됩니다.
    /// </summary>
    /// <example>
    /// 사용 예:
    /// <code>
    /// using(new ScreenLock()) {
    ///     // 여기서 EventSystem은 비활성화 상태입니다.
    ///     // ...
    /// } // using 블록을 벗어나면 EventSystem은 다시 활성화됩니다.
    /// </code>
    /// </example>
    /// <remarks>
    /// EventSystem이 이미 비활성화 상태인 경우 이 클래스를 사용해도 추가적인 영향은 없습니다.
    /// </remarks>
    public class ScreenLock : IDisposable {
        public ScreenLock() => Lock();
        public void Dispose() => UnLock();

        private void Lock() {
            if (EventSystem.current) {
                EventSystem.current.gameObject.SetActive(false);
            }
        }

        private void UnLock() {
            if (EventSystem.current) {
                EventSystem.current.gameObject.SetActive(true);
            }
        }
    }
}