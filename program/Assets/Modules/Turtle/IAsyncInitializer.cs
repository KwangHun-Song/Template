using Cysharp.Threading.Tasks;

namespace Turtle {
    /// <summary>
    /// 이 인터페이스를 상속받으면 비동기 초기화가 트랜지션 애니메이션과 함께 실행된다.
    /// </summary>
    public interface IAsyncInitializer {
        UniTask OnEnterAsync();
    }
}