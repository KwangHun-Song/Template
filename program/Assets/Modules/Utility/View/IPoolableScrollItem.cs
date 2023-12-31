using UnityEngine;

namespace Utility {
    public interface IPoolableScrollItem {
        GameObject GameObject { get; }
        
        void Initialize(int index);
    }
}