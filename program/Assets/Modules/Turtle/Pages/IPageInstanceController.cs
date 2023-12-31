using Cysharp.Threading.Tasks;

namespace Turtle {
    /// <summary>
    /// 페이지의 생사여탈을 관장한다. 유니티신에 남겨두고 그냥 활성화/비활성화를 할 수도 있고 Instantiate/Destroy를 할 수도 있다.
    /// </summary>
    public interface IPageInstanceController {
        public UniTask<IPage> Get(string pageName);
        public void Abandon(IPage page);
    }
}