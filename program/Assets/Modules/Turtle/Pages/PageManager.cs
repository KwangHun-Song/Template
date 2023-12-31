using Cysharp.Threading.Tasks;
using JetBrains.Annotations;

namespace Turtle {
    /// <summary>
    /// 페이지는 화면에 항상 최하단에 하나만 깔리는 전면 뷰를 가진 오브젝트이다.
    /// </summary>
    public class PageManager : IPageManager {
        [CanBeNull] public IPage CurrentPage { get; protected set; }
        [CanBeNull] public ITransition GlobalTransition { get; set; }

        private IPageInstanceController instanceController;
        [NotNull] public IPageInstanceController InstanceController 
            => instanceController ??= CreateDefaultInstanceController();
        
        

        public virtual async UniTask ChangePageAsync<TParam>(string pageName, TParam param) {
            await CloseCurrentPage();

            CurrentPage = await InstanceController.Get(pageName);

            await ShowPage(CurrentPage, param);
        }

        public virtual async UniTask CloseCurrentPage() {
            if (CurrentPage == null) return;
            
            await GetTransition(CurrentPage).OnTransitionOut();
            CurrentPage.Hide();
            CurrentPage.OnAfterLeave();
            
            InstanceController.Abandon(CurrentPage);
            CurrentPage = null;
        }

        protected virtual async UniTask ShowPage<TParam>(IPage page, TParam param) {
            if (page == null) return;
            
            page.Show();
            page.OnBeforeEnter(param);

            var onEnterAsync = page is IAsyncInitializer ai ? ai.OnEnterAsync() : UniTask.CompletedTask;
            await UniTask.WhenAll(GetTransition(page).OnTransitionEnter(), onEnterAsync);
            
            await page.OnAfterEnterAsync(param);
        }

        protected ITransition GetTransition(IPage page) {
            return page.GetCustomTransition() ?? GlobalTransition ?? NoTransition.Instance;
        }

        private IPageInstanceController CreateDefaultInstanceController() {
            throw new System.NotImplementedException();
        }
    }
}