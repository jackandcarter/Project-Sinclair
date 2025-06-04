using UnityEngine;
using UnityEngine.EventSystems;

public class DockDropZone : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        DockIcon droppedIcon = eventData.pointerDrag.GetComponent<DockIcon>();
        if (droppedIcon != null)
        {
            // Re-parent to this drop zone (the dock or folder container).
            droppedIcon.transform.SetParent(transform);

            // Optionally reset scale or position so it fits your layout group.
            // e.g.: droppedIcon.GetComponent<RectTransform>().localScale = Vector3.one;

            // Let the DockManager reorder the children if present.
            DockManager dm = GetComponent<DockManager>();
            if (dm != null)
                dm.ReorderIcons();
        }
    }
}
