using System.Collections.Generic;
using Pooling;
using UnityEngine;
using UnityEngine.UI;

namespace Utility {
    public class PoolableScrollRect : MonoBehaviour {
        [SerializeField] private GameObject itemPrefab;
        [SerializeField] private ScrollRect scrollRect;

        private readonly List<IPoolableScrollItem> visibleItems = new List<IPoolableScrollItem>();
        
        private RectTransform contentRectTransform;
        private RectTransform viewPortRectTransform;

        private float lastScrollPosition;
        
        private void Awake() {
            contentRectTransform = scrollRect.content;
            viewPortRectTransform = scrollRect.viewport;
        }

        private void Update() {
            UpdateVisibleItems();
        }
        
        private void UpdateVisibleItems() {
            var currentScrollPosition = scrollRect.verticalNormalizedPosition;
            if (Mathf.Approximately(currentScrollPosition, lastScrollPosition) == false) {
                UpdateVisibleItemsOnScroll();
                lastScrollPosition = currentScrollPosition;
            }
        }

        private void UpdateVisibleItemsOnScroll() {
            for (int i = visibleItems.Count - 1; i >= 0; i--) {
                var item = visibleItems[i];
                if (!IsItemVisible(item)) {
                    Pool.Release(item.GameObject);
                }
            }

            var startIndex = CalculateStartIndex();
            var endIndex = CalculateEndIndex();

            for (int i = startIndex; i <= endIndex; i++) {
                if (i < 0 || i >= contentRectTransform.childCount) continue;

                var itemTfm = contentRectTransform.GetChild(i);
                var itemName = itemTfm.gameObject.name;
                if (!visibleItems.Contains(itemTfm.GetComponent<IPoolableScrollItem>())) {
                    var item = Pool.Get<IPoolableScrollItem>(itemName, viewPortRectTransform);
                    item.Initialize(i);
                    visibleItems.Add(item);
                }
            }
        }

        private bool IsItemVisible(IPoolableScrollItem item) {
            var itemBounds = GetBounds(item.GameObject.GetComponent<RectTransform>());
            var viewportBounds = GetBounds(viewPortRectTransform);

            return itemBounds.Intersects(viewportBounds);
        }

        private Bounds GetBounds(RectTransform rectTransform) {
            var corners = new Vector3[4];
            rectTransform.GetWorldCorners(corners);
            return new Bounds(corners[0] + new Vector3(corners[2].x - corners[0].x, corners[2].y - corners[0].y) * 0.5f, corners[2] - corners[0]);
        }

        private int CalculateStartIndex() {
            var contentHeight = contentRectTransform.rect.height;
            var itemHeight = itemPrefab.GetComponent<RectTransform>().rect.height;
            var viewPortHeight = viewPortRectTransform.rect.height;

            var normalizedScrollPosition = scrollRect.verticalNormalizedPosition;
            var totalHeight = contentHeight - viewPortHeight;
            var scrollHeight = totalHeight * normalizedScrollPosition;

            return Mathf.FloorToInt(scrollHeight / itemHeight).ClampMin(0);
        }

        private int CalculateEndIndex() {
            int childCount = contentRectTransform.childCount;
            var contentHeight = contentRectTransform.rect.height;
            var itemHeight = itemPrefab.GetComponent<RectTransform>().rect.height;
            var viewPortHeight = viewPortRectTransform.rect.height;

            var normalizedScrollPosition = scrollRect.verticalNormalizedPosition;
            var totalHeight = contentHeight - viewPortHeight;
            var scrollHeight = totalHeight * normalizedScrollPosition;
            
            return Mathf.FloorToInt((scrollHeight + viewPortHeight) / itemHeight).ClampMax(childCount - 1);
        }
    }
}