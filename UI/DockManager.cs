using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class DockManager : MonoBehaviour {

    // Reorders non‑static child icons by their anchored X positions,
    // while preserving the order of static icons.
    public void ReorderIcons() {
        int staticCount = 0;
        List<RectTransform> reorderableIcons = new List<RectTransform>();

        foreach (Transform child in transform) {
            DockIcon icon = child.GetComponent<DockIcon>();
            if (icon != null && icon.isStatic) {
                staticCount++;
            } else if (icon != null) {
                reorderableIcons.Add(child.GetComponent<RectTransform>());
            }
        }

        // Sort the reorderable icons from left to right.
        reorderableIcons = reorderableIcons.OrderBy(t => t.anchoredPosition.x).ToList();

        // Place the reorderable icons after the static ones.
        int index = staticCount;
        foreach (RectTransform r in reorderableIcons) {
            r.SetSiblingIndex(index);
            index++;
        }
    }
}
