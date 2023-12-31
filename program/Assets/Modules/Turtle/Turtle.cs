using Cysharp.Threading.Tasks;

namespace Turtle {
    public static class Turtle {
        private static IPageManager pageManager;
        public static IPageManager PageManager => pageManager ??= new PageManager();

        private static IPopupManager popupManager;
        public static IPopupManager PopupManager => popupManager ??= new PopupManager();

        /// <summary>
        /// 원하는대로 구현한 페이지매니저와 팝업매니저를 등록할 수 있다. 등록하지 않으면 디폴트가 등록된다.
        /// </summary>
        public static void Initialize(IPageManager customPageManager, IPopupManager customPopupManager) {
            pageManager = customPageManager;
            popupManager = customPopupManager;
        }

        public static UniTask ChangePageAsync<TParam>(string pageName, TParam param)
            => PageManager.ChangePageAsync(pageName, param);

        public static UniTask CloseCurrentPage()
            => PageManager.CloseCurrentPage();

        public static UniTask<TLeaveParam> ShowPopupAsync<TEnterParam, TLeaveParam>(string popupName, TEnterParam enterParam)
            => PopupManager.ShowPopupAsync<TEnterParam, TLeaveParam>(popupName, enterParam);

        public static void CloseCurrentPopup()
            => PopupManager.CloseCurrentPopup();
        
        public static UniTask ShowPopupAsync(string popupName) 
            => ShowPopupAsync<bool, bool>(popupName, false);

        public static UniTask ShowPopupAsync<TEnterParam>(string popupName, TEnterParam enterParam)
            => ShowPopupAsync<TEnterParam, bool>(popupName, enterParam);
        
        public static UniTask<TLeaveParam> ShowPopupAsync<TLeaveParam>(string popupName)
            => ShowPopupAsync<bool, TLeaveParam>(popupName, false);
    }
}