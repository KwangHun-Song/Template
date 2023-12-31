using Cysharp.Threading.Tasks;

namespace Turtle {
    public interface IPageManager {
        IPage CurrentPage { get; }
        ITransition GlobalTransition { get; set; }
        IPageInstanceController InstanceController { get; }
        
        UniTask ChangePageAsync<TParam>(string pageName, TParam param);
        UniTask CloseCurrentPage();
    }
}